using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ȸ��ü
public class Rotor : Projectile
{
    [field: SerializeField] public float DPS { get; private set; }          // �ʴ� Ÿ�ݼ�

    void Start()
    {
        StartCoroutine(MultiHit());
    }

    IEnumerator MultiHit()
    {
        RaycastHit2D[] rayHits;

        while (true)
        {
            rayHits = Physics2D.CircleCastAll(transform.position, size/2, Vector2.up);
            foreach (RaycastHit2D hitObj in rayHits)
            {
                if (hitObj.transform.tag == "Enemy")
                    hitObj.transform.GetComponent<EnemyBasic>().Damaged(damage, critical, criticalDamage);
            }
            yield return new WaitForSeconds(1 / DPS);
        }

    }

    //���߿� �ٴ���Ʈ ���� ��
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Wall" || other.tag == "Door" || other.tag == "ShabbyWall")
        {
            Destroy(gameObject);
        }
    }
}
