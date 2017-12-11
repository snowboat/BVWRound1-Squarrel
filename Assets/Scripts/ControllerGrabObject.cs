using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGrabObject : MonoBehaviour {

    private SteamVR_TrackedObject trackedObj;
    
    private GameObject collidingObject;
    
    private GameObject objectInHand;
    private GameObject objectInHandSupport;

    private Animator acornAnimator;
    private Animator clawAnimator;

    [SerializeField]
    private bool isLeft;

    [SerializeField]
    private AudioSource controllerAudioSource;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        clawAnimator = GetComponentInChildren<Animator>();
    }

    private void SetCollidingObject(Collider col)
    {
    
        if (collidingObject || !col.GetComponent<Rigidbody>())
        {
            return;
        }

        collidingObject = col.gameObject;
        GiantAcornScript giantScript = collidingObject.GetComponent<GiantAcornScript>();
        if (giantScript)
        {
            if  (isLeft)
            {
                giantScript.SetLeftHand(true);
            } else
            {
                giantScript.SetRightHand(true);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
    }


    public void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }

    public void OnTriggerExit(Collider other)
    {
        if (!collidingObject)
        {
            return;
        }
        GiantAcornScript giantScript = collidingObject.GetComponent<GiantAcornScript>();
        if (giantScript)
        {
            if (isLeft)
            {
                giantScript.SetLeftHand(false);
            }
            else
            {
                giantScript.SetRightHand(false);
            }
        }
        collidingObject = null;
    }

    private void GrabObject()
    {
        objectInHand = collidingObject;
        collidingObject = null;

        var joint = AddFixedJoint();
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
        objectInHand.GetComponent<GiantAcornScript>().PlayPickUpSound();
    }


    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    private void ReleaseObject()
    {

        if (GetComponent<FixedJoint>())
        {

            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());

            if ((Mathf.Abs(Controller.velocity.y) < 8) && (Controller.velocity.x * Controller.velocity.x + Controller.velocity.y * Controller.velocity.y + Controller.velocity.z * Controller.velocity.z > 4))
            {
                objectInHand.GetComponent<Rigidbody>().velocity = Controller.velocity * 2.0f;
                objectInHand.GetComponent<GiantAcornScript>().setAttack(true);
                acornAnimator = objectInHand.GetComponent<Animator>();
                if (acornAnimator)
                {
                    acornAnimator.SetTrigger("isSpinning");
                }
                controllerAudioSource.Play();
                // Debug.Log("Double the speed");
            } 
            else {
                objectInHand.GetComponent<Rigidbody>().velocity = Controller.velocity;
                objectInHand.GetComponent<Rigidbody>().angularVelocity = Controller.angularVelocity;
                // Debug.Log("Usual speed");
            }
            // Debug.Log(objectInHand.GetComponent<Rigidbody>().velocity);
        }

        objectInHand = null;
    }

    private void Update()
    {

        if (Controller.GetHairTriggerDown())
        {
            clawAnimator.SetBool("isClawGrab", true);
            if (collidingObject)
            {
                GiantAcornScript giantScript = collidingObject.GetComponent<GiantAcornScript>();
                if (giantScript)
                {
                    if (giantScript.getGiant())
                    {
                        if (giantScript.getIsHeldByLeft() && giantScript.getIsHeldByRight())
                        {
                            GrabObject();
                        }
                    }
                    else
                    {
                        GrabObject();
                    }
                }
                    
            }
        }

        if (Controller.GetHairTriggerUp())
        {
            clawAnimator.SetBool("isClawGrab", false);
            if (objectInHand)
            {
                ReleaseObject();
            }
        }

        if (objectInHand && collidingObject)
        {
            GiantAcornScript giantScript = collidingObject.GetComponent<GiantAcornScript>();
            if (giantScript && giantScript.getGiant())
            {
                if ((!giantScript.getIsHeldByLeft()) || (!giantScript.getIsHeldByRight()) )
                {
                    ReleaseObject();
                }
            }
        }
    }
}