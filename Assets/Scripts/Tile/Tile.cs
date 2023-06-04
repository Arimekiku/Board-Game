using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private TileTypes _type;
    [SerializeField] private TileData _data;
    [SerializeField] private ParticleSystem _particlesGreen;
    [SerializeField] private ParticleSystem _particlesBlue;
    [SerializeField] private ParticleSystem _particlesRed;
    [SerializeField] private ParticleSystem _particlesYellow;
    [SerializeField] private AudioSource _tileSource;

    private List<Tile> _neighbours = new();
    private List<bool> _waveFunctionNeighbours = new() { false, false, false, false };
    private SpriteRenderer _renderer;
    private WaveFunctionCollapse _spriteHandler;
    private bool _excludePointSystem;

    private readonly float _translationTime = 0.1f;

    public event Action<int> OnDie;

    public int AmountOfPoints { get; private set; }
    public bool IsDestroyable { get; private set; }

    public TileTypes Type => _type;
    public WaveFunctionCollapse SpriteHandler => _spriteHandler;

    public void Initialize()
    {
        AmountOfPoints = 10;

        _spriteHandler = new();
        _spriteHandler.UpdateSpriteList(_data);
        _renderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void MakeMove()
    {
        IsDestroyable = false;

        LeanTween.cancel(gameObject);
        transform.position = new(transform.position.x, Mathf.RoundToInt(transform.position.y));
        Vector3 endPosition = new(transform.position.x, transform.position.y - 1);

        LeanTween.move(gameObject, endPosition, _translationTime).setEaseInOutSine().setOnComplete(() => UpdateSprite());
    }

    public IEnumerator CheckOnRemovingQueue(Tile tile)
    {
        tile.Destroy();
        OnDie.Invoke(AmountOfPoints);

        yield return new WaitForSeconds(0.05f);

        ClearNeighbours();

        foreach (Tile neighbour in _neighbours)
            if (neighbour.Type == _type)
                StartCoroutine(ContinueRemovingQueue(neighbour, AmountOfPoints + 5));
    }

    private IEnumerator ContinueRemovingQueue(Tile tile, int reward)
    {
        tile.Destroy();

        if (_excludePointSystem == false)
            OnDie.Invoke(reward);

        yield return new WaitForSeconds(0.05f);

        ClearNeighbours();

        foreach (Tile neighbour in _neighbours)
            if (neighbour.Type == _type)
                StartCoroutine(ContinueRemovingQueue(neighbour, reward + 5));
    }

    public void ClearNeighbours() 
    {
        foreach (Tile neighbour in _neighbours) 
            neighbour._neighbours.Remove(this);
    }

    public void Destroy()
    {
        //LeanTween.rotateZ(_renderer.gameObject, 360f, 0.1f).setOnComplete(() => Destroy(gameObject));
        LeanTween.scale(_renderer.gameObject, Vector3.zero, 0.2f).setOnComplete(() => Destroy(gameObject));

        switch (_type)
        {
            case TileTypes.Red: _particlesRed.Play(); break;
            case TileTypes.Blue: _particlesBlue.Play(); break;
            case TileTypes.Green: _particlesGreen.Play(); break;
            case TileTypes.Yellow: _particlesYellow.Play(); break;
        }

        _tileSource.Play();
    }

    public void AllowDestroying() => IsDestroyable = true;
    public void UpdateSprite() => SpriteHandler.CheckValidatePair(_waveFunctionNeighbours);
    public void PrioritizeOnDestroying() => _excludePointSystem = true;
    public void UpdateNeighbourList(List<bool> newNeighbours) => _waveFunctionNeighbours = newNeighbours;
    public void UpdateType(TileTypes newType) => _type = newType;
    public List<Tile> GetNeighbours() => new(_neighbours);
}