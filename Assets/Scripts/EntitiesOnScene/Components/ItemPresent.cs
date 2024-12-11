using System.Collections.Generic;
using UnityEngine;

public class ItemPresent : PrefabByComponentData
{
    [SerializeField] private List<ItemConfig> _itemsGenerate;
    [SerializeField] private RootSpriteRender _itemRoot;

    private ComponentItemPresent _componentItemPresent;

    internal override bool CanInterAct => true;
    public override string KeyComponentData => typeof(ComponentItemPresent).Name;
    internal ComponentItemPresent ComponentItem => _componentItemPresent;

    internal override ComponentData GetComponentData => new ComponentItemPresent(new ItemData(_itemsGenerate.GetRandom()));

    public override void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {
        _componentItemPresent = (ComponentItemPresent)componentData;
        _itemRoot.SetSprite(_componentItemPresent.ItemConfig.GetVariablesSprite((int)entityInProcess.Id));
    }

    public void PickEntity(EntityData entity, string command, string message, WorldData worldData)
    {
        var comItPr = entity.GetComponent<ComponentItemPresent>();

        var idEnt = long.Parse(message);

        var entUsed = worldData.GetEntityById(idEnt);

        var invs = entUsed.GetComponents<ComponentInventory>();
        foreach (ComponentInventory inv in invs)
        {
            if (inv.AvailableAddItem(comItPr.ItemData))
            {
                inv.AddItem(comItPr.ItemData);
                entUsed.UpdateEntity();

                worldData.RemoveEntity(entity.Id);
                break;
            }
        }
    }
}