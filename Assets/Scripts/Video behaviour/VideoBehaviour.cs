using RenderHeads.Media.AVProVideo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoBehaviour : MonoBehaviour
{
    [SerializeField]
    private MediaPlayer player;

    // Start is called before the first frame update
    void Start()
    {
        var videoData = SavingSystem.LoadVideoData();

        player.OpenMedia(new MediaPath(videoData.VideoUrl, MediaPathType.AbsolutePathOrURL));
        player.Pause();
    }
}
