using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class HomuPresentPBCD : PrefabByComponentData
{
    [SerializeField] private UIIconPresent _uiIconPresentPrefab;
    [SerializeField] private Transform _parentSlots;
    [SerializeField] private BaseInventoryAdapter _baseInventoryAdapter;
    [SerializeField] private UsingByEntity _usingByEntity;

    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private FSMController _fSMController;
    [SerializeField] private int SecondsToTransform = 2;
    [SerializeField] private List<RecipeSO> _itemsToUpgrade;
    [SerializeField] private TMP_Dropdown _dropdownSelectFollow;
    [SerializeField] private TMP_Dropdown _dropdownSelectRole;

    private ComponentHomu _component;
    private ComponentInventory _componentInventory;
    private EntityInProcess _entityInProcess;
    private List<UIIconPresent> _tempSlots = new();

    internal override bool _isNeedUpdate => true;
    public override string KeyComponentData => typeof(ComponentHomu).Name;
    internal override ComponentData GetComponentData => GetCompHomu();
    public override string GetDebugText => _component._titleDemo;
    internal ComponentHomu Component => _component;

    private void Awake()
    {
        _dropdownSelectFollow.onValueChanged.AddListener(SelectFollowing);
        _dropdownSelectRole.onValueChanged.AddListener(SelectRole);
    }

    public override void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {
        _component = (ComponentHomu)componentData;
        _entityInProcess = entityInProcess;
        _componentInventory = _entityInProcess.EntityData.Components.GetComponent<ComponentInventory>(_baseInventoryAdapter.AddingKey);

        UpdateHomu();
        InitSlots();
        _baseInventoryAdapter.Init(_componentInventory, _entityInProcess);
        _baseInventoryAdapter.InitSlots(_tempSlots);
    }

    private void InitSlots()
    {
        _tempSlots.Clear();
        _parentSlots.DestroyChildrens();
        for (int i = 0; i < _componentInventory.MaxItems; i++)
        {
            var uicp = Instantiate(_uiIconPresentPrefab, _parentSlots);

            _tempSlots.Add(uicp);
        }
    }

    private ComponentHomu GetCompHomu()
    {
        //... ?
        return new ComponentHomu();
    }

    public override void ExecuteCommand(EntityData entity, string command, string message, WorldData worldData)
    {
        switch (command)
        {
            case Dict.Commands.SelectFollow:
                SetTargetFollow(entity, message, worldData);
                break;
            case Dict.Commands.SelectRole:
                SetRole(entity, message, worldData);
                break;
        }
    }

    private void SetTargetFollow(EntityData entity, string mes, WorldData worldData)
    {
        var cmpFSM = entity.Components.GetComponent<ComponentFSM>();
        if (cmpFSM != null)
        {
            var idEnt = long.Parse(mes);
            if (worldData.HaveEnt(idEnt))
            {
                cmpFSM.EntityTarget = idEnt;
            }
            else
            {
                cmpFSM.EntityTarget = -1;
                cmpFSM.SetPosFocus(entity.Position.x, entity.Position.z);
            }
            entity.UpdateEntity();
        }
    }

    private void SetRole(EntityData entity, string mes, WorldData worldData)
    {
        var cmpHom = entity.Components.GetComponent<ComponentHomu>();
        if (cmpHom != null)
        {
            var role = (EnumHomuRole)int.Parse(mes);
            cmpHom.HomuRole = role;
            entity.UpdateEntity();
        }
    }

    internal override void UpdateComponent()
    {
        UpdateHomu();
    }

    private void UpdateHomu()
    {
        _spriteRenderer.color = _component._colorModelDemo;
    }

    public void SelectFollowing(int select)
    {
        switch (select)
        {
            case 0:
                _entityInProcess.SendCommand(GetCommandSelectFollowing(-1));
                break;
            case 1:
                _entityInProcess.SendCommand(GetCommandSelectFollowing(UIPlayerManager.Instance.EntityMonobeh.Id));
                break;
            default:
                break;
        }
    }

    public void SelectRole(int select)
    {
        //TODO Need new solution
        _entityInProcess.SendCommand(GetCommandSelectRole((EnumHomuRole)select));
    }

    public override void DoSecond(EntityData entity)
    {
        var ch = entity.Components.GetComponent<ComponentHomu>();
        if (ch != null)
        {
            if (ch.IsNoType)
            {
                CheckInventory(ch, entity);
            }
            if (ch._idsInFocus.Count > 0 && ch.IdInFocus == -1)
            {
                var cmpFSM = entity.Components.GetComponent<ComponentFSM>();
                if (cmpFSM != null)
                {
                    List<EntityData> ents = new();
                    foreach (var id in ch._idsInFocus)
                    {
                        var tmpEnt = GameProcess.Instance.GameWorld.GetEntityById(id);
                        if (tmpEnt != null)
                        {
                            ents.Add(tmpEnt);
                        }
                    }
                    if (ents.Count > 0)
                    {
                        ents = ents.OrderBy(e => Vector3.Distance(e.Position, entity.Position)).ToList();
                        ch.IdInFocus = ents[0].Id;
                        ch._idsInFocus.Clear();
                        entity.UpdateEntity();
                    }
                }
            }
        }
    }

    private void CheckInventory(ComponentHomu ch, EntityData entity)
    {
        var invs = entity.Components.GetComponents(typeof(ComponentInventory).Name);
        foreach (ComponentInventory i in invs)
        {
            var rec = _itemsToUpgrade.FirstOrDefault(r => i.AvailableRecipe(r));
            if (rec != null)
            {
                if (ch._thinkingTime >= SecondsToTransform)
                {
                    i.SubtrackItemsByRecipe(rec);
                    ch.ApplyRecipe(rec);
                    entity.UpdateEntity();
                }
                else
                {
                    ch._thinkingTime += 1;
                    return;
                }
                break;
            }
        }
        ch._thinkingTime = 0;
    }

    public CommandData GetCommandSelectFollowing(long idFollow)
    {
        return new CommandData()
        {
            KeyComponent = typeof(HomuPresentPBCD).Name,
            AddingKeyComponent = "",
            KeyCommand = Dict.Commands.SelectFollow,
            Message = $"{idFollow}",
        };
    }

    public CommandData GetCommandSelectRole(EnumHomuRole role)
    {
        return new CommandData()
        {
            KeyComponent = typeof(HomuPresentPBCD).Name,
            AddingKeyComponent = "",
            KeyCommand = Dict.Commands.SelectRole,
            Message = $"{(int)role}",
        };
    }

    private void OnDestroy()
    {
        _dropdownSelectFollow.onValueChanged.RemoveAllListeners();
        _dropdownSelectRole.onValueChanged.RemoveAllListeners();
    }
}