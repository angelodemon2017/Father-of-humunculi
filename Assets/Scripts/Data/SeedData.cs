
public class SeedData
{
    public string Seed;

    public SeedData()
    {
        int shaderMask = UnityEngine.Random.Range(10, 99);

        Seed = $"{shaderMask}";
    }

    public SeedData(string seed)
    {
        Seed = seed;
    }

    public int GetShaderMask()
    {
        return int.Parse(Seed.Substring(0, 2));
    }
}