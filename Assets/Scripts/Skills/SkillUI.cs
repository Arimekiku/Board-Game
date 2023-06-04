using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    [SerializeField] Toggle _skillToggle;
    [SerializeField] TextMeshProUGUI _skillAmount;
    [SerializeField] SoundHandler _soundHandler;

    public void UpdateAmount(int amount)
    {
        _skillAmount.text = "x" + amount;
    }

    public void SelectSkill()
    {
        _skillToggle.isOn = false;
        _soundHandler.PlayButtonSound();
    }

    public void DeselectSkill()
    {
        _skillToggle.isOn = true;
        _soundHandler.PlayButtonSound();
    }
}
