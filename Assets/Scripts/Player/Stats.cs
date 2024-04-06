using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    /// <summary>
    /// default�� �����ڰ� ���ϴ� �⺻ ��ġ
    /// increased,decreased�� ������, ������
    /// add,sub�� + �߰�,����
    /// ������ ���� ��ġ
    /// </summary>

    //HP
    public float HPMax = 100;
    public float HP = 100;
    public float tempHP = 0;

    // ���ذ���
    // UI : ���ذ��� 0%
    // �޴� ���� = ���ط� * ���ذ���
    // �ִ�, �ּ� 75%
    public float defaultDefensivePower = 0f;
    public float addDefensivePower { get; set; }
    public float increasedDefensivePower { get; set; }
    public float decreasedDefensivePower { get; set; }
    public float defensivePower
    {
        get {
            float DP = (defaultDefensivePower + addDefensivePower) * (1f + increasedDefensivePower) * (1f - decreasedDefensivePower);
            if (DP > 0.75f)
                return 1 - 0.75f;
            else if (DP < -0.75f)
                return 1 + 0.75f;
            else
                return 1 - DP;
        }
    }

    // �����̻� ����
    // UI : �����̻� ���� 0%
    // �����̻� ȿ�� = ���ӽð� �Ǵ� ���ط� * �����̻� ����
    // �ִ�,�ּ� 75%
    public float defaultSEResist = 0f;
    public float addSEResist { get; set; }
    public float increasedSEResist { get; set; }
    public float decreasedSEResist { get; set; }
    public float SEResist
    {
        get {
            float SER = (defaultSEResist + addSEResist) * (1f + increasedSEResist) * (1f - decreasedSEResist);
            if(SER > 0.75f)
                return 1 - 0.75f;
            else if(SER < -0.75f)
                return 1 + 0.75f;
            else 
                return 1 - SER;
        }
    }

    // Attack
    // ���ݷ�
    // UI : ���ݷ� 0
    // �⺻ ���� ���ط� = ���ݷ�
    public float defaultAttackPower = 0f;
    public float addAttackPower { get; set; }
    public float increasedAttackPower { get; set; }
    public float decreasedAttackPower { get; set; }
    public float attackPower
    {
        get { return (defaultAttackPower + addAttackPower) * (1f + increasedAttackPower) * (1f - decreasedAttackPower); }
    }

    // Speed
    // �̵� �ӵ� 5
    // �̵� �ӵ� = �̵��ӵ�
    public float defaultMoveSpeed = 5f;
    public float addMoveSpeed { get; set; }
    public float increasedMoveSpeed { get; set; }
    public float decreasedMoveSpeed { get; set; }
    public float moveSpeed
    {
        get { return (defaultMoveSpeed + addMoveSpeed) * (1f + increasedMoveSpeed) * (1f - decreasedMoveSpeed); }
    }

    [field: SerializeField] public List<StatusEffect> activeEffects = new List<StatusEffect>();         //���� �����
}
