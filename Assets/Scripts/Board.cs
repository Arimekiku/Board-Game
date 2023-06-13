using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private Node _nodePrefab;
    [SerializeField] private Tile[] _tilePrefabs;
    [SerializeField] private SoundHandler _soundHandler;
    
    private Node[,] _nodes;
    private Vector2Int _boardSize;

    private readonly List<Tile> _tilesToInitialize = new();
    private readonly Vector2Int[] _directions = new Vector2Int[4] 
    {
        Vector2Int.left,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.up
    };

    public event Action<int> OnComboPerformed;
    public event Action OnLose;

    private Vector2 Offset => new Vector2((_boardSize.x - 1) * 0.5f, -1 * 0.5f);
    public Vector2Int BoardSize => _boardSize;

    public void Initialize(Vector2Int boardSize)
    {
        _boardSize = boardSize;
        _nodes = new Node[_boardSize.x, _boardSize.y];

        for (int y = 0; y < boardSize.y; y++)
        {
            for (int x = 0; x < boardSize.x; x++)
            {
                Node node = _nodes[x, y] = Instantiate(_nodePrefab, transform);
                node.transform.localPosition = new Vector2(x - Offset.x, y - Offset.y);
                node.name = "Node " + x + " " + y;
                node.SetCoordinates(x, y);
            }
        }
    }

    public IEnumerator ClearNodes(TransitionHandler transitionHandler)
    {
        //Clear all tiles that about to spawn
        foreach (Tile tile in _tilesToInitialize) 
            tile.Destroy();

        _tilesToInitialize.Clear();

        //Clear all tiles on field
        foreach (Node node in GetNodesWithTile())
        {
            node.TileOnNode.Destroy();

            yield return new WaitForSeconds(0.05f);

            node.ClearTile();
        }

        //Load main menu
        transitionHandler.LoadMenu();
    }

    public IEnumerator SpawningCoroutine(DifficultySkaling currentFun)
    {
        while (true)
        {
            yield return new WaitForSeconds(currentFun.TileSpawnTime);

            SpawnTilesOnBoard(currentFun.TilesToSpawn);
        }
    }

    public IEnumerator MovingCoroutine(DifficultySkaling currentFun)
    {
        while (true)
        {
            yield return new WaitForSeconds(currentFun.TileTranslationTime);

            MoveTilesOnBoard();
            InitializeTilesOnBoard();

            CalculateLoseOption();
        }
    }

    private void SpawnTilesOnBoard(int numberOfTiles)
    {
        for (int i = 0; i < numberOfTiles; i++)
        {
            int index = UnityEngine.Random.Range(0, _tilePrefabs.Length);

            Tile tile = Instantiate(_tilePrefabs[index]);
            tile.transform.SetParent(transform);
            tile.Initialize();

            _tilesToInitialize.Add(tile);
        }   
    }

    private void MoveTilesOnBoard()
    {
        for (int y = 1; y < _boardSize.y; y++) 
        {
            for (int x = 0; x < _boardSize.x; x++) 
            {
                if (_nodes[x, y - 1].TileOnNode != null)
                    continue;

                if (_nodes[x, y].TileOnNode != null) 
                {
                    _nodes[x, y].TileOnNode.MakeMove();
                    _nodes[x, y - 1].SetTile(_nodes[x, y].TileOnNode);
                    _nodes[x, y].ClearTile();

                    if (_nodes[x, 0] == _nodes[x, y - 1])
                    {
                        _nodes[x, y - 1].TileOnNode.AllowDestroying();
                        _soundHandler.PlayTileCollidesWallSound();
                        UpdateTileNode(_nodes[x, y - 1]);
                    } 
                    else if (_nodes[x, y - 2].TileOnNode != null)
                    {
                        _nodes[x, y - 1].TileOnNode.AllowDestroying();
                        _soundHandler.PlayTileCollidesTileSound();
                        UpdateTileNode(_nodes[x, y - 1]);
                    }
                }
            }
        }
    }

    private void UpdateTileNode(Node node) 
    {
        List<Tile> checker = new();

        foreach (Vector2Int direction in _directions) 
        {
            Node neighbour = TryGetNode(node.Coordinates + direction);

            if (neighbour?.TileOnNode is not null) 
                checker.Add(neighbour.TileOnNode);             
        }

        node.TileOnNode.UpdateNeighbourList(checker);
    }

    private void InitializeTilesOnBoard()
    {
        foreach (Tile tile in _tilesToInitialize)
        {
            Vector2Int randomNodesColumn = new(UnityEngine.Random.Range(0, _nodes.GetLength(0)), _nodes.GetLength(1) - 1);
            while (_nodes[randomNodesColumn.x, randomNodesColumn.y].TileOnNode != null)
                randomNodesColumn = new(UnityEngine.Random.Range(0, _nodes.GetLength(0)), _nodes.GetLength(1) - 1);

            tile.transform.localPosition = randomNodesColumn - Offset;
            _nodes[randomNodesColumn.x, randomNodesColumn.y].SetTile(tile);

            tile.MakeMove();
        }

        _tilesToInitialize.Clear();
    }

    private void CalculateLoseOption()
    {
        for (int x = 0; x < _boardSize.x; x++)
            if (_nodes[x, _boardSize.y - 1].TileOnNode == null)
                return;

        OnLose.Invoke();
    }

    public Node TryGetNode(Tile tile)
    {
        foreach (Node node in _nodes)
            if (tile == node.TileOnNode)
                return node;

        throw new ArgumentException();
    }

    public Node TryGetNode(int x, int y) 
    {
        if (x >= _boardSize.x || x < 0)
            return null;
        
        if (y >= _boardSize.y || y < 0)
            return null;
        
        return _nodes[x, y];
    }

    public Node TryGetNode(Vector2Int coord) 
    {
        if (coord.x >= _boardSize.x || coord.x < 0)
            return null;
        
        if (coord.y >= _boardSize.y || coord.y < 0)
            return null;
        
        return _nodes[coord.x, coord.y];
    }

    public List<Node> GetNodesWithTile()
    {
        List<Node> nodesWithTile = new();

        foreach (Node node in _nodes)
            if (node.TileOnNode != null)
                nodesWithTile.Add(node);

        return nodesWithTile;
    }
}
