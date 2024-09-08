using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FaceGreed : BossFace
{

    public float lineWidth = 2f; // ���� �β�


    public LineRenderer lineRenderer;
    private List<Vector3> positions; // ���� �׸� ��ġ���� ����Ʈ


    //Ž��=���� ũ�� ���� ���·� �Ѿƿ´�. �̿��߿� ���� ���ٴڿ� ������ �ȵȴ�.
    //���ٴ��� ���������ʰ� �� ������ ���������� �����־ ������ �� �����ϰ� �ľ��Ѵ�.
    protected override void Start()
    {
        base.Start();


        //lineRenderer = gameObject.GetComponent<LineRenderer>();

        // �� �β�,material
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        // �� init
        lineRenderer.positionCount = 0;
    }



    protected override void init()
    {
        base.init();
        lineRenderer.positionCount = 0;
        positions = new List<Vector3>();
    }





    protected override void MovePattern()
    {
        Chase();
    }

    protected override void AttackPattern()
    {
        base.AttackPattern();
        if (nowAttack && isFaceAttack)
        {
            if (countTime <=1f && isReady)
            {
                StartCoroutine(ClearLineGradually());
            }
        }

    }
    protected override void faceAttack()
    {
        // �� �����Ӹ��� ���� �� ���� ��ǥ�� ������Ʈ
        //endPoint=transform;
        //lineRenderer.SetPosition(0, startPoint.position);
        //lineRenderer.SetPosition(1, endPoint.position);

        /*
        // ���� ��ġ�� positions�� �߰�
        positions.Add(transform.position);

        // Line
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
        */

        // ���� ��ġ���� �Ÿ� ���
        if (positions.Count == 0)
        {
            // ���� �Ÿ� �̻� �̵����� ���� positions�� �߰�
            positions.Add(transform.position);


        }
        else 
        { 
        if (Vector3.Distance(positions[positions.Count - 1], transform.position) >= 0.5f)
        {
            // ���� �Ÿ� �̻� �̵����� ���� positions�� �߰�
            positions.Add(transform.position);

            // LineRenderer�� ����
            lineRenderer.positionCount = positions.Count;
            lineRenderer.SetPositions(positions.ToArray());
        }
        }

    }
    private bool isReady = true;
    public IEnumerator ClearLineGradually()
    {
        isReady = false;

        float duration = 0.3f; // ��ü ���� �ð�
        float interval = duration / positions.Count; // �� ���� ������� ����

        while (positions.Count > 0)
        {
            // ���� ������ ���� ����
            positions.RemoveAt(0);
            lineRenderer.positionCount = positions.Count;
            lineRenderer.SetPositions(positions.ToArray());

            yield return new WaitForSeconds(interval);

        }
        lineRenderer.positionCount = 0;
        isReady = true;
    }




}
