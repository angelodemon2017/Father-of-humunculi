﻿using UnityEngine;

[CreateAssetMenu(menuName = "Recipies/Recipe item", order = 1)]
public class RecipeItem : RecipeSO
{
    public ElementRecipe ItemResult;

    private ItemData GetItem()
    {
        var resultItem = new ItemData(ItemResult.ItemConfig);
        resultItem.Count = ItemResult.Count;
        return resultItem;
    }
    public override UIIconModel IconModelResult => new UIIconModel(ItemResult);
    public override string TitleRecipe => $"{ItemResult.ItemConfig.Key}";

    internal override void ReleaseRecipe(EntityData entityData, string args = "")
    {
        var cmpInv = entityData.Components.GetComponent<ComponentInventory>();
        cmpInv.AddItem(GetItem());
        entityData.UpdateEntity();
    }
}