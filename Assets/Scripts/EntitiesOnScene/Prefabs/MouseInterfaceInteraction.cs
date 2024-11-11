using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MouseInterfaceInteraction : PrefabByComponentData
{
    //can decompose to several component with individual settings
    [SerializeField] private GameObject _tip;
    [SerializeField] private TextMeshProUGUI _tipText;
    [SerializeField] private UnityEvent<EntityData, string, WorldData> _executeCommandTouch;

    public EntityMonobeh RootMonobeh;

//    private EntityMonobeh _linkParent;
    private float _showTip;

    //    public EntityMonobeh EM => _linkParent;
    public override string KeyComponent => typeof(MouseInterfaceInteraction).Name;
    public override string KeyComponentData => typeof(ComponentInterractable).Name;
    internal override ComponentData GetComponentData => new ComponentInterractable();

    private void Awake()
    {
        _tip.SetActive(false);
    }

    public void Init(EntityMonobeh entityMonobeh, ComponentInterractable componentInterractable)
    {
//        _linkParent = entityMonobeh;
//        _tipText.text = componentInterractable.TipKey;

//        _tip.transform.rotation = Camera.main.transform.rotation;
        _tip.SetActive(false);
    }

    public override void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {
//        var componentInterractable = (ComponentInterractable)componentData;
//        _linkParent = GetComponentInParent<EntityMonobeh>();

//        _tipText.text = componentInterractable.TipKey;//can init several component

//        _tip.transform.rotation = Camera.main.transform.rotation;
//        _tip.SetActive(false);
    }

    public void OnClick(EntityMonobeh whoTouch)
    {
        RootMonobeh.EntityInProcess.SendCommand(new CommandData()
        {
            KeyCommand = KeyComponent,
            Message = $"{whoTouch}",
        });
/*        RootMonobeh.SendCommand(new CommandData()
        {
            KeyCommand = KeyComponent,
            Message = $"{whoTouch}",
        });/**/
    }

    public override void ExecuteCommand(EntityData entity, string message, WorldData worldData)
    {
        _executeCommandTouch?.Invoke(entity, message, worldData);
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