public class DifficultySkaling
{
    private int _savedHardenValue = 0;

    public float TileTranslationTime { get; private set; }
    public float TileSpawnTime { get; private set; }
    public int TilesToSpawn { get; private set; }

    public void Initialize() 
    {
        TileTranslationTime = 0.5f;
        TileSpawnTime = 1f;
        TilesToSpawn = 2;
    }

    public void Fun(int value)
    {
        if (value < _savedHardenValue || _savedHardenValue > 2000)
            return;

        if (value > 2000 && _savedHardenValue < 2000)
        {
            TileTranslationTime -= 0.1f;
        }

        if (value > 1600 && _savedHardenValue < 1600)
        {
            TileSpawnTime -= 0.25f;
        }

        if (value > 1200 && _savedHardenValue < 1200)
        {
            TileSpawnTime -= 0.05f;
            TileTranslationTime -= 0.05f;
        }

        if (value > 800 && _savedHardenValue < 800)
        {
            TilesToSpawn++;
            TileTranslationTime -= 0.05f;
        }

        if (value > 400 && _savedHardenValue < 400)
        {
            TileSpawnTime -= 0.2f;
        }

        _savedHardenValue = value;
    }
}