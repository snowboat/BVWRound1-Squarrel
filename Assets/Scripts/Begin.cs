using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Begin : MonoBehaviour {

    [SerializeField]
    VideoPlayer videoPlayer;

    float wait;

	void Start ()
    {
        wait = (float) videoPlayer.clip.length;
        StartCoroutine(NextScene());
	}
	
    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(wait + 2f);
        SceneManager.LoadScene("TreeHouse");
    }
}
