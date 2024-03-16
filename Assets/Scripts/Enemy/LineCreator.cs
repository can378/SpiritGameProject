using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCreator : MonoBehaviour
{
    public GameObject startObj;
    GameObject targetObj;
    
    public float lineLength;
    public float lineGrowSpeed;


    private LineRenderer lineRenderer;
    public float currentLineLength = 0f;
    public bool isLaserBeamRun=false;

    void Awake()
    {
        targetObj = GameObject.FindWithTag("Player");


        lineRenderer = transform.GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        // 라인 초기 길이
        lineRenderer.SetPosition(0, startObj.transform.position);
        lineRenderer.SetPosition(1, startObj.transform.position);

        //StartCoroutine(laserBeam());
    }

    public IEnumerator laserBeam() 
    {
        isLaserBeamRun = true;

        Vector3 targetPos=targetObj.transform.position;
        float distance;

        transform.gameObject.SetActive(true);

        while (true)
        {
            
            distance = Vector3.Distance(startObj.transform.position, targetPos);

            if (currentLineLength >= distance || 
                currentLineLength >= lineLength) 
            { break; }

            // 라인 길이 증가
            currentLineLength += lineGrowSpeed * Time.deltaTime;
            currentLineLength = Mathf.Min(currentLineLength, distance);
            yield return new WaitForSeconds(0.01f);

            // 시작점과 끝점 설정
            Vector3 startPoint = startObj.transform.position;
            Vector3 endPoint = startPoint +
                (targetPos - startObj.transform.position).normalized
                * currentLineLength;
            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, endPoint);

        }
        transform.gameObject.SetActive(false);
        //lineRenderer.material.color = Color.red;

        yield return null;
        isLaserBeamRun = false;
    }

}
