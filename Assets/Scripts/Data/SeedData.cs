public class SeedData
{
    public string Seed;

    public string MapNoiseFrom10To99 => Seed.Substring(2, 2);

    public SeedData()
    {
        int shaderMask = UnityEngine.Random.Range(10, 100);
        int perlinNoise = UnityEngine.Random.Range(10, 100);

        Seed = $"{shaderMask}{perlinNoise}";
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