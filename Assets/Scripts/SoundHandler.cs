using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SoundHandler : MonoBehaviour
{
    [SerializeField] private AudioSource _clickAudio;
    [SerializeField] private AudioSource _UIAudio;
    [SerializeField] private AudioSource _musicAudio;
    [SerializeField] private AudioSource _boardAudio;

    [Space, SerializeField] private List<AudioClip> _clickSounds;
    [SerializeField] private List<AudioClip> _UISounds;
    [SerializeField] private List<AudioClip> _tileCollidesTile;
    [SerializeField] private List<AudioClip> _tileCollidesWall;

    [Space, SerializeField] private Toggle _musicMM;
    [SerializeField] private Toggle _soundMM;
    [SerializeField] private Toggle _musicG;
    [SerializeField] private Toggle _soundG;

    public void PlayClickSound()
    {
        _clickAudio.clip = _clickSounds[Random.Range(0, _clickSounds.Count)];
        _clickAudio.Play();
    }

    public void PlayButtonSound()
    {
        _UIAudio.clip = _UISounds[Random.Range(0, _UISounds.Count)];
        _UIAudio.Play();
    }

    public void PlayTileCollidesTileSound()
    {
        _boardAudio.clip = _tileCollidesTile[Random.Range(0, _tileCollidesTile.Count)];
        _boardAudio.Play();
    }

    public void PlayTileCollidesWallSound()
    {
        _boardAudio.clip = _tileCollidesWall[Random.Range(0, _tileCollidesWall.Count)];
        _boardAudio.Play();
    }

    public void UpdateSound(Toggle sound)
    {
        PlayButtonSound();

        _soundMM.isOn = sound.isOn;
        _soundG.isOn = sound.isOn;

        if (sound.isOn)
        {
            _UIAudio.mute = false;
            _clickAudio.mute = false;
            _boardAudio.mute = false;
            return;
        }

        _UIAudio.mute = true;
        _clickAudio.mute = true;
        _boardAudio.mute = true;
    }

    public void UpdateMusic(Toggle music)
    {
        PlayButtonSound();

        _musicMM.isOn = music.isOn;
        _musicG.isOn = music.isOn;

        if (music.isOn)
        {
            _musicAudio.mute = false;
            return;
        }

        _musicAudio.mute = true;
    }
}
