using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoyMove : MonoBehaviour
{
    private static float distance = 5.66f;

    private Vector3 pos1 = new Vector3(distance, 0f, distance);
    private Vector3 pos2 = new Vector3(-distance, 0f, distance);
    private Vector3 pos3 = new Vector3(-distance, 0f, -distance);
    private Vector3 pos4 = new Vector3(distance, 0f, -distance);

    private Vector3 nextPosition;

    [SerializeField]
    private GameObject baby;

    [SerializeField]
    private GameObject center;

    [SerializeField]
    private GameObject acornGenerator;

    private Vector3 boyDirection;
    //private static AudioSource audioSource;

    public AudioClip boyFootStep;

    [SerializeField]
    private AudioSource source;
    [SerializeField]
    private AudioClip voiceBeforeBossFight;

    // boolean for boy's movement
    private bool isMove = false;
 
    // boolean for the sound
    private bool isPlay = false;

    IEnumerator GenerateNormalAcorn()
    {
        int num = Random.Range(1, 3);
        acornGenerator.GetComponent<AcornGeneratorScript>().GenerateNormalAcorn(num);
        yield return new WaitForSeconds(boyFootStep.length);
        StartCoroutine(GenerateNormalAcorn());
    }

    IEnumerator GenerateGiantAcorn()
    {
        acornGenerator.GetComponent<AcornGeneratorScript>().GenerateGiantAcorn(1);
        yield return new WaitForSeconds(boyFootStep.length);
        StartCoroutine(GenerateGiantAcorn());
    }

    private void Update()
    {
        if ((isMove) && (transform.position.x > (nextPosition.x - 0.5)) && (transform.position.x < (nextPosition.x + 0.5)) && (transform.position.z > (nextPosition.z - 0.5)) && (transform.position.z < (nextPosition.z + 0.5)))
        {
            isMove = false;
            isPlay = false;
            source.Stop();
            StopAllCoroutines();
            AttackRecord.isAttacking = true;
            Debug.Log("Start attacking");
        }
        if (isMove)
        {
            MoveAudio();
        }
        
        if (isMove && !isPlay)
        {
            FootStepSound();
            StartCoroutine(GenerateNormalAcorn());
        }
    }

    private void FootStepSound()
    {
        source.Play();
        isPlay = true;
    }

    private void MoveAudio()
    {
        boyDirection = baby.transform.position;
       
        float speed = 30f;
         
        if (isMove)
        {
            if (boyDirection.z < 0)
            {
                transform.RotateAround(center.transform.position, Vector3.up, speed * Time.deltaTime);
            }
            else
            {
                transform.RotateAround(center.transform.position, Vector3.down, speed * Time.deltaTime);
            }
        }
    }

    IEnumerator DelayedMove()
    {
        yield return new WaitForSeconds(4.0f);
        isMove = true;
    }

    public void SetIsMovedDelayed()
    {
        StartCoroutine(DelayedMove());
    }

    public void SetNextPosition(int np)
    {
        switch (np)
        {
            case 1:
                nextPosition = pos1;
                break;
            case 2:
                nextPosition = pos2;
                break;
            case 3:
                nextPosition = pos3;
                break;
            case 4:
                nextPosition = pos4;
                break;
            default:
                break;
        }
    }

    public void PlayBoyVoiceBeforeBossFight()
    {
        source.PlayOneShot(voiceBeforeBossFight);
    }
}
