public class EntityResource : EntityData
{
    public int IdResource;
    public int TestValue = 0;

    public override string DebugField => $"{(IdResource == 0 ? "камень" : "дерево")}({TestValue})";

    public EntityResource(int idResource, float xpos, float zpos) : base(xpos, zpos)
    {
        IdResource = idResource;
        Components.Add(new ComponentCounter(50, UpperTestValue));
    }

    private void UpperTestValue()
    {
        TestValue++;
        UpdateEntity();
    }

    public override void Touch(int paramTouch = 0)
    {
        TestValue = 0;
        UpdateEntity();
        GameProcess.Instance.GameWorld.AddEntity(new EntityMiniMob(Position.x, Position.z));
    }
}