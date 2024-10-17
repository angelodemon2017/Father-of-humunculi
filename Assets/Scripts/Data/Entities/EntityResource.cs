public class EntityResource : EntityData
{
    public int IdResource;
    public int TestValue = 0;

    public override string DebugField => $"{(IdResource == 0 ? "камень" : "дерево")}({TestValue})";

    public EntityResource(int idResource, float xpos, float zpos) : base(xpos, zpos)
    {
        IdResource = idResource;
        Components.Add(new ComponentCounter(50, UpperTestValue));
        Components.Add(new ComponentModelPrefab("PlaneBush"));
        Components.Add(new ComponentInterractable("*click*"));
        Components.Add(new ComponentUIlabels());
//        Components.Add(new ComponentCounter(10, SpawnMob));
    }

    private void UpperTestValue()
    {
        TestValue++;
        var com = Components.GetComponent<ComponentModelPrefab>();
        com.CurrentParamOfModel = TestValue;
        UpdateEntity();
    }

    public override void ApplyCommand(CommandData command)
    {
        if (command.Component == "")
        {
            TestValue = 0;
            var com = Components.GetComponent<ComponentModelPrefab>();
            com.CurrentParamOfModel = TestValue;
            UpdateEntity();
            SpawnMob();
        }

        base.ApplyCommand(command);
    }

    private void SpawnMob()
    {
        GameProcess.Instance.GameWorld.AddEntity(new EntityMiniMob(Position.x, Position.z));
    }

    public override CommandData GetCommand(string parametr)
    {
        return TouchCommand();
    }

    public CommandData TouchCommand()
    {
        return new CommandData(Id, "");
    }
}