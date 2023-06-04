using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private SoundHandler _soundHandler;
    [Space, SerializeField] private TextMeshProUGUI _scoreBar;
    [Space, SerializeField] private Toggle _slowmotionSkill;
    [SerializeField] private Slider _slowmotionValue;
    [Space, SerializeField] private TextMeshProUGUI _timer;

    public void UpdateScoreBar(int amount)
    {
        _scoreBar.text = amount.ToString();
    }

    public void UpdateTimerBar(float amount)
    {
        _timer.text = ((int)amount).ToString();
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

    public void UpdateSlowMotionBar(float value)
    {
        _slowmotionValue.value = _slowmotionValue.maxValue - value;
    }
}
