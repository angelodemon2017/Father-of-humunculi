using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
/*    private List<string> _keyTriggers = new();
    private List<string> KeyTriggers 
    {
        get
        {
            if (_keyTriggers.Count == 0 && _itemsToUpgrade.Count != 0)
            {
                _keyTriggers = _itemsToUpgrade.Select(i => i.Key).ToList();
            }

            return _keyTriggers;
        }
    }/**/

    private ComponentHomu _component;
    private ComponentInventory _componentInventory;
    private EntityInProcess _entityInProcess;
    private List<UIIconPresent> _tempSlots = new();

    internal override bool _isNeedUpdate => true;
    public override string KeyComponent => typeof(HomuPresentPBCD).Name;
    public override string KeyComponentData => typeof(ComponentHomu).Name;
    internal override ComponentData GetComponentData => GetCompHomu();
    public override string GetDebugText => _component._titleDemo;

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

    internal override void UpdateComponent()
    {
        UpdateHomu();
    }

    private void UpdateHomu()
    {
        _spriteRenderer.color = _component._colorModelDemo;
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
}