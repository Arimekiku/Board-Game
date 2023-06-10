using System;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Tile TileOnNode { get; private set; }
    public Vector2Int Coordinates { get; private set; }

    private SpriteRenderer _nodeRenderer;

    private readonly Color _unselected = new Color(Color.white.r, Color.white.g, Color.white.b, 0);
    private readonly Color _selected = new Color(Color.white.r, Color.white.g, Color.white.b, 50f / 255f);

    private void Awake() => _nodeRenderer = GetComponentInChildren<SpriteRenderer>();

    public void SetCoordinates(int x, int y) 
    { 
        if (x < 0 || y < 0)
            throw new IndexOutOfRangeException("Invalid coordinates");

        Coordinates = new Vector2Int(x, y);
    } 
    
    public void SetTile(Tile tile) => TileOnNode = tile;
    public void ClearTile() => TileOnNode = null;
    public void SetSelected() => _nodeRenderer.color = _selected;
    public void SetUnselected() => _nodeRenderer.color = _unselected;
}
