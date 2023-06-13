using UnityEngine;

public class TransitionHandler : MonoBehaviour
{
    [SerializeField] private CanvasGroup _transitionBetweenPanels;
    [SerializeField] private Game _game;
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private GameObject _credits;

    public void LoadMenu()
    {
        _transitionBetweenPanels.blocksRaycasts = true;
        LeanTween.alphaCanvas(_transitionBetweenPanels, 1, 1f).setOnComplete(TurnOnMenu);
    }

    public void LoadGame()
    {
        _transitionBetweenPanels.blocksRaycasts = true;
        LeanTween.alphaCanvas(_transitionBetweenPanels, 1, 1f).setOnComplete(TurnOnGame);
    }

    public void LoadCredits()
    {
        _transitionBetweenPanels.blocksRaycasts = true;
        LeanTween.alphaCanvas(_transitionBetweenPanels, 1, 1f).setOnComplete(TurnOnCredits);
    }

    private void TurnOnGame()
    {
        _mainMenu.gameObject.SetActive(false);
        _game.Initialize();
        LeanTween.alphaCanvas(_transitionBetweenPanels, 0, 1f).setOnComplete(() => { _transitionBetweenPanels.blocksRaycasts = false; _game.StartGame(); });
    }

    private void TurnOnMenu()
    {
        _game.gameObject.SetActive(false);
        _credits.SetActive(false);
        _mainMenu.Initialize();
        LeanTween.alphaCanvas(_transitionBetweenPanels, 0, 1f).setOnComplete(() => _transitionBetweenPanels.blocksRaycasts = false);
    }

    private void TurnOnCredits()
    {
        _mainMenu.gameObject.SetActive(false);
        _credits.SetActive(true);
        LeanTween.alphaCanvas(_transitionBetweenPanels, 0, 1f).setOnComplete(() => _transitionBetweenPanels.blocksRaycasts = false);
    }
}
