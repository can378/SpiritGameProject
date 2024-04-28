using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCreator : MonoBehaviour
{
    public GameObject startObj;

    public float lineLength;
    public float lineGrowSpeed;


    private LineRenderer lineRenderer;
    public float currentLineLength = 0f;
    public bool isLaserBeamRun=false;


    float distance;
    Transform target;

    void Start()
    {
        lineRenderer = transform.GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        // ���� �ʱ� ����
        lineRenderer.SetPosition(0, startObj.transform.position);
        lineRenderer.SetPosition(1, startObj.transform.position);

        //StartCoroutine(laserBeam());
    }

    public IEnumerator laserBeam() 
    {
        isLaserBeamRun = true;
        transform.gameObject.SetActive(true);
        
        target= GameObject.FindWithTag("Player").transform;
        


        while (true)
        {
            
            distance = Vector3.Distance(startObj.transform.position, target.position);

            if (currentLineLength >= distance || 
                currentLineLength >= lineLength) 
            { break; }

            // ���� ���� ����
            currentLineLength += lineGrowSpeed * Time.deltaTime;
            currentLineLength = Mathf.Min(currentLineLength, distance);
            yield return new WaitForSeconds(0.01f);

            // �������� ���� ����
            Vector3 startPoint = startObj.transform.position;
            Vector3 endPoint = startPoint +
                (target.position - startObj.transform.position).normalized
                * currentLineLength;
            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, endPoint);

        }
        
        

        transform.gameObject.SetActive(false);
        isLaserBeamRun = false;
        yield return null;
    }

}
