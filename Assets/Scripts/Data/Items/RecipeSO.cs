using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Recipe", order = 1)]
public class RecipeSO : ScriptableObject
{
    [HideInInspector]
    public int Index;

    public EnumHomuType homuType;
    public EntityMonobeh _entityConfig;
    public EntitySO _entitySOBuild;
    public Sprite IconBuild;

    public ElementRecipe Result;

    public List<ElementRecipe> Resources = new();

    public GroupSO GroupRecipeTag;

    public UIIconModel IconModelResult => IsBuild ? new UIIconModel(IconBuild) : new UIIconModel(Result);
    public string TitleRecipe => IsBuild ? $"{_entityConfig.gameObject.name}" : Result.ItemConfig.Key;
    public bool IsBuild => _entityConfig != null;
}

[System.Serializable]
public class ElementRecipe
{
    public ItemConfig ItemConfig;
    public int Count;
}