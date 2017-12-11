using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantAcornScript : MonoBehaviour {

    private Attack ScriptOfAttack;

    [SerializeField]
    private AudioSource acornSoundSource;
    [SerializeField]
    private AudioClip acornDrop;
    [SerializeField]
    private AudioClip acornPick;
    [SerializeField]
    private AudioClip acornCrack;

    private bool isGiant = false;
    private bool isAttack = false;
    private bool heldByLeft;
    private bool heldByRight;

    private Rigidbody rb;

    public void setGiant(bool isG)
    {
        isGiant = isG;
    }

    public bool getGiant()
    {
        return isGiant;
    }

    public void setAttack(bool isA)
    {
        isAttack = isA;
    }

    public void SetLeftHand(bool isHold)
    {
        heldByLeft = isHold;
    }

    public void SetRightHand(bool isHold)
    {
        heldByRight = isHold;
    }

    public bool getIsHeldByLeft()
    {
        return heldByLeft;
    }

    public bool getIsHeldByRight()
    {
        return heldByRight;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Table")
        {
            acornSoundSource.clip = acornDrop;
            acornSoundSource.Play();
        }
    }

    public void PlayPickUpSound()
    {
        acornSoundSource.clip = acornPick;
        acornSoundSource.Play();
    }

    public void PlayCrackSound()
    {
        acornSoundSource.clip = acornCrack;
        acornSoundSource.Play();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        if (isAttack)
        {
            Quaternion rotation = Quaternion.LookRotation(rb.velocity);
            transform.rotation = rotation;
            transform.Rotate(0, 90, 0);

        }
    }

}
