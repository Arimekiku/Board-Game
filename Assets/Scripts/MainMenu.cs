using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _highScore;
    [SerializeField] private SoundHandler _soundHandler;
    [SerializeField] private Toggle _SFX;
    [SerializeField] private Toggle _music;
    
    public void Initialize() 
    {
        UpdateHighScore();

        if (_soundHandler.Mixer.GetFloat("SFX", out float SFXValue)) 
        {
            if (SFXValue == 0)
                _SFX.isOn = true;
            else
                _SFX.isOn = false;
        } 

        if (_soundHandler.Mixer.GetFloat("Music", out float musicValue)) 
        {
            if (musicValue == 0)
                _SFX.isOn = true;
            else
                _SFX.isOn = false;
        }

        gameObject.SetActive(true);
    }

    private void Start() => UpdateHighScore();

    private void UpdateHighScore()
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
