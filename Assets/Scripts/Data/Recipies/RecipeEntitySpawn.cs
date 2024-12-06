using UnityEngine;

[CreateAssetMenu(menuName = "Recipies/Recipe entity spawn", order = 1)]
public class RecipeEntitySpawn : RecipeSO
{
    public EntityMonobeh EntityConfig;
    public Sprite IconBuild;

    public override UIIconModel IconModelResult => new UIIconModel(this);
    public override string TitleRecipe => $"{EntityConfig.gameObject.name}";

    internal override void ReleaseRecipe(EntityData entityData, string args = "")
    {
        var mess = args.Split('^');
        var newEntity = EntityConfig.CreateEntity(float.Parse(mess[1]), float.Parse(mess[2]));

        GameProcess.Instance.GameWorld.AddEntity(newEntity);
    }
}