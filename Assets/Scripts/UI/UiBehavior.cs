using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UiBehavior : MonoBehaviour
{
    [SerializeField]
    private Image connectionLamp;

    [SerializeField]
    private Button applyButton;

    [SerializeField]
    private TMP_InputField ServerIpInput;

    [SerializeField]
    private TMP_InputField ServerPortInput;

    [SerializeField]
    private TMP_InputField VideoUrlInput;

    [SerializeField]
    private Slider volumeLevel;

    [SerializeField]
    private Toggle isSoundEnabled;

    [SerializeField]
    private Toggle isMusicEnabled;

    public event Action<float> onVolumeChanged;
    public event Action<bool> onSoundChanged;
    public event Action<bool> onMusicChanged;

    public void SetGreen()
    {
        connectionLamp.color = Color.green;
    }

    public void SetRed()
    {
        connectionLamp.color = Color.red;
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadData();

        volumeLevel.onValueChanged.AddListener((value) =>
        {
            onVolumeChanged?.Invoke(value);
        });

        isSoundEnabled.onValueChanged.AddListener((value) =>
        {
            onSoundChanged?.Invoke(value);
        });

        isMusicEnabled.onValueChanged.AddListener(value =>
        {
            onMusicChanged?.Invoke(value);
        });

        applyButton.onClick.AddListener(SaveData);
    }

    private void LoadData()
    {
        var savedSoundData = SavingSystem.LoadSoundData();
        volumeLevel.value = savedSoundData.VolumeLevel;
        isSoundEnabled.isOn = savedSoundData.IsSoundEnabled;
        isMusicEnabled.isOn = savedSoundData.IsMusicEnabled;

        var savedServerData = SavingSystem.LoadServerData();

        ServerIpInput.text = savedServerData.ServerIp;
        ServerPortInput.text = savedServerData.ServerPort;

        var savedVideoData = SavingSystem.LoadVideoData();
        VideoUrlInput.text = savedVideoData.VideoUrl;
    }

    private void SaveData()
    {
        var sound = new SavingSystem.SavedDataSound()
        {
            VolumeLevel = volumeLevel.value,
            IsMusicEnabled = isMusicEnabled.isOn,
            IsSoundEnabled = isSoundEnabled.isOn

        };
        var server = new SavingSystem.SavedServerData()
        {
            ServerIp = ServerIpInput.text,
            ServerPort = ServerPortInput.text,
        };

        var video = new SavingSystem.SavedVideoData()
        {
            VideoUrl = VideoUrlInput.text
        };

        SavingSystem.Save(sound, server, video);
    }
}
