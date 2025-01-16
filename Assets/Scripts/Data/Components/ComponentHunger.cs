using static OptimazeExtensions;

public class ComponentHunger : ComponentData
{
    public int Starvation;
    public int Saturation;

    public ComponentHunger() : base(TypeCache<ComponentHunger>.IdType) { }

    public bool ApplyHunger(int hunger)
    {
        if (Saturation > 0)
        {
            Saturation -= hunger;
            if (Saturation < 0)
            {
                Saturation = 0;
            }
        }
        else if(Starvation > 0)
        {
            Starvation -= hunger;
            if (Starvation < 0)
            {
                Starvation = 0;
            }
        }
        else
        {
            return false;
        }

        return true;
    }
}