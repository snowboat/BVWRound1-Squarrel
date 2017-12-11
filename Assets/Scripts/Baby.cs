using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Baby : MonoBehaviour
{
    private static float distanceX = 3.0f;
    private static float distanceZ = 3.7f;

    private Vector3 pos1 = new Vector3(distanceX, 0f, 0f);
    private Vector3 pos2 = new Vector3(0f, 0f, distanceZ);
    private Vector3 pos3 = new Vector3(-distanceX, 0f, 0f);
    private Vector3 pos4 = new Vector3(0, 0f, -distanceZ);

    private int currentPosition;
    private Vector3 nextPosition;

    [SerializeField]
    private GameObject[] hand;
    private GameObject handAttack;

    [SerializeField]
    private GameObject boyFace;
    [SerializeField]
    private GameObject wall;

    [SerializeField]
    private GameObject acornGenerator;

    [SerializeField]
    private GameObject soundSource;
    private BoyMove soundScript;
    [SerializeField]
    private AudioClip boyVoiceBeforeBossFight;

    [SerializeField]
    private AudioSource babySound;
    [SerializeField]
    private AudioClip babyNoAudio;

    [SerializeField]
    private GameObject treeHouse;

    //[SerializeField]
    //private GameObject winText;

    [SerializeField]
    private GameObject cameraControl;

    [SerializeField]
    private AnimationClip treeHouseExplosionAnimation;

    // delay babymove;
    private bool isBabyMove = false;

    private bool isGrab = false;
    private bool isEscape = false;

    private bool isBoyFaceShow = false;
    private bool bossFightStart = false;

    private bool isWin = false;
    private bool isEnd = false;

    // baby voice over
    private int voiceNum = 0;
    [SerializeField]
    private AudioClip[] babyHelp;

    // baby animation
    private Animator babyAnimator;

    // baby position list
    [SerializeField]
    private List<Transform> pos1to2;
    [SerializeField]
    private List<Transform> pos1to3;
    [SerializeField]
    private List<Transform> pos1to4;
    [SerializeField]
    private List<Transform> pos2to1;
    [SerializeField]
    private List<Transform> pos2to3;
    [SerializeField]
    private List<Transform> pos2to4;
    [SerializeField]
    private List<Transform> pos3to1;
    [SerializeField]
    private List<Transform> pos3to2;
    [SerializeField]
    private List<Transform> pos3to4;
    [SerializeField]
    private List<Transform> pos4to1;
    [SerializeField]
    private List<Transform> pos4to2;
    [SerializeField]
    private List<Transform> pos4to3;

    private int currentState;
    private List<Transform> nextRoute;

    private Vector3 beforeTarget;

    [SerializeField]
    private AnimationClip handLetGoAnimation;
    [SerializeField]
    private AnimationClip boyCryAnimation;
    [SerializeField]
    private GameObject evilFace;
    [SerializeField]
    private GameObject evilHand;


    // ending cut scene
    [SerializeField]
    private GameObject endingVideoPlayer;
    [SerializeField]
    private GameObject rendererCube;
    [SerializeField]
    private GameObject playerBackground;
    [SerializeField]
    private GameObject environment;
    [SerializeField]
    private GameObject boy;
    [SerializeField]
    private GameObject leftHand;
    [SerializeField]
    private GameObject rightHand;


    private void Start()
    {
        babyAnimator = GetComponent<Animator>();
        babySound = GetComponent<AudioSource>();

        // assign y value
        float tempY = transform.position.y;
        pos1.y = tempY;
        pos2.y = tempY;
        pos3.y = tempY;
        pos4.y = tempY;

        // get sound source script
        soundScript = soundSource.GetComponent<BoyMove>();

        // initial position of baby
        currentPosition = 1;
        transform.position = pos1;
        beforeTarget = transform.position;
        StartCoroutine(DelayUpdateNextAttack());
        AttackRecord.handAttackCount++;
    }

    IEnumerator DelayUpdateNextAttack()
    {
        yield return new WaitForSeconds(2.0f);        
        UpdateNextPosition();
        UpdateAttackingHand();
        soundScript.SetIsMovedDelayed();
        SetIsBabyMove(true);
        BabyHelp();
        GetComponent<Rigidbody>().isKinematic = false;
    }

    private void MirrorHand(int handNum)
    {
        GameObject mirrorHand = hand[handNum - 1];
        Vector3 scaleToChange = mirrorHand.transform.localScale;
        scaleToChange.x = -mirrorHand.transform.localScale.x;
        mirrorHand.transform.localScale = scaleToChange;
    }

    // voice over
    void BabyHelp()
    {
        babySound.clip = babyHelp[voiceNum];
        babySound.Play();
        voiceNum++;

        if (voiceNum == babyHelp.Length)
        {
            voiceNum = 0;
        }
    }
    IEnumerator PlayBoyVoice()
    {
        yield return new WaitForSeconds(1.0f);
        soundScript.PlayBoyVoiceBeforeBossFight();
        yield return new WaitForSeconds(boyVoiceBeforeBossFight.length);
        StartCoroutine(ShowBoyFace());
    }

    IEnumerator ShowBoyFace()
    {
        // TODO: play the voice of the boy
        evilFace.SetActive(true);
        evilHand.SetActive(true);
        treeHouse.GetComponent<Animator>().SetTrigger("BossFight");
        cameraControl.GetComponent<CameraControl>().PunchShaking();
        treeHouse.GetComponent<TreeHouseScript>().PlayExplosionSound();

        acornGenerator.GetComponent<AcornGeneratorScript>().GenerateNormalAcorn(10);
        yield return new WaitForSeconds(treeHouseExplosionAnimation.length / 2.0f);
        acornGenerator.GetComponent<AcornGeneratorScript>().GenerateNormalAcorn(10);
        yield return new WaitForSeconds(treeHouseExplosionAnimation.length / 2.0f);
        acornGenerator.GetComponent<AcornGeneratorScript>().GenerateGiantAcorn(1);

        boyFace.SetActive(true);
        wall.SetActive(false);
        // mirror the hand
        MirrorHand(5);
        MirrorHand(8);
       
        bossFightStart = true;
        treeHouse.GetComponent<TreeHouseScript>().PlayBossFightBGM();
    }

    private void UpdateNextBossFight(int handNum)
    {
        AttackRecord.handAttacking = handNum;
        handAttack = hand[AttackRecord.handAttacking - 1];
        var meshes = handAttack.GetComponentsInChildren<Renderer>();
        foreach (Renderer mesh in meshes)
        {
            mesh.enabled = true;
        }
        // Debug.Log("change attacking hand " + AttackRecord.handAttacking);
        handAttack.GetComponent<Attack>().SetHp(1);
        AttackRecord.isAttacking = true;
    }

    IEnumerator BoyReactionWhenWin()
    {
        boyFace.GetComponent<BoyFaceScript>().BoyCry();
        yield return new WaitForSeconds(boyCryAnimation.length * 2);
        Debug.Log("length is " + boyCryAnimation.length * 2);
        boyFace.SetActive(false);
        treeHouse.GetComponent<Animator>().SetTrigger("BoyRunAway");
        // winText.SetActive(true);
        // call the next scene
        StartCoroutine(EndScene());
    }

    private void Update()
    {
        if (isWin && !isEnd)
        {
            StartCoroutine(BoyReactionWhenWin());
            AttackRecord.isAttacking = false;
            handAttack = null;
            isEnd = true;
        }

        if (handAttack)
        {
            Attack handAttackScript = handAttack.GetComponent<Attack>();
            if (handAttackScript)
            {
                int hp = handAttackScript.GetHp();
                if ((hp <= 0) && (nextRoute == null) && (!isGrab) && (!isEscape))
                {
                    // Debug.Log("hp from baby is " + hp);
                    if (AttackRecord.handAttackCount < AttackRecord.totalAttackBeforeBossFight)
                    {
                        handAttack = null;
                        StartCoroutine(DelayUpdateNextAttack());
                        AttackRecord.handAttackCount++;
                        Debug.Log("Set next normal attack " + AttackRecord.handAttackCount);
                    }
                    else
                    {
                        // Start Boss fight
                        GetComponent<Rigidbody>().isKinematic = false;
                        if (currentPosition != 4)
                        {
                            // move baby in front of the player
                            if (currentPosition == 1)
                            {
                                nextRoute = pos1to4;
                            }
                            else if (currentPosition == 2)
                            {
                                nextRoute = pos2to4;
                            }
                            else if (currentPosition == 3)
                            {
                                nextRoute = pos3to4;
                            }
                            currentPosition = 4;
                            currentState = 1;
                            SetIsBabyMove(true);
                        }
                        if (!isBoyFaceShow)
                        {
                            StartCoroutine(PlayBoyVoice());
                            isBoyFaceShow = true;
                        }
                        else if(bossFightStart)
                        {
                            handAttack = null;
                            if (AttackRecord.handAttacking != 5)
                            {
                                UpdateNextBossFight(5);
                            } else
                            {
                                UpdateNextBossFight(8);
                            }
                            AttackRecord.handAttackCount++;
                            Debug.Log("Set next boss attack " + AttackRecord.handAttackCount);
                        }
                    }
                }
            }
        }
    }

    IEnumerator MoveBabyBackToIdleZone()
    {

        Vector3 idleZone = pos1;
        switch(currentPosition)
        {
            case 2:
                idleZone = pos2;
                break;
            case 3:
                idleZone = pos3;
                break;
            case 4:
                idleZone = pos4;
                break;
            default:
                break;
        }

        float startTime = Time.time;
        float duration = 1.0f;
        float stepTime = Time.deltaTime;
        Vector3 originalPos = transform.position;
        // Debug.Log("original pos is " + originalPos);
        // Debug.Log("idle zone is " + idleZone);
        GameObject target = new GameObject();
        target.transform.position = idleZone;
        GameObject currentXZPos = new GameObject();
        Vector3 currentXZPosition = transform.position;
        currentXZPosition.y = idleZone.y;
        currentXZPos.transform.position = currentXZPosition;
        currentXZPos.transform.LookAt(target.transform);
        currentXZPos.transform.Rotate(0, 90, 0);
        transform.rotation = currentXZPos.transform.rotation;

        while (Time.time - startTime <= duration)
        {
            Vector3 newPos = Vector3.Lerp(originalPos, idleZone, (Time.time - startTime) / duration);
            transform.position = newPos;
            yield return new WaitForSeconds(stepTime);
        }
        isEscape = false;
        babyAnimator.SetBool("IsEscape", false);
        babyAnimator.SetBool("IsScared", true);
    }

    IEnumerator DelayedBackToIdleZone()
    {
        yield return new WaitForSeconds(handLetGoAnimation.length/4);
        StartCoroutine(MoveBabyBackToIdleZone());
    }

    public void SetBabyToIdleZone()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        StartCoroutine(DelayedBackToIdleZone());
    }

    private void FixedUpdate()
    {
        if (isBabyMove)
        {
            BabyMove();
        }

        if (nextRoute != null)
        {
            if (transform.position == nextRoute[3].position)
            {
                SetIsBabyMove(false);
                babyAnimator.SetBool("IsRunning", false);
                babyAnimator.SetBool("IsScared", true);
                nextRoute = null;
            }
        }

    }

    private void BabyMove()
    {
        // booleans
        babyAnimator.SetBool("IsRunning", true);
        babyAnimator.SetBool("IsScared", false);

        // speed
        float speed = 1f * Time.deltaTime;

        // move
        if (nextRoute!= null)
        {
            if ((transform.position == nextRoute[currentState].position) && (currentState != 3))
            {
                currentState++;
            }
            transform.position = Vector3.MoveTowards(transform.position, nextRoute[currentState].position, speed);

            Quaternion previousRot = transform.rotation;
            transform.LookAt(beforeTarget);
            transform.Rotate(0, -90, 0);

            Quaternion rot = transform.rotation;
            transform.rotation = Quaternion.Lerp(previousRot, rot, speed);

            // reset beforeTarget position
            beforeTarget = transform.position;
        }
    }

    void UpdateAttackingHand()
    {
        handAttack = hand[AttackRecord.handAttacking - 1];
        Debug.Log("change attacking hand " + AttackRecord.handAttacking);
        var meshes = handAttack.GetComponentsInChildren<Renderer>();
        foreach (Renderer mesh in meshes)
        {
            mesh.enabled = true;
        }
        handAttack.GetComponent<Attack>().SetHp(2);
    }

    void UpdateNextPosition()
    {
        // assign next position of baby
        int ranNum = Random.Range(1, 5);
        while (ranNum == currentPosition)
        {
            ranNum = Random.Range(1, 5);
        }

        switch (ranNum)
        {
            case 1:
                if (currentPosition == 2)
                {
                    nextRoute = pos2to1;
                }
                else if (currentPosition == 3)
                {
                    nextRoute = pos3to1;
                }
                else if (currentPosition == 4)
                {
                    nextRoute = pos4to1;
                }
                currentPosition = 1;
                break;

            case 2:
                if (currentPosition == 1)
                {
                    nextRoute = pos1to2;
                }
                else if (currentPosition == 3)
                {
                    nextRoute = pos3to2;
                }
                else if (currentPosition == 4)
                {
                    nextRoute = pos4to2;
                }
                currentPosition = 2;
                break;

            case 3:
                if (currentPosition == 1)
                {
                    nextRoute = pos1to3;
                }
                else if (currentPosition == 2)
                {
                    nextRoute = pos2to3;
                }
                else if (currentPosition == 4)
                {
                    nextRoute = pos4to3;
                }
                currentPosition = 3;
                break;
              

            case 4:
                if (currentPosition == 1)
                {
                    nextRoute = pos1to4;
                }
                else if (currentPosition == 2)
                {
                    nextRoute = pos2to4;
                }
                else if (currentPosition == 3)
                {
                    nextRoute = pos3to4;
                }
                currentPosition = 4;
                break;
        }
        currentState = 1;

        float p = Random.Range(0.0f, 1.0f);
        switch (ranNum)
        {
            case 1:
                if (p < 0.5f)
                {
                    soundScript.SetNextPosition(1);
                    AttackRecord.handAttacking = 2;
                }
                else
                {
                    soundScript.SetNextPosition(4);
                    AttackRecord.handAttacking = 7;
                }
                break;
            case 2:
                if (p < 0.5f)
                {
                    soundScript.SetNextPosition(1);
                    AttackRecord.handAttacking = 1;
                }
                else
                {
                    soundScript.SetNextPosition(2);
                    AttackRecord.handAttacking = 4;
                }
                break;
            case 3:
                if (p < 0.5f)
                {
                    soundScript.SetNextPosition(2);
                    AttackRecord.handAttacking = 3;
                }
                else
                {
                    soundScript.SetNextPosition(3);
                    AttackRecord.handAttacking = 6;
                }
                break;
            case 4:
                if (p < 0.5f)
                {
                    soundScript.SetNextPosition(3);
                    AttackRecord.handAttacking = 5;
                }
                else
                {
                    soundScript.SetNextPosition(4);
                    AttackRecord.handAttacking = 8;
                }
                break;
        }
    }

    

    public void SetIsBabyMove(bool move)
    {
        isBabyMove = move;
        // Debug.Log("Set baby move to " + move);
    }

    public void SetIsBabyGrab(bool grab)
    {
        isGrab = grab;
        if (isGrab)
        {
            BabyHelp();
            babyAnimator.SetBool("IsCaught", true);
            babyAnimator.SetBool("IsScared", false);
        }
    }

    public bool GetIsBabyGrab()
    {
        return isGrab;
    }

    public void SetIsBabyEscape(bool escape)
    {
        isEscape = escape;
        if (escape)
        {
            babyAnimator.SetBool("IsEscape", true);
            babyAnimator.SetBool("IsCaught", false);
        }
    }

    public void SetWin(bool win)
    {
        isWin = win;
    }

    public void PlayBabyNoAudio()
    {
        babySound.PlayOneShot(babyNoAudio);
    }

    IEnumerator EndScene()
    {
        yield return new WaitForSeconds(5.0f);
        //SceneManager.LoadScene("Ending");
        playerBackground.SetActive(true);
        rendererCube.SetActive(true);
        endingVideoPlayer.SetActive(true);
        environment.SetActive(false);
        boy.SetActive(false);
        leftHand.SetActive(false);
        rightHand.SetActive(false);
        Destroy(gameObject, 0.1f);
    }
   
}