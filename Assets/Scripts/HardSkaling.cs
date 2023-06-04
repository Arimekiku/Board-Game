public class HardSkaling
{
    private int _savedHardenValue = 0;

    private float _tileTranslationTime = 0.5f;
    private float _tileSpawnTime = 1f;
    private int _tilesToSpawn = 2;

    public int TilesToSpawn => _tilesToSpawn;
    public float TileSpawnTime => _tileSpawnTime;
    public float TileTranslationTime => _tileTranslationTime;

    public void Fun(int value)
    {
        if (value < _savedHardenValue || _savedHardenValue > 2000)
            return;

        if (value > 2000 && _savedHardenValue < 2000)
        {
            _tileTranslationTime -= 0.1f;
        }

        if (value > 1600 && _savedHardenValue < 1600)
        {
            _tileSpawnTime -= 0.25f;
        }

        if (value > 1200 && _savedHardenValue < 1200)
        {
            _tileSpawnTime -= 0.05f;
            _tileTranslationTime -= 0.05f;
        }

        if (value > 800 && _savedHardenValue < 800)
        {
            _tilesToSpawn++;
            _tileTranslationTime -= 0.05f;
        }

        if (value > 400 && _savedHardenValue < 400)
        {
            _tileSpawnTime -= 0.2f;
        }

        _savedHardenValue = value;
    }
}
