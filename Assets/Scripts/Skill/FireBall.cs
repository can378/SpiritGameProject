using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Skill
{
    [field: SerializeField] public int defalutDamage { get; private set; }
    [field: SerializeField] public float ratio { get; private set; }
    [field: SerializeField] public float size { get; private set; }
    [field: SerializeField] public float knockBack { get; private set; }
    [field: SerializeField] public GameObject FireBallEffect { get; private set; }
    [field: SerializeField] public GameObject[] StatusEffect { get; private set; }

    public override void Use(GameObject user)
    {
        this.user = user;
        StartCoroutine("Fire");
    }

    IEnumerator Fire()
    {
        Debug.Log("FireBall");

        if (user.tag == "Player")
        {
            Player player = user.GetComponent<Player>();
            // ��Ÿ�� ����
            skillCoolTime = (1 - player.stats.skillCoolTime) * skillDefalutCoolTime;

            // ����
            yield return new WaitForSeconds(preDelay / player.stats.attackSpeed);

            GameObject effect = Instantiate(FireBallEffect, player.status.mousePos, Quaternion.identity);
            effect.transform.localScale = new Vector3(size, size, 0);
            HitDetection hitDetection = effect.GetComponent<HitDetection>();
            /*
            ����ü = false
            ����� = -1
            �ٴ���Ʈ = false
            �ʴ� Ÿ�� Ƚ�� = -1 
            �Ӽ� = �� : 4
            ���ط� = ���ط� * �÷��̾� �ֹ���
            �˹� = �˹�
            ġȮ = 0
            ġ�� = 0
            ����� = ȭ��
            */
            hitDetection.SetHitDetection(false, -1, false, -1, defalutDamage + player.stats.skillPower * ratio, knockBack,0,0, StatusEffect);

            Destroy(effect, rate);
        }
    }

    public override void Exit(GameObject user)
    {

    }
}
