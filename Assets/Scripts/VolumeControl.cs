using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeControl : MonoBehaviour {

    [SerializeField]
    AudioSource audioSource;

	void Start ()
    {
        StartCoroutine(VolumeDown());
    }

	void Update ()
    {
		
	}

    IEnumerator VolumeDown()
    {
        yield return new WaitForSeconds(10f);
        audioSource.volume = Mathf.Lerp(audioSource.volume, 0.3f, Time.deltaTime * 2f);
    }
}
