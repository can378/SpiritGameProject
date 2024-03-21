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

    // ��� �޴� ����
    // UI : �޴� ���� 100%
    // �޴� ���� = ���ط� * �޴� ����
    public float defaultDefensivePower = 1f;
    public float addDefensivePower { get; set; }
    public float increasedDefensivePower { get; set; }
    public float decreasedDefensivePower { get; set; }
    public float defensivePower
    {
        get { return (defaultDefensivePower + addDefensivePower) * (1f + increasedDefensivePower) * (1f - decreasedDefensivePower); }
    }
    
    // �Ӽ� �޴� ����
    public float[] resist = new float[11] {1,1,1,1,1,1,1,1,1,1,1};  //������ ������ ���� ��������

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
