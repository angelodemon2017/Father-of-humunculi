using UnityEngine;

public class HomuPresentPBCD : PrefabByComponentData
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private FSMController _fSMController;
    [SerializeField] private WaitFinishInteractState _stateWaiting;

    private ComponentHomu _component;

    public override string KeyComponent => typeof(HomuPresentPBCD).Name;
    public override string KeyComponentData => typeof(ComponentHomu).Name;
    internal override ComponentData GetComponentData => GetCompHomu();
    public override string GetDebugText => _component._titleDemo;

    public override void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {
        _component = (ComponentHomu)componentData;

        UpdateHomu();
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

    private void UpdateHomu()
    {
        _spriteRenderer.color = _component._colorModelDemo;
    }
}