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

    // ����
    // UI : ���� 0%
    // �޴� ���� = ���ط� * ����
    // �ּ� -75%, �ִ� 75%
    public float defaultDefensivePower = 0f;
    public float addDefensivePower { get; set; }
    public float increasedDefensivePower { get; set; }
    public float decreasedDefensivePower { get; set; }
    public float defensivePower
    {
        get {
            float DP = (defaultDefensivePower + addDefensivePower) * (1f + increasedDefensivePower) * (1f - decreasedDefensivePower);
            if (DP > 0.75f)
                return 0.75f;
            else if (DP < -0.75f)
                return -0.75f;
            else
                return DP;
        }
    }

    // �����̻� ����
    // UI : �����̻� ���� 0%
    // �����̻� ȿ�� = ���ӽð� �Ǵ� ���ط� * �����̻� ����
    // �ּ� -75%, �ִ� 75%
    public float defaultSEResist = 0f;
    public float addSEResist { get; set; }
    public float increasedSEResist { get; set; }
    public float decreasedSEResist { get; set; }
    public float SEResist
    {
        get {
            float SER = (defaultSEResist + addSEResist) * (1f + increasedSEResist) * (1f - decreasedSEResist);
            if(SER > 0.75f)
                return 0.75f;
            else if(SER < -0.75f)
                return -0.75f;
            else 
                return SER;
        }
    }

    // Attack
    // ���ݷ�
    // UI : ���ݷ� 0
    // �⺻ ���� ���ط� = ���ݷ�
    // �ּ� 0
    public float defaultAttackPower = 0f;
    public float addAttackPower { get; set; }
    public float increasedAttackPower { get; set; }
    public float decreasedAttackPower { get; set; }
    public float attackPower
    {
        get
        {
            float AP = (defaultAttackPower + addAttackPower) * (1f + increasedAttackPower) * (1f - decreasedAttackPower);
            if (AP <= 0)
                return 0;
            return AP;
        }
    }

    // Speed
    // �̵� �ӵ� 5
    // �̵� �ӵ� = �̵��ӵ�
    // �ּ� 0
    public float defaultMoveSpeed = 5f;
    public float addMoveSpeed { get; set; }
    public float increasedMoveSpeed { get; set; }
    public float decreasedMoveSpeed { get; set; }
    public float moveSpeed
    {
        get
        {
            float MS = (defaultMoveSpeed + addMoveSpeed) * (1f + increasedMoveSpeed) * (1f - decreasedMoveSpeed);
            if (MS <= 0)
                return 0;
            return MS;
        }
    }

    [field: SerializeField] public List<StatusEffect> activeEffects = new List<StatusEffect>();         //���� �����
}
