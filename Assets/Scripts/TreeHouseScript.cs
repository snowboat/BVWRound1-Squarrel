using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeHouseScript : MonoBehaviour {

    [SerializeField]
    private AudioSource treeHouseSoundSource;
    [SerializeField]
    private AudioClip explosionClip;
    [SerializeField]
    private AudioClip normalBGM;
    [SerializeField]
    private AudioClip bossFightBgm;
    [SerializeField]
    private AudioClip boyLaughter;


    private void Awake()
    {
        treeHouseSoundSource.clip = normalBGM;
        treeHouseSoundSource.Play();
    }

    public void PlayExplosionSound()
    {
        treeHouseSoundSource.Stop();
        treeHouseSoundSource.PlayOneShot(explosionClip);
    }

    public void PlayBossFightBGM()
    {
        treeHouseSoundSource.Stop();
        treeHouseSoundSource.volume = 0.5f;
        treeHouseSoundSource.clip = bossFightBgm;
        treeHouseSoundSource.Play();
    }

    public void PlayBoyLaughter()
    {
        treeHouseSoundSource.Stop();
        treeHouseSoundSource.clip = boyLaughter;
        treeHouseSoundSource.Play();
    }

}
