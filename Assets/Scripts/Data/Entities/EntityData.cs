﻿using System;
using System.Collections.Generic;

public class EntityData
{
    public long Id;
    public string TypeKey;
    
    public List<ComponentData> Components = new();
//    public PropsData Props = new();

    private Action<long> _updater;

    private EntitySO _cashConfig;
    public EntitySO Config
    {
        get 
        {
            if (_cashConfig == null)
            {
                _cashConfig = EntityController.GetEntity(TypeKey);
            }
            return _cashConfig;
        }   
    }
    internal WorldData worldData => GameProcess.Instance.GameWorld;
    internal string _DebugField = string.Empty;
    public virtual string DebugField => _DebugField;

    public UnityEngine.Vector3 Position 
    {
        get
        {
            var comp = Components.GetComponent<ComponentPosition>();
            return comp == null ? UnityEngine.Vector3.zero : comp.Position;
        }
    }

    public EntityData(string key, PropsData props)
    {
        TypeKey = key;
//        Props = props;
    }

    public EntityData(float xpos = 0, float zpos = 0)
    {
        Components.Add(new ComponentPosition(xpos, zpos));
    }

    public void SetUpdateAction(Action<long> updater)
    {
        _updater = updater;
    }

    internal void UpdateEntity()
    {
        Components.ForEach(c => c.UpdateAfterEntityUpdate(this));
        _updater?.Invoke(Id);
    }

    public virtual void ApplyCommand(CommandData command)
    {
        if (command.Component == typeof(ComponentPosition).Name)
        {
            var comp = Components.GetComponent<ComponentPosition>();
            comp.UpdateByCommand(command.Message);
        }
        if (command.Component == Dict.Commands.UseItem)
        {
            var compInv = Components.GetComponent<ComponentInventory>();
            if (compInv != null)
            {
                compInv.UseItem(int.Parse(command.Message), this);
            }
        }
        if (command.Component == Dict.Commands.SetBuild)
        {
            var compInv = Components.GetComponent<ComponentInventory>();
            if (compInv != null)
            {
                var mess = command.Message.Split('^');
                var recipe = RecipesController.GetRecipe(int.Parse(mess[0]));
                if (compInv.AvailableRecipe(recipe))
                {
                    compInv.SubtrackItemsByRecipe(recipe);

                    var newEntity = recipe.Build.GetEntityByRecipe(
                        float.Parse(mess[1]),
                        float.Parse(mess[2]));
                    GameProcess.Instance.GameWorld.AddEntity(newEntity);
                }
            }
        }

        UpdateEntity();
    }
}

public class EntityStartSpawnPoint : EntityData
{
    public EntityStartSpawnPoint(float xpos, float zpos) : base(xpos, zpos)
    {

    }
}

public class EntityCapacity : EntityData
{
    public EntityCapacity(int countComponents) : base()
    {
        for (int b = 0; b < countComponents; b++)
        {
            Components.Add(new ComponentCounter(100));
        }
    }
}