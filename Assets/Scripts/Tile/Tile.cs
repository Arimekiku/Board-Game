using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private TileTypes _type;
    [SerializeField] private ParticleSystem _particles;
    [SerializeField] private AudioSource _tileSource;

    private SpriteRenderer _renderer;

    public int AmountOfPoints { get; private set; }
    public bool IsDestroyable { get; private set; }
    public List<Tile> Neighbours { get; private set; } = new List<Tile>();
    public TileTypes Type => _type;

    private const float TRANSLATION_TIME = 0.1f;

    public void Initialize()
    {
        AmountOfPoints = 10;

        _renderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void MakeMove()
    {
        IsDestroyable = false;

        LeanTween.cancel(gameObject);
        transform.position = new Vector2(transform.position.x, Mathf.RoundToInt(transform.position.y));
        Vector3 endPosition = new Vector2(transform.position.x, transform.position.y - 1);

        //TODO: make tile change sprite after moving by reasonable way
        LeanTween.move(gameObject, endPosition, TRANSLATION_TIME).setEaseInOutSine();
    }

    public void Destroy()
    {
        _particles.transform.SetParent(transform.parent);
        _particles.Play();
        _tileSource.Play();

        LeanTween.scale(gameObject, Vector3.zero, 0.2f).setDestroyOnComplete(true);
    }

    public void UpdateNeighbourList(List<Tile> newNeighbours)
    {
        Neighbours = newNeighbours;

        foreach (Tile tile in Neighbours) 
            tile.Neighbours.Add(this);
    } 

    public void AllowDestroying() => IsDestroyable = true;    
    public void UpdateType(TileTypes newType) => _type = newType;
}