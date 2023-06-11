using UnityEngine;

public class Swipe : Skill
{
    public override void Initialize()
    {
        _uses = 3;
        _skillUI.UpdateAmount(_uses);
        _selectCode = KeyCode.Alpha1;
    }

    public override void Activate()
    {
        ShowSkill();

        foreach (Node node in _nodesInRange) 
        {
            node.TileOnNode?.Destroy();
            node.ClearTile();
        }

        _uses--;
        _skillUI.UpdateAmount(_uses);
        Deselect();
    }

    protected override void UpdateNodeList(Node currentNode)
    {
        ClearSelectedArea();

        for (int x = 0; x < _board.BoardSize.x; x++) 
            _nodesInRange.Add(_board.TryGetNode(x, currentNode.Coordinates.y));

        _nodesInRange.ForEach(n => n.SetSelected());
    }

    protected override void ShowSkill()
    {
        Node node = _nodesInRange[0];

        GameObject slash = Instantiate(_slashPrefab);
        slash.transform.SetParent(transform);
        slash.transform.position = node.transform.position;
        LeanTween.move(slash, slash.transform.position + new Vector3(12f, 0f), 0.2f).setEaseInOutSine();
    }
}
