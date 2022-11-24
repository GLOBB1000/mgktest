using RenderHeads.Media.AVProVideo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField]
    private List<AudioSource> soundSources;

    [SerializeField]
    private AudioSource musicSource;

    [SerializeField]
    private UiBehavior uiBehavior;

    [SerializeField]
    private MediaPlayer mediaPlayer;

    private void Awake()
    {
        uiBehavior.onVolumeChanged += UiBehavior_onVolumeChanged;
        uiBehavior.onMusicChanged += UiBehavior_onMusicChanged;
        uiBehavior.onSoundChanged += UiBehavior_onSoundChanged;
    }

    private void Start()
    {
        var savingData = SavingSystem.LoadSoundData();

        musicSource.enabled = savingData.IsMusicEnabled;
        musicSource.volume = savingData.VolumeLevel;

        foreach (var source in soundSources)
        {
            source.enabled = savingData.IsSoundEnabled;
            source.volume = savingData.VolumeLevel;
        }
    }

    public void SetMusicEnabled()
    {
        if (mediaPlayer.Info.HasAudio())
            musicSource.volume = 0;
    }

    private void UiBehavior_onSoundChanged(bool obj)
    {
        foreach (var item in soundSources)
            item.enabled = obj;
    }

    private void UiBehavior_onMusicChanged(bool obj)
    {
        musicSource.enabled = obj;
    }

    private void UiBehavior_onVolumeChanged(float obj)
    {
        musicSource.volume = obj;

        foreach(AudioSource source in soundSources)
            source.volume = obj;
    }
}
