using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Recipe", order = 1)]
public class RecipeSO : ScriptableObject
{
    public EnumBuilds Build = EnumBuilds.None;
    public Sprite IconBuild;

    public ElementRecipe Result;

    public List<ElementRecipe> Resources = new();

    public GroupSO GroupRecipeTag;

    public bool IsBuild => Build != EnumBuilds.None;
}

[System.Serializable]
public class ElementRecipe
{
    public ItemConfig ItemConfig;
    public int Count;
}