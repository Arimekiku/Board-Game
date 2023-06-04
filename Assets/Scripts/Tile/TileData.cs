using UnityEngine;

[CreateAssetMenu(fileName = "Tile Data")]
public class TileData : ScriptableObject
{
    public Sprite DefaultSprite;

    public Sprite BottomSprite;
    public Sprite BottomLeftSprite;
    public Sprite BottomRightSprite;
    public Sprite BottomTopSprite;
    public Sprite BottomRightLeftSprite;
    public Sprite BottomTopRightSprite;
    public Sprite BottomTopLeftSprite;
    public Sprite BottomTopRightLeftSprite;

    public Sprite LeftSprite;
    public Sprite LeftRightSprite;

    public Sprite TopSprite;
    public Sprite TopLeftSprite;
    public Sprite TopRightSprite;
    public Sprite TopLeftRightSprite;

    public Sprite RightSprite;
}
