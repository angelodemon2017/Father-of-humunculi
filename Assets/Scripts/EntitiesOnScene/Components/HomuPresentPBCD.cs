using System.Collections.Generic;
using UnityEngine;

public class HomuPresentPBCD : PrefabByComponentData
{
    [SerializeField] private UIIconPresent _uiIconPresentPrefab;
    [SerializeField] private Transform _parentSlots;
    [SerializeField] private BaseInventoryAdapter _baseInventoryAdapter;

    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private FSMController _fSMController;
    [SerializeField] private WaitFinishInteractState _stateWaiting;

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
}