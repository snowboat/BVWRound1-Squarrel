using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Attack : MonoBehaviour {

    [SerializeField]
    private int handNumber;
    [SerializeField]
    private GameObject baby;
    [SerializeField]
    private GameObject crackedAcornPrefab;
    [SerializeField]
    private AnimationClip acornCrackedAnimation;

    private Vector3 originalPosition;
    private Vector3 babyPosition;
    private Vector3 nextDestination;
    private Quaternion originalQuaternion;
    private Quaternion windowQuaternion;
    private Quaternion babyQuaternion;

    private Animator handAnimator;
    [SerializeField]
    private AnimationClip grabAnimation;
    [SerializeField]
    private AnimationClip grabLetGoAnimation;

    private bool isHandStop = false;
    private bool isOutside = true;

    private FixedJoint fx;

    [SerializeField]
    private Transform windowPos;
    [SerializeField]
    private Transform babyPos;

    // voice over
    [SerializeField]
    private AudioClip[] boyOuch;

    [SerializeField]
    private AudioSource boySound;
    [SerializeField]
    private GameObject boyFace;

    private static int boyOuchNum = 0;

    [SerializeField]
    private GameObject treeHouse;

    private int hp;
    
    public int GetHp()
    {
        return hp;
    }

    public void SetHp(int h)
    {
        hp = h;
    }

    public void ReduceHp()
    {
        hp--;
    }

    private void Awake()
    {
        originalPosition = transform.position;
        babyPosition = babyPos.position;
        babyPosition.y = 0.2f;

        // get the initial rotation
        transform.LookAt(windowPos);
        transform.Rotate(0, 180, 0);
        originalQuaternion = transform.rotation;

        // get the rotation when going through the window
        var rotationGetter = new GameObject();
        Vector3 rotationGetterPosition = originalPosition;
        rotationGetterPosition[1] = windowPos.position.y;
        rotationGetter.transform.position = rotationGetterPosition;
        rotationGetter.transform.LookAt(windowPos);
        rotationGetter.transform.Rotate(0, 180, 0);
        windowQuaternion = rotationGetter.transform.rotation;

        // get the rotation when grabbing the baby
        var windowPosition = new GameObject();
        windowPosition.transform.position = windowPos.position;
        windowPosition.transform.LookAt(babyPos);
        windowPosition.transform.Rotate(20, 180, 0);
        babyQuaternion = windowPosition.transform.rotation;

        handAnimator = GetComponent<Animator>();
    }

    IEnumerator WaitGrabAnimation()
    {
        yield return new WaitForSeconds(grabAnimation.length * 2.0f);
        isHandStop = false;
    }

    IEnumerator WaitGrabLetGoAnimation()
    {
        yield return new WaitForSeconds(grabLetGoAnimation.length * 2.0f);
        isHandStop = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Baby")
        {
            if (fx == null)
            {
                fx = gameObject.AddComponent<FixedJoint>() as FixedJoint;
                // fx.anchor = new Vector3(0f, 0f, 0f);
                fx.breakForce = 20000;
                fx.breakTorque = 20000;
                fx.connectedBody = baby.GetComponent<Rigidbody>();
                AttackRecord.isAttacking = false;
                baby.GetComponent<Baby>().SetIsBabyGrab(true);
                handAnimator.SetBool("Grab", true);
                handAnimator.SetBool("PrepareGrab", false);
                Debug.Log("Grab!");
                isHandStop = true;
                StartCoroutine(WaitGrabAnimation());
            }
        }

        if (other.tag == "Acorn")
        {
            if (AttackRecord.handAttacking == handNumber && (!(isOutside && baby.GetComponent<Baby>().GetIsBabyGrab()))
                )
            {
                ReduceHp();
                if (hp <= 0)
                {
                    PlayBoyOuch();
                    if (baby.GetComponent<Baby>().GetIsBabyGrab())
                    {
                        baby.GetComponent<Baby>().SetIsBabyEscape(true);
                    }
                    isHandStop = false;
                    AttackRecord.isAttacking = false;
                    // change the animation
                    handAnimator.SetBool("PrepareGrab", false);
                    handAnimator.SetBool("StartWave", false);
                    handAnimator.SetBool("Grab", false);
                    handAnimator.SetBool("LetGo", true);
                    // stop the hand when let go
                    //if (baby.GetComponent<Baby>().GetIsBabyGrab())
                    //{
                    //    isHandStop = true;
                    //    StartCoroutine(WaitGrabLetGoAnimation());
                    //}

                    if (boyFace.activeInHierarchy)
                    {
                        boyFace.GetComponent<BoyFaceScript>().BoyAngry();
                    }
                }
                
                // replace the acorn with crack acorn
                Vector3 pos = other.transform.position;
                Quaternion rot = other.transform.rotation;
                Vector3 scale = other.transform.localScale;
                GameObject crackedAcorn = Instantiate(crackedAcornPrefab, pos, rot);
                crackedAcorn.transform.localScale = scale;
                Destroy(crackedAcorn, acornCrackedAnimation.length);
            }
            
        }
    }

    // when hp drops to 0, release joint
    private void ReleaseBaby()
    {
        if (fx!= null)
        {
            var joints = GetComponents<FixedJoint>();
            foreach (FixedJoint joint in joints)
            {
                joint.connectedBody = null;
                Destroy(joint);
            }
            fx = null;
        }
        baby.GetComponent<Baby>().SetBabyToIdleZone();
        baby.GetComponent<Baby>().SetIsBabyGrab(false);
        
    }

    private void FixedUpdate()
    {
        if (!isHandStop)
        {
            if ((handNumber == AttackRecord.handAttacking) && (AttackRecord.isAttacking))
            {
                // Debug.Log("Hp is " + hp);
                HandMove();
                handAnimator.SetBool("StartWave", true);
                handAnimator.SetBool("BackToDefault", false);
            }
            if (!AttackRecord.isAttacking)
            {
                HandBack();
            } else if (handNumber != AttackRecord.handAttacking)
            {
                HandBack();
            }
            if ((handNumber == AttackRecord.handAttacking) && (hp <= 0) && (baby.GetComponent<Baby>().GetIsBabyGrab()))
            {
                
                Debug.Log("release the baby");
                ReleaseBaby();
            }
            
        }
    }

    private void PlayBoyOuch()
    {
        boySound.Stop();
        boySound.clip = boyOuch[boyOuchNum];
        boySound.Play();
        boyOuchNum++;

        if (boyOuchNum >= boyOuch.Length)
        {
            boyOuchNum = 0;
        }
    }

    private void HandMove()
    {
        float speed = 1.0f * Time.deltaTime;
        if (isOutside)
        {
            nextDestination = windowPos.position;
            float t = (transform.position.x - originalPosition.x) / (windowPos.position.x - originalPosition.x);
            transform.rotation = Quaternion.Lerp(originalQuaternion, windowQuaternion, t);
        }
        else
        {
            nextDestination = babyPosition;
            float t = (transform.position.x - windowPos.position.x) / (babyPosition.x - windowPos.position.x);
            transform.rotation = Quaternion.Lerp(windowQuaternion, babyQuaternion, t);
            
            if (t > 0.7)
            {
                handAnimator.SetBool("PrepareGrab", true);
                handAnimator.SetBool("StartWave", false);
            }
        }
        
        if (windowPos.position == transform.position)
        {
            isOutside = false;
        }
        Vector3 pos = Vector3.MoveTowards(transform.position, nextDestination, speed);
        GetComponent<Rigidbody>().MovePosition(pos);
    }

    private void HandBack()
    {
        float speed = 1.0f * Time.deltaTime;
        if (!baby.GetComponent<Baby>().GetIsBabyGrab()) {
            speed = 3.0f * Time.deltaTime;
        } 
        

        if (isOutside)
        {
            nextDestination = originalPosition;
            float t = (transform.position.x - windowPos.position.x) / (originalPosition.x - windowPos.position.x);
            transform.rotation = Quaternion.Lerp(windowQuaternion, originalQuaternion, t);
            if (transform.position == originalPosition)
            {
                if (baby.GetComponent<Baby>().GetIsBabyGrab() && AttackRecord.handAttacking == handNumber)
                {

                    // loseText.SetActive(true);
                    // treeHouse.GetComponent<TreeHouseScript>().PlayBoyLaughter();
                    AttackRecord.isAttacking = false;
                    AttackRecord.handAttacking = 0;
                    AttackRecord.handAttackCount = 0;
                    SceneManager.LoadScene("TreeHouse");
                } 
                handAnimator.SetBool("LetGo", false);
                handAnimator.SetBool("BackToDefault", true);
            }
        }
        else
        {
            nextDestination = windowPos.position;
            float t = (transform.position.x - babyPosition.x) / (windowPos.position.x - babyPosition.x);
            transform.rotation = Quaternion.Lerp(babyQuaternion, windowQuaternion, t);
        }
        if (windowPos.position == transform.position)
        {
            isOutside = true;
            if (baby.GetComponent<Baby>().GetIsBabyGrab() && AttackRecord.handAttacking == handNumber)
            {
                baby.GetComponent<Baby>().PlayBabyNoAudio();
            }
        }
        Vector3 pos = Vector3.MoveTowards(transform.position, nextDestination, speed);
        GetComponent<Rigidbody>().MovePosition(pos);
    }

    void OnDrawGizmosSelected()
    { 
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(originalPosition, windowPos.position);
        Gizmos.DrawLine(windowPos.position, babyPosition);
    }
}
