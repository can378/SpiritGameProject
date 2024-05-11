using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����
public class StunDeBuff : StatusEffect
{
    // ���� �� ��ų, �̵� �Ұ�
    // �ǰ� �� ����

    public override void ApplyEffect()
    {
        ResetEffect();
    }

    public override void ResetEffect()
    {
        if (target.tag == "Player")
        {
            Player player = target.GetComponent<Player>();

            // ȿ�� ����
            player.isFlinch = true;

            // ��ø 
            overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

            // ���׿� ���� ���ӽð� ����
            duration = (1 - player.stats.SEResist) * defaultDuration;

            player.StopCoroutine(player.flinchCoroutine);
            player.flinchCoroutine = StartCoroutine(player.Flinch(duration));

            StartCoroutine(Stun());
        }
        else if (target.tag == "Enemy")
        {
            print("enemy stun - reset effect");
            EnemyStats enemy = target.GetComponent<EnemyStats>();
            
            enemy.isEnemyStun = true;
            duration = 4;
            
            StartCoroutine(Stun());
        }
        
    }

    IEnumerator Stun()
    {
        if(target.tag == "Player")
        {
            Player player = target.GetComponent<Player>();

            while(player.isFlinch)
            {
                yield return null;
            }

            duration = 0;
        }
        else if (target.tag == "Enemy")
        {
            //player���� isEnemyAttackalve false�̸� ���ؾȹް� �ϱ�!!!!!!!!!!
            print("enemy stun");
            target.GetComponent<EnemyBasic>().StopAllCoroutines();
            yield return new WaitForSeconds(duration);

            
        }
    }

    public override void RemoveEffect()
    {
        if (target.tag == "Player")
        {
            Player player = target.GetComponent<Player>();

            if (player.flinchCoroutine != null) StopCoroutine(player.flinchCoroutine);
        }
        else if (target.tag == "Enemy")
        {
            EnemyStats stats = target.GetComponent<EnemyStats>();
            stats.isEnemyStun = false;
            duration = 0;
            target.GetComponent<EnemyBasic>().RestartAllCoroutines();
            
        }
    }
}

