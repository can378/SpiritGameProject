using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FaceGreed : BossFace
{
    private Transform startPoint; // ���� ���۵� ��ġ
    public float lineWidth = 2f; // ���� �β�

    private LineRenderer lineRenderer;

    
    //Ž��=���� ũ�� ������. �̿��߿� ���ٴ��� ������µ� ���ٴڿ� ������ �ȵȴ�.
    //���ٴ��� ���������ʰ� �� ������ ���������� �����־ ������ �� �����ϰ� �ľ��Ѵ�.
    protected override void Start()
    {
        
        startPoint = transform;

        lineRenderer = gameObject.GetComponent<LineRenderer>();

        // �� �β�
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        // ��Ƽ����
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        // ���� ������ ���� ��ġ �ʱ�ȭ
        lineRenderer.positionCount = 2;
    }
    protected override void initAttack()
    {
        base.initAttack();
        startPoint = transform;
    }
    protected override void MovePattern()
    {
        //if (nowAttack) { base.MovePattern(); }
    }
    protected override void faceAttack()
    {
        // �� �����Ӹ��� ���� �� ���� ��ǥ�� ������Ʈ
        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(1, enemyStatus.enemyTarget.position);
    }


}
