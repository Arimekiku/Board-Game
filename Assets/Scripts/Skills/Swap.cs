using UnityEngine;

public class Swap : Skill
{
    [SerializeField] private TileData _redData;
    [SerializeField] private TileData _greenData;
    [SerializeField] private TileData _blueData;
    [SerializeField] private TileData _yellowData;

    private TileTypes _selectedType;

    public override void Initialize()
    {
        _uses = 2;
        _skillUI.UpdateAmount(_uses);
        _selectCode = KeyCode.Alpha2;
    }

    public override void Activate()
    {
        if (_nodesInRange[0].TileOnNode == null)
        {
            Deselect();
            return;
        }  

        _selectedType = CalculateType(_nodesInRange[0].TileOnNode);

        _nodesInRange[0].TileOnNode.UpdateType(_selectedType);
        switch (_selectedType)
        {
            case TileTypes.Green:
                _nodesInRange[0].TileOnNode.SpriteHandler.UpdateSpriteList(_greenData);
                break;
            case TileTypes.Blue:
                _nodesInRange[0].TileOnNode.SpriteHandler.UpdateSpriteList(_blueData);
                break;
            case TileTypes.Yellow:
                _nodesInRange[0].TileOnNode.SpriteHandler.UpdateSpriteList(_yellowData);
                break;
            case TileTypes.Red:
                _nodesInRange[0].TileOnNode.SpriteHandler.UpdateSpriteList(_redData);
                break;
        }

        //_nodesInRange[0].TileOnNode.UpdateSprite();

        _uses--;
        _skillUI.UpdateAmount(_uses);
        Deselect();
    }

    private TileTypes CalculateType(Tile tile)
    {
        int redCount = 0, blueCount = 0, yellowCount = 0, greenCount = 0;

        foreach (Tile neighbour in tile.Neighbours)
        {
            switch (neighbour.Type)
            {
                case TileTypes.Red: redCount++; break;
                case TileTypes.Blue: blueCount++; break;
                case TileTypes.Yellow: yellowCount++; break;
                case TileTypes.Green: greenCount++; break;
            }
        }

        int max = Mathf.Max(redCount, yellowCount, blueCount, greenCount);
        
        switch (tile.Type)
        {
            case TileTypes.Red: 
                if (max == redCount)
                {
                    Deselect();
                    return TileTypes.Red; 
                }
                break;
            case TileTypes.Blue:
                if (max == blueCount)
                {
                    Deselect();
                    return TileTypes.Blue;
                }
                    break; 
            case TileTypes.Yellow: 
                if (max == yellowCount)
                {
                    Deselect();
                    return TileTypes.Yellow;
                }
                break;
            case TileTypes.Green:
                if (max == greenCount)
                {
                    Deselect();
                    return TileTypes.Green;
                }
                break;
        }

        return max == redCount ? TileTypes.Red : max == greenCount ? TileTypes.Green : max == yellowCount ? TileTypes.Yellow : TileTypes.Blue;
    }

    protected override void UpdateSelectedList(Node onNode)
    {
        if (onNode == null)
            return;

        ClearSelectedList();

        _nodesInRange.Add(onNode);
        _nodesInRange.ForEach(n => n.SetSelected());
    }

    protected override void ShowSkill()
    {
        return;
    }
}
