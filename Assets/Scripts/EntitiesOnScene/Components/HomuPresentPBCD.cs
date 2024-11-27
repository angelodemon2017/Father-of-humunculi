using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class HomuPresentPBCD : PrefabByComponentData
{
    [SerializeField] private UIIconPresent _uiIconPresentPrefab;
    [SerializeField] private Transform _parentSlots;
    [SerializeField] private BaseInventoryAdapter _baseInventoryAdapter;

    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private FSMController _fSMController;
    [SerializeField] private WaitFinishInteractState _stateWaiting;
    [SerializeField] private int SecondsToTransform = 2;
    [SerializeField] private List<RecipeSO> _itemsToUpgrade;
    [SerializeField] private TMP_Dropdown _dropdown;

    private ComponentHomu _component;
    private ComponentInventory _componentInventory;
    private EntityInProcess _entityInProcess;
    private List<UIIconPresent> _tempSlots = new();

    internal override bool _isNeedUpdate => true;
    public override string KeyComponent => typeof(HomuPresentPBCD).Name;
    public override string KeyComponentData => typeof(ComponentHomu).Name;
    internal override ComponentData GetComponentData => GetCompHomu();
    public override string GetDebugText => _component._titleDemo;

    private void Awake()
    {
        _dropdown.onValueChanged.AddListener(SelectFollowing);
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

    private void OnEnable()
    {
        var tempState = Instantiate(_stateWaiting);
        tempState.SetSpectator(gameObject, _fSMController.GetCurrentState);
        _fSMController.SetState(tempState, true);
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

    public override void DoSecond(EntityData entity)
    {
        var ch = entity.Components.GetComponent<ComponentHomu>();
        if (ch != null)
        {
            if (ch.IsNoType)
            {
                CheckInventory(ch, entity);
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

    public CommandData GetCommandSelectRole(int role)
    {
        return new CommandData()
        {
            KeyComponent = typeof(HomuPresentPBCD).Name,
            AddingKeyComponent = "",
            KeyCommand = Dict.Commands.SelectRole,
            Message = $"{role}",
        };
    }
}