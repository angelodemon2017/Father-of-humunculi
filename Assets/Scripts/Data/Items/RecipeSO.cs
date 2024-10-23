using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Recipe", order = 1)]
public class RecipeSO : ScriptableObject
{
    [HideInInspector]
    public int Index;

    public ElementRecipe Result;

    public List<ElementRecipe> Resources = new();

    public GroupSO GroupRecipeTag;
}

[System.Serializable]
public class ElementRecipe
{
    public ItemConfig ItemConfig;
    public int Count;
}