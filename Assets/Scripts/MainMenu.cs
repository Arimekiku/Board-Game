using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _highScore;

    private void Start() => UpdateHighScore();

    public void UpdateHighScore()
    {
        if (PlayerPrefs.HasKey("highScore"))
        {
            _highScore.enabled = true;
            _highScore.text = "Highscore: " + PlayerPrefs.GetInt("highScore");
        } 
        else
        {
            _highScore.enabled = false;
        }
    }
}
