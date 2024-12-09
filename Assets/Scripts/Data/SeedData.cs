public class SeedData
{
    public string Seed;

    private int _mapPerlinNoise = 0;
    private int _mapLayer2 = 0;

    public int MapNoise
    {
        get
        {
            return _mapPerlinNoise;
        }
    }
    public int MapLayer2
    {
        get
        {
            return _mapLayer2;
        }
    }

    public SeedData()
    {
        int shaderMask = UnityEngine.Random.Range(10, 100);
        int perlinNoise = UnityEngine.Random.Range(10, 100);
        int mapLayer2 = UnityEngine.Random.Range(10, 100);

        Seed = $"{shaderMask}{perlinNoise}{mapLayer2}";

        InitCash();
    }

    public SeedData(string seed)
    {
        Seed = seed;

        InitCash();
    }

    private void InitCash()
    {
        _mapPerlinNoise = int.Parse(Seed.Substring(2, 2));
        _mapLayer2 = int.Parse(Seed.Substring(4, 2));
    }

    public int GetShaderMask()
    {
        return int.Parse(Seed.Substring(0, 2));
    }
}