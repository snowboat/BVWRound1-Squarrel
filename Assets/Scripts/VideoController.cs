using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour {

    [SerializeField]
    VideoClip video;

    VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.playOnAwake = false;

        videoPlayer.clip = video;
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(0.1f);

        if (!videoPlayer.isPlaying)
        {
            videoPlayer.Play();
        }
    }
}
