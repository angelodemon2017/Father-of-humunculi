using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MouseInterfaceInteraction : PrefabByComponentData
{
    //can decompose to several component with individual settings
    [SerializeField] private GameObject _tip;
    [SerializeField] private TextMeshProUGUI _tipText;
    [SerializeField] private UnityEvent<EntityData, string, string, WorldData> _executeCommandTouch;

    public EntityMonobeh RootMonobeh;

    private float _showTip;

    public override string KeyComponent => typeof(MouseInterfaceInteraction).Name;
    public override string KeyComponentData => typeof(ComponentInterractable).Name;
    internal override ComponentData GetComponentData => new ComponentInterractable();

    private void Awake()
    {
        _tip.SetActive(false);
    }

    public void OnClick(EntityMonobeh whoTouch)
    {
        RootMonobeh.EntityInProcess.SendCommand(new CommandData()
        {
            KeyComponent = KeyComponent,
            AddingKeyComponent = AddingKey,
            Message = $"{whoTouch.Id}",
        });
    }

    public override void ExecuteCommand(EntityData entity, string command, string message, WorldData worldData)
    {
        _executeCommandTouch?.Invoke(entity, command, message, worldData);
    }

    public void ShowTip()
    {
        _showTip = 0.1f;
        _tip.SetActive(true);
    }

    private void FixedUpdate()
    {
        if (_showTip > 0)
        {
            _showTip -= Time.fixedDeltaTime;
            if (_showTip <= 0)
            {
                _tip.SetActive(false);
            }
        }
    }
}