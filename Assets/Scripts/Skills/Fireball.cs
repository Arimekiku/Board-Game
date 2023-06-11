using UnityEngine;

public class Fireball : Skill
{
    public override void Initialize()
    {
        _uses = 1;
        _skillUI.UpdateAmount(_uses);
        _selectCode = KeyCode.Alpha3;
    }

    public override void Activate()
    {
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
        if (currentNode == null)
            return;

        ClearSelectedArea();

        _nodesInRange.Add(currentNode);
        Vector2Int nodePosition = currentNode.Coordinates;

        if (currentNode.Coordinates.y < _board.BoardSize.y - 1) 
        {
            _nodesInRange.Add(_board.TryGetNode(nodePosition.x, nodePosition.y + 1));

            if (currentNode.Coordinates.y < _board.BoardSize.y - 2)
                _nodesInRange.Add(_board.TryGetNode(nodePosition.x, nodePosition.y + 2));
        }

        if (currentNode.Coordinates.y > 0) 
        {
            _nodesInRange.Add(_board.TryGetNode(nodePosition.x, nodePosition.y - 1));

            if (currentNode.Coordinates.y > 1)
                _nodesInRange.Add(_board.TryGetNode(nodePosition.x, nodePosition.y - 2));
        }

        if (currentNode.Coordinates.x < _board.BoardSize.x - 1) 
        {
            _nodesInRange.Add(_board.TryGetNode(nodePosition.x + 1, nodePosition.y));

            if (currentNode.Coordinates.x < _board.BoardSize.x - 2)
                _nodesInRange.Add(_board.TryGetNode(nodePosition.x + 2, nodePosition.y));
        }

        if (currentNode.Coordinates.x > 0) 
        {
            _nodesInRange.Add(_board.TryGetNode(nodePosition.x - 1, nodePosition.y));

            if (currentNode.Coordinates.y > 1)
                _nodesInRange.Add(_board.TryGetNode(nodePosition.x - 2, nodePosition.y));
        }

        _nodesInRange.ForEach(n => n.SetSelected());
    }

    protected override void ShowSkill()
    {
        Node node = _nodesInRange[0];

        GameObject slash = Instantiate(_slashPrefab);
        slash.transform.SetParent(transform);
        slash.transform.position = new(node.transform.position.x + 0.2f, node.transform.position.y);
        LeanTween.move(slash, slash.transform.position + new Vector3(2f, 0f), 0.2f);

        slash = Instantiate(_slashPrefab);
        slash.transform.SetParent(transform);
        slash.transform.position = new(node.transform.position.x - 0.2f, node.transform.position.y);
        LeanTween.move(slash, slash.transform.position + new Vector3(-2f, 0f), 0.2f);

        slash = Instantiate(_slashPrefab);
        slash.transform.SetParent(transform);
        slash.transform.position = new(node.transform.position.x, node.transform.position.y + 0.2f);
        LeanTween.move(slash, slash.transform.position + new Vector3(0f, 2f), 0.2f);

        slash = Instantiate(_slashPrefab);
        slash.transform.SetParent(transform);
        slash.transform.position = new(node.transform.position.x, node.transform.position.y - 0.2f);
        LeanTween.move(slash, slash.transform.position + new Vector3(0f, -2f), 0.2f);
    }
}
