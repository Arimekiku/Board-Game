using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private TransitionHandler _transitionHandler;
    [Space, SerializeField] private Board _board;
    [SerializeField] private Vector2Int _boardSize;
    [Space, SerializeField] private Fireball _fireballSkill;
    [SerializeField] private Swap _swapSkill;
    [SerializeField] private Swipe _swipeSkill;

    private Skill _currentSkill;
    private Camera _mainCamera;
    private GameUI _gameUI;
    private DifficultySkaling _currentFun = new DifficultySkaling();
    private int _totalScore = 0;
    private float _currentSlowMotionTime = 0f;
    private bool _performingSlowMotion;
    private bool _isLost;

    private void Awake()
    {
        _gameUI = GetComponent<GameUI>();
        _mainCamera = Camera.main;

        _board.Initialize(_boardSize);
        _board.OnLose += Lose;
    }

    private void Update()
    {
        if (_performingSlowMotion)
        {
            _currentSlowMotionTime += Time.unscaledDeltaTime;

            _gameUI.UpdateSlowMotionBar(_currentSlowMotionTime);
        }
        else if (_currentSlowMotionTime > 0f) 
        {
            _currentSlowMotionTime -= Time.unscaledDeltaTime / 2f;

            _gameUI.UpdateSlowMotionBar(_currentSlowMotionTime);
        }

        if (Input.GetMouseButtonDown(0)) 
        {
            RaycastHit2D hit = Physics2D.Raycast(_mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        
            if (_currentSkill == null)
                OnDeselectedSkillClick(hit);
            else
                OnSelectedSkillClick(hit);
        }

        if (Input.GetMouseButtonDown(1) && _currentSkill != null)
        {
            _currentSkill.Deselect();
            _currentSkill = null;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 0.5f;

            _performingSlowMotion = true;
            _gameUI.SelectSlowMotionSkill();
        }

        if (Input.GetKeyUp(KeyCode.Space) || _currentSlowMotionTime > 4f)
        {
            Time.timeScale = 1f;

            _performingSlowMotion = false;
            _gameUI.DeselectSlowMotionSkill();
        }

        TrySelectSkill(_swipeSkill);
        TrySelectSkill(_swapSkill);
        TrySelectSkill(_fireballSkill);
    }

    public void Initialize()
    {
        gameObject.SetActive(true);

        _totalScore = 0;
        _currentSlowMotionTime = 0f;

        _gameUI.Initialize();
        _swipeSkill.Initialize();
        _swapSkill.Initialize();
        _fireballSkill.Initialize();
        _currentFun.Initialize();
    }

    public void StartGame()
    {
        _isLost = false;
        
        StartCoroutine(_board.SpawningCoroutine(_currentFun));
        StartCoroutine(_board.MovingCoroutine(_currentFun));
    }

    private void TrySelectSkill(Skill skillToSelect)
    {
        if (Input.GetKeyDown(skillToSelect.SelectCode) && skillToSelect.Uses > 0)
        {
            if (skillToSelect.IsSelected)
            {   
                _currentSkill.Deselect();
                _currentSkill = null;
            }
            else
            {
                _currentSkill?.Deselect();
                _currentSkill = skillToSelect;
                _currentSkill.Select();
            }
        }
    }

    private void OnSelectedSkillClick(RaycastHit2D hit) 
    {
        if (hit.collider == null) 
        {
            _currentSkill.Deselect();
            return;
        }
                    
        if (hit.collider.TryGetComponent(out Node node))
        {
            _currentSkill.Activate();
            _currentSkill = null;
        }
    }

    private void OnDeselectedSkillClick(RaycastHit2D hit) 
    {
        if (hit.collider == null)
            return;

        if (hit.collider.TryGetComponent(out Node node))
        {
            if (node.TileOnNode == null)
                return;

            if (node.TileOnNode.IsDestroyable || _performingSlowMotion == true) 
                StartCoroutine(BeginRemovingQueue(node.TileOnNode));
        }
    }

    private void UpdateScore(int score)
    {
        if (_isLost == true)
            return;

        _totalScore += score;
        _gameUI.UpdateScoreBar(_totalScore);
        _currentFun.Fun(_totalScore);
    }

    private void UpdateSkills(int combo)
    {
        if (combo > 5)
            _fireballSkill.IncreaseUses();
        else if (combo > 4)
            _swapSkill.IncreaseUses();
        else if (combo > 3)
            _swipeSkill.IncreaseUses();
    }

    private void Lose()
    {
        _board.StopAllCoroutines();
        StartCoroutine(_board.ClearNodes(_transitionHandler));

        _isLost = true;

        if (_totalScore > PlayerPrefs.GetInt("highScore"))
            PlayerPrefs.SetInt("highScore", _totalScore);
    }

    private IEnumerator BeginRemovingQueue(Tile tile)
    {   
        foreach (Tile tileToRemove in GetTilesToRemove(tile)) 
        {
            tileToRemove.Destroy();

            yield return new WaitForSeconds(0.05f);

            UpdateScore(tileToRemove.AmountOfPoints);
        }     
    }
    
    private HashSet<Tile> GetTilesToRemove(Tile firstTile) 
    {
        HashSet<Tile> tilesToRemove = new HashSet<Tile>();

        Queue<Tile> tilesInQueue = new Queue<Tile>();
        tilesInQueue.Enqueue(firstTile);
        Tile currentTile;

        while (tilesInQueue.Count > 0) 
        {
            currentTile = tilesInQueue.Dequeue();
            foreach (Tile neighbour in currentTile.Neighbours) 
                if (tilesToRemove.Contains(neighbour) is false) 
                    tilesInQueue.Enqueue(neighbour);

            if (currentTile.Type == firstTile.Type)
                tilesToRemove.Add(currentTile);
        }

        return tilesToRemove;
    }
}
