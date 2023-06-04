using System.Collections.Generic;
using UnityEngine;

public class WaveFunctionCollapse
{
    private readonly Dictionary<List<bool>, Sprite> _spriteHolder = new(new ListEqualityComparer());

    public void UpdateSpriteList(TileData data)
    {
        _spriteHolder.Clear();

        _spriteHolder.Add(new() { true, true, true, true }, data.BottomTopRightLeftSprite);

        _spriteHolder.Add(new() { true, true, true, false }, data.BottomTopLeftSprite);
        _spriteHolder.Add(new() { true, true, false, true }, data.BottomTopRightSprite);
        _spriteHolder.Add(new() { true, false, true, true }, data.TopLeftRightSprite);
        _spriteHolder.Add(new() { false, true, true, true }, data.BottomRightLeftSprite);

        _spriteHolder.Add(new() { true, true, false, false }, data.BottomTopSprite);
        _spriteHolder.Add(new() { true, false, true, false }, data.TopLeftSprite);
        _spriteHolder.Add(new() { false, true, true, false }, data.BottomLeftSprite);
        _spriteHolder.Add(new() { true, false, false, true }, data.TopRightSprite);
        _spriteHolder.Add(new() { false, true, false, true }, data.BottomRightSprite);
        _spriteHolder.Add(new() { false, false, true, true }, data.LeftRightSprite);

        _spriteHolder.Add(new() { true, false, false, false }, data.TopSprite);
        _spriteHolder.Add(new() { false, true, false, false }, data.BottomSprite);
        _spriteHolder.Add(new() { false, false, true, false }, data.LeftSprite);
        _spriteHolder.Add(new() { false, false, false, true }, data.RightSprite);

        _spriteHolder.Add(new() { false, false, false, false }, data.DefaultSprite);
    }

    public Sprite CheckValidatePair(List<bool> key)
    {
        if (_spriteHolder.TryGetValue(key, out Sprite sprite))
            return sprite;

        return null;
    }
}

public class ListEqualityComparer : IEqualityComparer<List<bool>>
{
    public bool Equals(List<bool> x, List<bool> y)
    {
        if (x.Count != y.Count) 
            return false;

        for (int i = 0; i < x.Count; i++) 
            if (x[i] != y[i])
                return false;

        return true;
    }

    public int GetHashCode(List<bool> obj)
    {
        unchecked
        {
            int hash = 19;
            foreach (var element in obj)
            {
                hash = hash * 31 + element.GetHashCode();
            }
            return hash;
        }
    }
}
