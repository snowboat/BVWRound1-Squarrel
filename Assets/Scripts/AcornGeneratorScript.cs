using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcornGeneratorScript : MonoBehaviour {

    [SerializeField]
    private GameObject acornPrefeb;

    private int numberOfNormalAcorn;
    private int numberOfGiantAcorn;

    IEnumerator GenerateNormalAcorn()
    {
        if (numberOfNormalAcorn > 0)
        {
            numberOfNormalAcorn--;
            yield return new WaitForSeconds(0.25f);
            GameObject newAcorn = Instantiate(acornPrefeb, transform);
            newAcorn.transform.position = new Vector3(Random.Range(-1.5f, 1.5f), newAcorn.transform.position.y, Random.Range(-0.8f, 0.8f));
            float scaleSize = Random.Range(5.0f, 9.0f);
            newAcorn.transform.localScale = new Vector3(scaleSize, scaleSize, scaleSize);
            newAcorn.transform.Rotate(Random.Range(0.0f, 180.0f), Random.Range(0.0f, 180.0f), Random.Range(0.0f, 180.0f));
            GiantAcornScript giantScript = newAcorn.GetComponent<GiantAcornScript>();
            giantScript.setGiant(false);
            StartCoroutine(GenerateNormalAcorn());
        }
    }

    IEnumerator GenerateGiantAcorn()
    {
        if (numberOfGiantAcorn > 0)
        {
            numberOfGiantAcorn--;
            yield return new WaitForSeconds(1.5f);
            GameObject newAcorn = Instantiate(acornPrefeb, transform);
            newAcorn.transform.position = new Vector3(Random.Range(-0.3f, 0.3f), newAcorn.transform.position.y, Random.Range(-0.3f, 0.3f));
            float scaleSize = Random.Range(14.0f, 18.0f);
            newAcorn.transform.localScale = new Vector3(scaleSize, scaleSize, scaleSize);
            newAcorn.transform.Rotate(Random.Range(0.0f, 180.0f), Random.Range(0.0f, 180.0f), Random.Range(0.0f, 180.0f));
            GiantAcornScript giantScript = newAcorn.GetComponent<GiantAcornScript>();
            giantScript.setGiant(true);
            StartCoroutine(GenerateGiantAcorn());
        }
    }

    public void GenerateNormalAcorn(int num)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject newAcorn = Instantiate(acornPrefeb, transform);
            float pos_x = Random.Range(0.5f, 1.2f);
            float pos_z = Random.Range(0.5f, 0.7f);
            int pos = Random.Range(0, 4);
            switch(pos)
            {
                case 0:
                    newAcorn.transform.position = new Vector3(pos_x, newAcorn.transform.position.y, pos_z);
                    break;
                case 1:
                    newAcorn.transform.position = new Vector3(-pos_x, newAcorn.transform.position.y, pos_z);
                    break;
                case 2:
                    newAcorn.transform.position = new Vector3(-pos_x, newAcorn.transform.position.y, -pos_z);
                    break;
                case 3:
                    newAcorn.transform.position = new Vector3(pos_x, newAcorn.transform.position.y, -pos_z);
                    break;
            }
            float scaleSize = Random.Range(6.0f, 8.0f);
            newAcorn.transform.localScale = new Vector3(scaleSize, scaleSize, scaleSize);
            newAcorn.transform.Rotate(Random.Range(0.0f, 180.0f), Random.Range(0.0f, 180.0f), Random.Range(0.0f, 180.0f));
            GiantAcornScript giantScript = newAcorn.GetComponent<GiantAcornScript>();
            giantScript.setGiant(false);
        }
    }

    public void GenerateGiantAcorn(int num)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject newAcorn = Instantiate(acornPrefeb, transform);
            float pos_x = Random.Range(0.5f, 0.7f);
            float pos_z = Random.Range(0.5f, 0.6f);
            int pos = Random.Range(0, 4);
            switch (pos)
            {
                case 0:
                    newAcorn.transform.position = new Vector3(pos_x, newAcorn.transform.position.y, pos_z);
                    break;
                case 1:
                    newAcorn.transform.position = new Vector3(-pos_x, newAcorn.transform.position.y, pos_z);
                    break;
                case 2:
                    newAcorn.transform.position = new Vector3(-pos_x, newAcorn.transform.position.y, -pos_z);
                    break;
                case 3:
                    newAcorn.transform.position = new Vector3(pos_x, newAcorn.transform.position.y, -pos_z);
                    break;
            }
            float scaleSize = Random.Range(16.0f, 18.0f);
            newAcorn.transform.localScale = new Vector3(scaleSize, scaleSize, scaleSize);
            newAcorn.transform.Rotate(Random.Range(0.0f, 180.0f), Random.Range(0.0f, 180.0f), Random.Range(0.0f, 180.0f));
            GiantAcornScript giantScript = newAcorn.GetComponent<GiantAcornScript>();
            giantScript.setGiant(true);
        }
    }

    // Generate Acorn in the same time
    /*
    public void generateAcorn(int numOfNormal, int numOfGiant)
    {
        for (int i = 0; i < numOfNormal; i++)
        {
            GameObject newAcorn = Instantiate(acornPrefeb, transform);
            newAcorn.transform.position = new Vector3(Random.Range(-1.5f, 1.5f), newAcorn.transform.position.y, Random.Range(-1.0f, 1.0f));
            float scaleSize = Random.Range(5.0f, 9.0f);
            newAcorn.transform.localScale = new Vector3(scaleSize, scaleSize, scaleSize);
            newAcorn.transform.Rotate(Random.Range(0.0f, 180.0f), Random.Range(0.0f, 180.0f), Random.Range(0.0f, 180.0f));
            GiantAcornScript giantScript = newAcorn.GetComponent<GiantAcornScript>();
            giantScript.setGiant(false);
        }

        for (int i = 0; i < numOfGiant; i++)
        {
            GameObject newAcorn = Instantiate(acornPrefeb, transform);
            newAcorn.transform.position = new Vector3(Random.Range(-0.3f, 0.3f), newAcorn.transform.position.y, Random.Range(-0.3f, 0.3f));
            float scaleSize = Random.Range(15.0f, 19.0f);
            newAcorn.transform.localScale = new Vector3(scaleSize, scaleSize, scaleSize);
            newAcorn.transform.Rotate(Random.Range(0.0f, 180.0f), Random.Range(0.0f, 180.0f), Random.Range(0.0f, 180.0f));
            GiantAcornScript giantScript = newAcorn.GetComponent<GiantAcornScript>();
            giantScript.setGiant(true);
        }
    }
    */

    private void Start()
    {
        numberOfNormalAcorn = 20;
        numberOfGiantAcorn = 2;
        // StartCoroutine(GenerateNormalAcorn());
        // StartCoroutine(GenerateGiantAcorn());
    }


}
