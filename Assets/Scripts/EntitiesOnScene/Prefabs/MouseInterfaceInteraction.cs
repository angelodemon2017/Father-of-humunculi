using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class MouseInterfaceInteraction : PrefabByComponentData
{
    //can decompose to several component with individual settings
    [SerializeField] private GameObject _tip;
    [SerializeField] private TextMeshProUGUI _tipText;
    [SerializeField] private UnityEvent<EntityData, string, string, WorldData> _executeCommandTouch;
    [SerializeField] private List<PrefabByComponentData> _canInteractabler;

    public EntityMonobeh RootMonobeh;

    private float _showTip;

    internal override bool CanInterAct => _canInteractabler.Count > 0 ? _canInteractabler.Any(c => c.CanInterAct) : false;
    public override string KeyComponent => typeof(MouseInterfaceInteraction).Name;
    public override string KeyComponentData => typeof(ComponentInterractable).Name;
    internal override ComponentData GetComponentData => new ComponentInterractable();

    private void Awake()
    {
        _tip.SetActive(false);
    }

    public void OnClick(EntityMonobeh whoTouch)
    {
        if (RootMonobeh.EntityInProcess == null)
        {
            WorldViewer.Instance.RemoveEntity(RootMonobeh);
            Debug.Log($"Mysterious circumstances");
        }
        else
        {
            RootMonobeh.EntityInProcess.SendCommand(new CommandData()
            {
                KeyComponent = KeyComponent,
                AddingKeyComponent = AddingKey,
                Message = $"{whoTouch.Id}",
            });
        }
    }

    public override void ExecuteCommand(EntityData entity, string command, string message, WorldData worldData)
    {
        _executeCommandTouch?.Invoke(entity, command, message, worldData);
    }

    public void ShowTip()
    {
        if (CanInterAct)
        {
            _showTip = 0.1f;
            _tip.SetActive(true);
        }
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