using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guiding : MonoBehaviour
{
    public Transform guidingTarget;
    public float speed;
    public float angular;

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        //StartGuide();
    }

    void FixedUpdate()
    {
        Guide();
    }

    void StartGuide()
    {
        //타겟이 없으면 반환
        if (!guidingTarget)
        {
            return;
        }

        rigid.velocity = (guidingTarget.position - transform.position).normalized * speed;
    }

    void Guide()
    {
        if (!guidingTarget)
        {
            return;
        }

        Vector2 angle = ((Vector2)(guidingTarget.position - transform.position).normalized - rigid.velocity.normalized).normalized;
        rigid.velocity = (rigid.velocity.normalized + angle * Time.fixedDeltaTime * angular).normalized * speed;
    }


}
