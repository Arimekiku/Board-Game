using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private TileTypes _type;
    [SerializeField] private TileData _data;
    [SerializeField] private ParticleSystem _particles;
    [SerializeField] private AudioSource _tileSource;

    private SpriteRenderer _renderer;
    private WaveFunctionCollapse _spriteHandler;
    private bool _excludePointSystem;

    private readonly float _translationTime = 0.1f;

    public event Action<int> OnDie;

    public int AmountOfPoints { get; private set; }
    public bool IsDestroyable { get; private set; }
    public HashSet<Tile> Neighbours { get; private set; } = new HashSet<Tile>();

    public TileTypes Type => _type;
    public WaveFunctionCollapse SpriteHandler => _spriteHandler;

    public void Initialize()
    {
        AmountOfPoints = 10;

        _spriteHandler = new WaveFunctionCollapse();
        _spriteHandler.UpdateSpriteList(_data);
        _renderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void MakeMove()
    {
        IsDestroyable = false;

        LeanTween.cancel(gameObject);
        transform.position = new Vector2(transform.position.x, Mathf.RoundToInt(transform.position.y));
        Vector3 endPosition = new Vector2(transform.position.x, transform.position.y - 1);

        //TODO: make tile change sprite after moving by reasonable way
        LeanTween.move(gameObject, endPosition, _translationTime).setEaseInOutSine();
    }

    public IEnumerator StartRemovingQueue()
    {   
        Destroy();
        OnDie.Invoke(AmountOfPoints);
        
        yield return new WaitForSeconds(0.05f);
        yield return IterateRemovingQueue(this, AmountOfPoints + 5);       
    }

    private IEnumerator ContinueRemovingQueue(Tile tile, int currentReward)
    {
        tile.Destroy();
        OnDie.Invoke(currentReward);

        yield return new WaitForSeconds(0.05f);
        yield return IterateRemovingQueue(tile, currentReward + 5);
    }

    private IEnumerator IterateRemovingQueue(Tile tile, int currentReward) 
    {
        HashSet<Tile> neighboursCopy = tile.Neighbours; 
        
        foreach (Tile neighbour in neighboursCopy) 
        {
            if (neighbour.Type == _type) 
            {
                yield return new WaitForSeconds(0.1f);
                StartCoroutine(ContinueRemovingQueue(neighbour, currentReward + 5));
            }  
        }    
    }

    public void Destroy()
    {
        //Clear the tile from all neighbour tiles lists
        foreach (Tile neighbour in Neighbours)
            neighbour.Neighbours.Remove(this);

        //Make all of the visual and audio stuff
        _particles.transform.SetParent(transform.parent);
        _particles.Play();
        _tileSource.Play();

        LeanTween.scale(gameObject, Vector3.zero, 0.2f).setDestroyOnComplete(true);
    }

    public void UpdateNeighbourList(HashSet<Tile> newNeighbours)
    {
        Neighbours = newNeighbours;

        foreach (Tile tile in Neighbours) 
            tile.Neighbours.Add(this);
    } 

    public void AllowDestroying() => IsDestroyable = true;
    public void PrioritizeOnDestroying() => _excludePointSystem = true;
    public void UpdateType(TileTypes newType) => _type = newType;
}