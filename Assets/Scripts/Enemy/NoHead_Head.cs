using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoHead_Head : MonoBehaviour
{
    public LineCreator lineCreator;

    void Start()
    { StartCoroutine(head()); }

    private void OnEnable()
    { StartCoroutine(head()); }

    private void OnDisable()
    { StopCoroutine(head()); }



    IEnumerator head() 
    {
        //vomit
        if (lineCreator.isLaserBeamRun == false)
        {
            //StartCoroutine(lineCreator.laserBeam());
            yield return new WaitForSeconds(5f);
        }

    }
    
}
