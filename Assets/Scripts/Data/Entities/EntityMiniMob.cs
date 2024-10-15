public class EntityMiniMob : EntityData
{
    public EntityMiniMob(float xpos, float zpos) : base(xpos, zpos)
    {
        Components.Add(new ComponentModelPrefab("MiniMob"));
    }
}