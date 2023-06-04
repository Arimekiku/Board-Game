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
            UIAudio.mute = false;
            ClickAudio.mute = false;
            BoardAudio.mute = false;
            return;
        }

        UIAudio.mute = true;
        ClickAudio.mute = true;
        BoardAudio.mute = true;
    }

    public void UpdateMusic(Toggle music)
    {
        PlayButtonSound();

        _musicMM.isOn = music.isOn;
        _musicG.isOn = music.isOn;

        if (music.isOn)
        {
            MusicAudio.mute = false;
            return;
        }

        MusicAudio.mute = true;
    }

    public AudioSource ClickAudio => _clickAudio;
    public AudioSource UIAudio => _UIAudio;
    public AudioSource MusicAudio => _musicAudio;
    public AudioSource BoardAudio => _boardAudio;
}
