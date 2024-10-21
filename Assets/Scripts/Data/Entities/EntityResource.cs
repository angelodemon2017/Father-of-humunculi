using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityResource : EntityData
{
    public int IdResource;
    public int TestValue = 0;

    public override string DebugField => $"{(IdResource == 0 ? "камень" : "дерево")}({TestValue})";

    public EntityResource(int idResource, float xpos, float zpos) : base(xpos, zpos)
    {
        IdResource = idResource;

        Components.AddRange(new List<ComponentData>()
        {
            new ComponentCounter(50, UpperTestValue),
            new ComponentModelPrefab("PlaneBush"),
            new ComponentInterractable(GetTip),
            new ComponentUIlabels(Color.white)
        });
    }

    private string GetTip() => "*click*";

    private void UpperTestValue()
    {
        TestValue++;
        var com = Components.GetComponent<ComponentModelPrefab>();
        com.CurrentParamOfModel = TestValue;
        UpdateEntity();
    }

    public override void ApplyCommand(CommandData command)
    {
        if (command.Component == typeof(ComponentInterractable).Name)
        {
            var ent = worldData.entityDatas.FirstOrDefault(e => $"{e.Id}" == command.Message);
            var inv = ent.Components.GetComponent<ComponentInventory>();

            var iconf = ItemsController.GetItem(EnumItem.Star);
            var newid = new ItemData(iconf);
            newid.Count = TestValue;

            inv.AddItem(newid);
            ent.UpdateEntity();
            TestValue = 0;
            var com = Components.GetComponent<ComponentModelPrefab>();
            com.CurrentParamOfModel = TestValue;
            UpdateEntity();
            SpawnMob();
        }

        base.ApplyCommand(command);
    }

    private void SpawnMob()
    {
        GameProcess.Instance.GameWorld.AddEntity(new EntityMiniMob(Position.x, Position.z));
    }
}