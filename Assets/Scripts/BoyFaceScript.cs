using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoyFaceScript : MonoBehaviour {

    [SerializeField]
    private int hp;

    [SerializeField]
    private GameObject crackedAcornPrefab;
    [SerializeField]
    private AnimationClip acornCrackedAnimation;

    [SerializeField]
    private AudioSource boyFaceSoundSource;
    [SerializeField]
    private AudioClip boyHurtAudio;
    [SerializeField]
    private AudioClip boyCryAudio;

    [SerializeField]
    private GameObject baby;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Acorn")
        {
            if (collision.collider.GetComponent<GiantAcornScript>().getGiant())
            {
                hp = hp - 2;
            } else
            {
                hp--;
            }
            // replace the acorn with crack acorn
            Vector3 pos = collision.collider.transform.position;
            Quaternion rot = collision.collider.transform.rotation;
            Vector3 scale = collision.collider.transform.localScale;
            GameObject crackedAcorn = Instantiate(crackedAcornPrefab, pos, rot);
            crackedAcorn.transform.localScale = scale;
            Destroy(crackedAcorn, acornCrackedAnimation.length);

            BoyHurt();

            if (hp <= 0)
            {
                baby.GetComponent<Baby>().SetWin(true);
            }
        }
    }

    private void BoyHurt()
    {
        if (!gameObject.activeSelf)
        {
            Debug.Log("no face");
        }
        gameObject.GetComponent<Animator>().SetTrigger("Hurt");
        // gameObject.GetComponent<Animator>().SetBool("Hunt", true);
        // gameObject.GetComponent<Animator>().SetBool("Angry", false);
        boyFaceSoundSource.PlayOneShot(boyHurtAudio);
        Debug.Log("Play boy hurt");
    }

    public void BoyAngry()
    {
        if (!gameObject.activeSelf)
        {
            Debug.Log("no face");
        }
        gameObject.GetComponent<Animator>().SetTrigger("Angry");
        // gameObject.GetComponent<Animator>().SetBool("Hunt", false);
        // gameObject.GetComponent<Animator>().SetBool("Angry", true);
        Debug.Log("Play boy angry");
    }

    public void BoyCry()
    {
        if (!gameObject.activeSelf)
        {
            Debug.Log("no face");
        }
        gameObject.GetComponent<Animator>().SetTrigger("Cry");
        boyFaceSoundSource.volume = 1.0f;
        boyFaceSoundSource.PlayOneShot(boyCryAudio);
        Debug.Log("Play boy cry");
    }
}
