using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceGreed : EnemyBasic
{
    private Transform startPoint; // ���� ���۵� ��ġ
    private Transform target; // �̵� ��ǥ
    public float lineWidth = 2f; // ���� �β�

    private LineRenderer lineRenderer;


    //Ž��=���� ũ�� ������. �̿��߿� ���ٴ��� ������µ� ���ٴڿ� ������ �ȵȴ�.
    //���ٴ��� ���������ʰ� �� ������ ���������� �����־ ������ �� �����ϰ� �ľ��Ѵ�.
    private void Start()
    {

        startPoint = transform;
        target = GameObject.FindWithTag("Player").transform;

        lineRenderer = gameObject.GetComponent<LineRenderer>();

        // �� �β�
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        // ��Ƽ����
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        // ���� ������ ���� ��ġ �ʱ�ȭ
        lineRenderer.positionCount = 2;
    }
    protected override void AttackPattern()
    {
        print("greed1");
        StartCoroutine(greed());
    }

    IEnumerator greed()
    {
        print("greed2");
        isAttack = true;
        isAttackReady = false;

        //START
        startPoint = transform;
        target = GameObject.FindWithTag("Player").transform;

        // �� �����Ӹ��� ���� �� ���� ��ǥ�� ������Ʈ
        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(1, target.position);


        //END
        isAttack = false;
        yield return new WaitForSeconds(1f);
        isAttackReady = true;

    }
}
