using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    [SerializeField] protected GameObject _slashPrefab;

    private Node _previousSelectedNode;
    private Camera _mainCamera;

    protected int _uses;
    protected bool _isSelected;
    protected List<Node> _nodesInRange = new List<Node>();
    protected SkillUI _skillUI;
    protected KeyCode _selectCode;
    protected Board _board;

    public KeyCode SelectCode => _selectCode;
    public int Uses => _uses;
    public bool IsSelected => _isSelected;

    private void Awake()
    {
        _skillUI = GetComponent<SkillUI>();
        _board = FindObjectOfType<Board>();
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (_isSelected == true)
            TryShowSkill();
    }

    private void TryShowSkill() 
    {
        RaycastHit2D hit = Physics2D.Raycast(_mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider == null) 
        {
            ClearSelectedList();
            return;
        }

        if (hit.collider.TryGetComponent(out Node node))
        {
            if (_previousSelectedNode != node)
            {
                _previousSelectedNode = node;
                UpdateSelectedList(_previousSelectedNode);
            }
        }
    }

    protected void ClearSelectedList()
    {
        _nodesInRange.ForEach(n => n.SetUnselected());
        _nodesInRange.Clear();
    }

    public void IncreaseUses()
    {
        _uses++;
        _skillUI.UpdateAmount(_uses);
    }

    public void Select()
    {
        _isSelected = true;
        _skillUI.SelectSkill();
    }

    public void Deselect()
    {
        _isSelected = false;
        _skillUI.DeselectSkill();
        ClearSelectedList();
    }

    public abstract void Initialize();
    public abstract void Activate();

    protected abstract void ShowSkill();
    protected abstract void UpdateSelectedList(Node node);
}
