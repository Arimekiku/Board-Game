using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private TransitionHandler _transitionHandler;
    [Space, SerializeField] private Board _board;
    [SerializeField] private Vector2Int _boardSize;
    [Space, SerializeField] private Fireball _fireballSkill;
    [SerializeField] private Swap _swapSkill;
    [SerializeField] private Swipe _swipeSkill;

    private Skill _selectedSkill;
    private Camera _mainCamera;
    private GameUI _gameUI;
    private HardSkaling _currentFun;
    private int _totalScore = 0;
    private float _currentSlowMotionTime = 0f;
    private bool _performingSlowMotion;
    private bool _isLost;

    public TimerBehaviour _lostTimer;

    private void Awake()
    {
        _lostTimer = new(60);
        _gameUI = GetComponent<GameUI>();
        _mainCamera = Camera.main;

        _board.Initialize(_boardSize);

        _lostTimer.OnTimerEnd += Lose;
        _board.OnTileDied += UpdateScore;
        _board.OnLose += Lose;
        _board.OnComboPerformed += UpdateSkills;
    }

    private void Update()
    {
        _lostTimer.UpdateTime(Time.deltaTime);

        if (_lostTimer.IsStopped == false)
            _gameUI.UpdateTimerBar(_lostTimer.RemainingTime);

        if (_performingSlowMotion)
        {
            _currentSlowMotionTime += Time.unscaledDeltaTime;

            _gameUI.UpdateSlowMotionBar(_currentSlowMotionTime);
        }

        if (_performingSlowMotion == false && _currentSlowMotionTime > 0f)
        {
            _currentSlowMotionTime -= Time.unscaledDeltaTime / 2f;

            _gameUI.UpdateSlowMotionBar(_currentSlowMotionTime);
        }

        if (Input.GetMouseButtonDown(0)) 
        {
            RaycastHit2D hit = Physics2D.Raycast(_mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        
            if (_selectedSkill == null)
                OnDeselectedSkillClick(hit);
            else
                OnSelectedSkillClick(hit);
        }

        if (Input.GetMouseButtonDown(1) && _selectedSkill != null)
        {
            _selectedSkill.Deselect();
            _selectedSkill = null;
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

    private void TrySelectSkill(Skill skillToSelect)
    {
        if (Input.GetKeyDown(skillToSelect.SelectCode) && skillToSelect.Uses > 0)
        {
            if (skillToSelect.IsSelected)
            {
                if (_selectedSkill != null)
                    _selectedSkill.Deselect();  
                _selectedSkill = null;
            }
            else
            {
                if (_selectedSkill != null)
                    _selectedSkill.Deselect();

                _selectedSkill = skillToSelect;
                _selectedSkill.Select();
            }
        }
    }

    private void OnSelectedSkillClick(RaycastHit2D hit) 
    {
        if (hit.collider == null) 
        {
            _selectedSkill.Deselect();
            return;
        }
                    
        if (hit.collider.TryGetComponent(out Node node))
        {
            _selectedSkill.Activate();
            _selectedSkill = null;
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
            {
                node.TileOnNode.OnDie += UpdateScore;
                StartCoroutine(node.TileOnNode.CheckOnRemovingQueue(node.TileOnNode));
            }  
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

        if (combo > 0)
            _lostTimer.AdjustTime(combo);

        if (combo == 0)
            _lostTimer.AdjustTime(-1);
    }

    private void Lose()
    {
        _board.StopAllCoroutines();
        StartCoroutine(_board.ClearNodes(_transitionHandler));

        _lostTimer.StopTimer();
        _isLost = true;

        if (_totalScore > PlayerPrefs.GetInt("highScore"))
            PlayerPrefs.SetInt("highScore", _totalScore);
    }

    public void ReloadGame()
    {
        _totalScore = 0;
        _currentSlowMotionTime = 0f;

        _gameUI.UpdateScoreBar(0);
        _gameUI.UpdateSlowMotionBar(0);
        _gameUI.UpdateTimerBar(60);

        _swipeSkill.Initialize();
        _swapSkill.Initialize();
        _fireballSkill.Initialize();
    }

    public void Initialize()
    {
        _isLost = false;
        _board.OnTileHitGround += StartTimer;

        _currentFun = new();

        StartCoroutine(_board.SpawningCoroutine(_currentFun));
        StartCoroutine(_board.MovingCoroutine(_currentFun));
    }

    private void StartTimer()
    {
        _lostTimer.RestartTimer();

        _board.OnTileHitGround -= StartTimer;
    }
}
