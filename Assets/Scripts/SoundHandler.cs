using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundHandler : MonoBehaviour
{
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private AudioSource _soundSource;

    [Space, SerializeField] private List<AudioClip> _clickClips;
    [SerializeField] private List<AudioClip> _UIClips;
    [SerializeField] private List<AudioClip> _tileCollidesTileClips;
    [SerializeField] private List<AudioClip> _tileCollidesWallClips;

    public AudioMixer Mixer => _mixer;

    private const float PITCH_VALUE = 0.2f;
    private const float VOLUME_VALUE = 0.15f;

    private void PlaySound(List<AudioClip> clips) 
    {
        _soundSource.clip = clips[Random.Range(0, clips.Count)];

        _soundSource.volume = Random.Range(1 - VOLUME_VALUE, 1 + PITCH_VALUE);
        _soundSource.pitch = Random.Range(1 - PITCH_VALUE, 1 + PITCH_VALUE);

        _soundSource.PlayOneShot(_soundSource.clip);
    }

    public void UpdateSound(Toggle sound)
    {
        PlayButtonSound();

        if (sound.isOn)
        {
            _mixer.SetFloat("SFX", 0);
            return;
        }

        _mixer.SetFloat("SFX", -80);
    }

    public void UpdateMusic(Toggle music)
    {
        PlayButtonSound();

        if (music.isOn)
        {
            _mixer.SetFloat("Music", 0);
            return;
        }

        _mixer.SetFloat("Music", -80);
    }

    public void PlayClickSound() => PlaySound(_clickClips);
    public void PlayButtonSound() => PlaySound(_UIClips);
    public void PlayTileCollidesTileSound() => PlaySound(_tileCollidesTileClips);
    public void PlayTileCollidesWallSound() => PlaySound(_tileCollidesWallClips);
}
