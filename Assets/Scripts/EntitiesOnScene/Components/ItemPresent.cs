using System.Collections.Generic;
using UnityEngine;

public class ItemPresent : PrefabByComponentData
{
    [SerializeField] private List<ItemConfig> _itemsGenerate;
    [SerializeField] private RootSpriteRender _itemRoot;

    public override string KeyComponent => typeof(ItemPresent).Name;
    public override string KeyComponentData => typeof(ComponentItemPresent).Name;

    internal override ComponentData GetComponentData => new ComponentItemPresent(new ItemData(_itemsGenerate.GetRandom()));

    public override void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {
        var _component = (ComponentItemPresent)componentData;
        _itemRoot.SetSprite(_component.ItemConfig.IconItem);
    }

    public void PickEntity(EntityData entity, string message, WorldData worldData)
    {
        var comItPr = entity.Components.GetComponent<ComponentItemPresent>();

        var idEnt = long.Parse(message);

        var entUsed = worldData.GetEntityById(idEnt);

        var invComp = entUsed.Components.GetComponent<ComponentInventory>();

        if (invComp != null)
        {
            invComp.AddItem(comItPr.ItemData);
            entUsed.UpdateEntity();

            worldData.RemoveEntity(entity.Id);
        }
    }
}