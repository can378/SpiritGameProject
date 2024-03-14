using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{

    public float laserLength;
    public GameObject startPos;
    public GameObject laser;
    private LineRenderer lineRend;
    private GameObject target;

    Vector2 dirVec;
    void Start()
    {
        lineRend = laser.GetComponent<LineRenderer>();
        target = GameObject.FindWithTag("Player");

        StartCoroutine(shotBeam());
    }


    IEnumerator shotBeam() {

        /*
        //방향조정
        dirVec = (target.transform.position - transform.position).normalized;
        transform.LookAt(dirVec);


        //laser쏘기
        Vector3 endPos = startPos.transform.position + (transform.up * laserLength);
        lineRend.SetPositions(new Vector3[] { startPos.transform.position, endPos });
        */

        Vector3 endPos;
        for (int i = 5; i >= 1; i--)
        {
            endPos = (target.transform.position - transform.position) *Mathf.Pow(1/2,i);
            lineRend.SetPositions(new Vector3[] { startPos.transform.position, endPos });
            yield return new WaitForSeconds(1f);
        }
        //endPos = target.transform.position;
        //lineRend.SetPositions(new Vector3[] { startPos.transform.position, endPos });


        yield return new WaitForSeconds(1f);
        StartCoroutine(shotBeam());

    }
}
