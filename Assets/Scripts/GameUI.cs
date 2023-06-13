using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private SoundHandler _soundHandler;
    [Space, SerializeField] private TextMeshProUGUI _scoreBar;
    [Space, SerializeField] private Toggle _slowmotionSkill;
    [SerializeField] private Slider _slowmotionValue;
    [SerializeField] private Toggle _SFX;
    [SerializeField] private Toggle _music;

    public void Initialize() 
    {
        UpdateScoreBar(0);
        UpdateSlowMotionBar(0);

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
    }
    
    public void SelectSlowMotionSkill()
    {
        _slowmotionSkill.isOn = false;
        _soundHandler.PlayButtonSound();
    }

    public void DeselectSlowMotionSkill()
    {
        _slowmotionSkill.isOn = true;
        _soundHandler.PlayButtonSound(); 
    }

    public void UpdateScoreBar(int amount) => _scoreBar.text = amount.ToString();
    public void UpdateSlowMotionBar(float value) => _slowmotionValue.value = _slowmotionValue.maxValue - value;
}
