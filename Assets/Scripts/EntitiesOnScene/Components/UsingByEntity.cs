using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsingByEntity : PrefabByComponentData
{
    [SerializeField] private List<GameObject> _onOffsObjects;
    [SerializeField] private Button _closeButton;

    private ComponentUsingByEntity _component;
    private EntityInProcess _entityInProcess;

    internal override bool _isNeedUpdate => true;
    private bool _isOpen => _component.EntityId == UIPlayerManager.Instance.EntityMonobeh.Id;
    public override string KeyComponent => typeof(UsingByEntity).Name;
    public override string KeyComponentData => typeof(ComponentUsingByEntity).Name;
    internal override ComponentData GetComponentData => new ComponentUsingByEntity();

    public override void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {
        _component = (ComponentUsingByEntity)componentData;
        _entityInProcess = entityInProcess;
        _closeButton.onClick.AddListener(Close);
        Close();
    }

    public override void ExecuteCommand(EntityData entity, string command, string message, WorldData worldData)
    {
        switch (command)
        {
            case Dict.Commands.CloseUI:
                var comp = entity.Components.GetComponent<ComponentUsingByEntity>();
                comp.SetEntityOpener(-1);
                break;
            default:
                break;
        }
    }

    public void Pick(EntityData entity, string command, string message, WorldData worldData)
    {
        var compPlayer = entity.Components.GetComponent<ComponentUsingByEntity>();

        if (compPlayer != null)
        {
            compPlayer.SetEntityOpener(long.Parse(message));
            entity.UpdateEntity();
        }
    }

    internal override void UpdateComponent()
    {
        _onOffsObjects.ForEach(g => g.SetActive(_isOpen));
    }

    private void FixedUpdate()
    {
        if (_isOpen)
        {
            if (Vector3.Distance(transform.position, UIPlayerManager.Instance.EntityMonobeh.transform.position) > Config.CloseUIDistance)
            {
                Close();
            }
        }
    }

    private void Close()
    {
        _onOffsObjects.ForEach(g => g.SetActive(false));
        _entityInProcess.SendCommand(GetCommandCloseUI());
    }

    private CommandData GetCommandCloseUI()
    {
        return new CommandData()
        {
            KeyCommand = Dict.Commands.CloseUI,
            KeyComponent = KeyComponent,
        };
    }

    private void OnDestroy()
    {
        _closeButton.onClick.RemoveAllListeners();
    }
}