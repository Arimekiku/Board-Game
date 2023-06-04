using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private Tile _tileOnNode;

    private SpriteRenderer _nodeRenderer;
    private Vector2Int _coordinates;

    public Tile TileOnNode => _tileOnNode;
    public Vector2Int Coordinates => _coordinates;

    private void Awake()
    {
        _nodeRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void SetCoordinates(int x, int y) => _coordinates = new(x, y);
    public void SetTile(Tile tile) => _tileOnNode = tile;
    public void ClearTile() => _tileOnNode = null;
    public void SetSelected() => _nodeRenderer.color = new(_nodeRenderer.color.r, _nodeRenderer.color.g, _nodeRenderer.color.b, 50f / 255f);
    public void SetUnselected() => _nodeRenderer.color = new(_nodeRenderer.color.r, _nodeRenderer.color.g, _nodeRenderer.color.b, 0);
}
