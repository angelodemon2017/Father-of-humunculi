using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using static OptimazeExtensions;

public class MouseInterfaceInteraction : PrefabByComponentData
{
    public override int KeyType => TypeCache<MouseInterfaceInteraction>.IdType;
    //can decompose to several component with individual settings
    [SerializeField] private GameObject _tip;
    [SerializeField] private TextMeshProUGUI _tipText;
    [SerializeField] private UnityEvent<EntityData, string, string, WorldData> _executeCommandTouch;
    [SerializeField] private List<PrefabByComponentData> _canInteractabler;
    [SerializeField] private List<MapActions> _mapActions;
    [SerializeField] private string MessageHelp;
    [SerializeField] private ItemConfig ConfigForActionWithoutItem;
    [SerializeField] private bool _defaultAvailableInteract;//DEBUG field

    public EntityMonobeh RootMonobeh;

    private float _showTip;

    private string GetMessageHelp => string.IsNullOrWhiteSpace(MessageHelp) ? "..." : MessageHelp;
    internal override bool CanInterAct => _canInteractabler.Count > 0 ? _canInteractabler.Any(c => c.CanInterAct) : _defaultAvailableInteract;
    public override int KeyComponentData => TypeCache<ComponentInterractable>.IdType;
    internal override ComponentData GetComponentData => new ComponentInterractable();

    private void Awake()
    {
        _tip.SetActive(false);
    }

    public void AttackInteract(EntityMonobeh whoTouch)
    {
        RootMonobeh.EntityInProcess.SendCommand(new CommandData()
        {
            KeyComponent = KeyType,
            KeyCommand = Dict.Commands.MakeDamage,
            AddingKeyComponent = AddingKey,
            Message = $"{whoTouch.Id}",
        });
    }

    public void OnClick(EntityMonobeh whoTouch)
    {
        if (RootMonobeh.EntityInProcess == null)
        {
            WorldViewer.Instance.RemoveEntity(RootMonobeh);
            Debug.LogError($"Mysterious circumstances");
        }
        else
        {
            RootMonobeh.EntityInProcess.SendCommand(new CommandData()
            {
                KeyComponent = KeyType,
                AddingKeyComponent = AddingKey,
                Message = $"{whoTouch.Id}",
            });
        }
    }

    public override void ExecuteCommand(EntityData entity, string command, string message, WorldData worldData)
    {
        switch (command)
        {
            case Dict.Commands.MakeDamage:
                var entId = long.Parse(message);
                var whoTouched = worldData.GetEntityById(entId);
                AttackInteract(whoTouched, entity, worldData);
                break;
            default:
                UseMapAction(entity, message, worldData);
                break;
        }
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

    private void AttackInteract(EntityData whoTouched, EntityData targetTouch, WorldData worldData)
    {
        var whoConf = whoTouched.GetConfig;
        var targetConf = targetTouch.GetConfig;

        var damageCMP = whoConf.GetMyComponent<DamagerConfig>();
        var HPPCMP = targetConf.GetMyComponent<HealthPointConfig>();

        if (HPPCMP.GetDamage(targetTouch, damageCMP))
        {
            targetTouch.UpdateEntity();
        }
    }

    private void UseMapAction(EntityData entity, string message, WorldData worldData)
    {
        var entId = long.Parse(message);
        var whoTouched = worldData.GetEntityById(entId);
        foreach (var act in _mapActions)
        {
            if (act._needItems.Count == 0)
            {
                act._executeCommandTouch.Invoke(entity, string.Empty, GetMessageHelp, worldData);
                break;
            }
            else if (act._needItems.Any(ni => ni.Key == ConfigForActionWithoutItem.Key))
            {
                act._executeCommandTouch.Invoke(entity, string.Empty, message, worldData);
                break;
            }
            else
            {
                var invs = whoTouched.GetComponents<ComponentInventory>();
                if (act._needItems.Any(ni => invs.Any(i => i.GetCountOfItem(ni.Key) > 0)))
                {
                    act._executeCommandTouch.Invoke(entity, string.Empty, message, worldData);
                    break;
                }
            }
        }
    }

    [System.Serializable]
    private class MapActions
    {
        public List<ItemConfig> _needItems;
        public UnityEvent<EntityData, string, string, WorldData> _executeCommandTouch;
    }
}