using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static OptimazeExtensions;

public class UsingByEntity : PrefabByComponentData
{
    public override int KeyType => TypeCache<UsingByEntity>.IdType;
    [SerializeField] private List<GameObject> _onOffsObjects;
    [SerializeField] private Button _closeButton;

    private ComponentUsingByEntity _component;
    private EntityInProcess _entityInProcess;

    internal override bool CanInterAct => _component.EntityId == -1;
    internal override bool _isNeedUpdate => true;
    internal bool _isOpen => _component.EntityId == UIPlayerManager.Instance.EntityMonobeh.Id;
    public override int KeyComponentData => TypeCache<ComponentUsingByEntity>.IdType;
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
                var comp = entity.GetComponent<ComponentUsingByEntity>();
                comp.SetEntityOpener(-1);
                break;
            default:
                break;
        }
    }

    public void Pick(EntityData entity, string command, string message, WorldData worldData)
    {
        var compPlayer = entity.GetComponent<ComponentUsingByEntity>();

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

    internal void Close()
    {
        if (_isOpen)
        {
            _onOffsObjects.ForEach(g => g.SetActive(false));
            _entityInProcess.SendCommand(GetCommandCloseUI());
        }
    }

    private CommandData GetCommandCloseUI()
    {
        return new CommandData()
        {
            KeyCommand = Dict.Commands.CloseUI,
            AddingKeyComponent = AddingKey,
            KeyComponent = KeyType,
        };
    }

    private void OnDestroy()
    {
        _closeButton.onClick.RemoveAllListeners();
    }
}