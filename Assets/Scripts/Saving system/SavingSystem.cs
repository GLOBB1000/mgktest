using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SavingSystem 
{
    public struct SavedDataSound
    {
        public float VolumeLevel;
        public bool IsMusicEnabled;
        public bool IsSoundEnabled;
    }
    public struct SavedServerData
    {
        public string ServerIp;
        public string ServerPort;
    }

    public struct SavedVideoData
    {
        public string VideoUrl;
    }

    public static void Save(SavedDataSound sound, SavedServerData serverData, SavedVideoData videoData)
    {
        var soundJson = JsonConvert.SerializeObject(sound);
        PlayerPrefs.SetString("SoundData", soundJson);

        var serverJson = JsonConvert.SerializeObject(serverData);

        var path = Path.Combine(Application.streamingAssetsPath, "ServerData.txt");

        File.WriteAllText(path, serverJson);

        var videoJson = JsonConvert.SerializeObject(videoData);
        PlayerPrefs.SetString("VideoData", videoJson);
    }

    public static SavedDataSound LoadSoundData()
    {
        var data = PlayerPrefs.GetString("SoundData");

        if (!string.IsNullOrEmpty(data))
            return JsonConvert.DeserializeObject<SavedDataSound>(data);

        return new SavedDataSound() 
        {
            VolumeLevel = 0.5f,
            IsMusicEnabled = true,
            IsSoundEnabled = true,
        };

    }

    public static SavedVideoData LoadVideoData()
    {
        var data = PlayerPrefs.GetString("VideoData");

        if (!string.IsNullOrEmpty(data))
            return JsonConvert.DeserializeObject<SavedVideoData>(data);

        return new SavedVideoData()
        {
            VideoUrl = "rtsp://wowzaec2demo.streamlock.net/vod/mp4:BigBuckBunny_115k.mp4"
        };
    }

    public static SavedServerData LoadServerData()
    {
        var path = Path.Combine(Application.streamingAssetsPath, "ServerData.txt");

        if (File.Exists(path))
        {
            var loadedServerData = File.ReadAllText(path);

            if (!string.IsNullOrEmpty(loadedServerData))
                return JsonConvert.DeserializeObject<SavedServerData>(loadedServerData);
        }

        return new SavedServerData()
        {
            ServerIp = "0.0.0.0",
            ServerPort = "8888"
        };
    }
}
