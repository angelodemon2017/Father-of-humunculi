using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Recipe", order = 1)]
public class RecipeSO : ScriptableObject
{
    [HideInInspector]
    public int Index;

    public EntitySO _entitySOBuild;
    /// <summary>
    ///TODO DELETE ALL LOGIC
    /// </summary>
    public EnumBuilds Build = EnumBuilds.None;
    public Sprite IconBuild;

    public ElementRecipe Result;

    public List<ElementRecipe> Resources = new();

    public GroupSO GroupRecipeTag;

    public UIIconModel IconModelResult => IsBuild ? new UIIconModel(IconBuild) : new UIIconModel(Result);
    public string TitleRecipe => IsBuild ? $"{_entitySOBuild.Key}" : Result.ItemConfig.Key;
    public bool IsBuild => _entitySOBuild != null;
}

[System.Serializable]
public class ElementRecipe
{
    public ItemConfig ItemConfig;
    public int Count;
}