using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    private GameObject shakingCamera;

    // degree of shaking
    
    private float shakeAmount;
    /*
    private float startTime;
    private float increaseDuration = 6.5f;
    private float decreaseDuration = 2.5f;
    private bool isIncrease = false;

    private bool isShake = false;
    */


    void FixedUpdate()
    {
        /*
        if (isShake)
        {
            if (isIncrease)
            {
                shakeAmount = Mathf.Lerp(0.0f, 0.3f, (Time.time - startTime) / increaseDuration);
                if (Time.time - startTime >= increaseDuration)
                {
                    isIncrease = false;
                    startTime = Time.time;
                }
            } else
            {
                shakeAmount = Mathf.Lerp(0.3f, 0.0f, (Time.time - startTime) / decreaseDuration);
            }
            ShakeCamera();
        }
        */
    }

    /*
    private void ShakeCamera()
    {
        shakingCamera.transform.position = Random.insideUnitSphere * shakeAmount;
    }

    public void SetIsShake(bool shake)
    {
        isShake = shake;
        if (shake)
        {
            startTime = Time.time;
            isIncrease = true;
        }
    }

    public void SetShakeAmount(float amount)
    {
        shakeAmount = amount;
    }
    */

    IEnumerator ShakingCameraForPunch()
    {
        float startTime = Time.time;
        float stepTime = Time.deltaTime * 2;
        float duration = 2.0f;
        while (Time.time - startTime < duration)
        {
            shakeAmount = Mathf.Lerp(0.2f, 0.0f, (Time.time - startTime) / duration);
            shakingCamera.transform.position = Random.insideUnitSphere * shakeAmount;
            yield return new WaitForSeconds(stepTime);
        }
    }
    public void PunchShaking()
    {
        StartCoroutine(ShakingCameraForPunch());
    }
}
