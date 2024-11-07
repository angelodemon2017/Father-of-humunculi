using TMPro;
using UnityEngine;

public class MouseInterfaceInteraction : PrefabByComponentData
{
    //can decompose to several component with individual settings
    [SerializeField] private GameObject _tip;
    [SerializeField] private TextMeshProUGUI _tipText;

    private EntityMonobeh _linkParent;
    private float _showTip;

    public override string KeyComponent => typeof(ComponentInterractable).Name;
    public EntityMonobeh EM => _linkParent;

    public void Init(EntityMonobeh entityMonobeh, ComponentInterractable componentInterractable)
    {
        _linkParent = entityMonobeh;
        _tipText.text = componentInterractable.TipKey;

        _tip.transform.rotation = Camera.main.transform.rotation;
        _tip.SetActive(false);
    }

    public override void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {
        var componentInterractable = (ComponentInterractable)componentData;
        _linkParent = GetComponentInParent<EntityMonobeh>();

        _tipText.text = componentInterractable.TipKey;//can init several component

        _tip.transform.rotation = Camera.main.transform.rotation;
        _tip.SetActive(false);
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