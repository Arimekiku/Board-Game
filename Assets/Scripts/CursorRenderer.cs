using UnityEngine;

public class CursorRenderer : MonoBehaviour
{
    [SerializeField] private Sprite _defaultCursor;
    [SerializeField] private Sprite _activeCursor;
    [SerializeField] private SoundHandler _soundHandler;

    private Camera _mainCamera;
    private SpriteRenderer _renderer;

    private void Start()
    {
        Cursor.visible = false; 

        _mainCamera = Camera.main;
        _renderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        Vector2 cursorPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = cursorPosition;

        if (Input.GetMouseButtonDown(0))
        {
            _renderer.sprite = _activeCursor;
            _soundHandler.PlayClickSound();
        }

        if (Input.GetMouseButtonUp(0))  
            _renderer.sprite = _defaultCursor;
    }
}
