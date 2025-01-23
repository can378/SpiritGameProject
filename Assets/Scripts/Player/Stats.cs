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
    [field :SerializeField] public float HPMax { get; set; } = 100f;
    [field: SerializeField] public float HP { get; set; } = 100f;
    [field: SerializeField] public float tempHP { get; set; } = 0;
    // ���ε�
    [field: SerializeField] public float poiseMax { get; set; } = 20f;
    [field: SerializeField] public float poise { get; set; } = 20f;

    // ����
    // UI : ���� 0%
    // �޴� ���� = ���ط� * ����
    // �ּ� -50%, �ִ� 50%
    [field: SerializeField] public float defaultDefensivePower { get; set; } = 0f;
    public float addDefensivePower { get; set; }
    public float increasedDefensivePower { get; set; }
    public float decreasedDefensivePower { get; set; }
    public float defensivePower
    {
        get { return Mathf.Clamp((defaultDefensivePower + addDefensivePower) * (1f + increasedDefensivePower) * (1f - decreasedDefensivePower), -0.5f, 0.5f); }
    }

    // �����̻� ����
    // UI : �����̻� ���� 0%
    // �����̻� ȿ�� = ���ӽð� �Ǵ� ���ط� * �����̻� ����
    // �ּ� -75%, �ִ� 75%
    [field: SerializeField] public float[] defaultSEResist { get; set; } = new float[(int)BuffType.SPECIAL];
    public float[] addSEResist { get; set; } = new float[(int)BuffType.SPECIAL];
    public float[] increasedSEResist { get; set; } = new float[(int)BuffType.SPECIAL];
    public float[] decreasedSEResist { get; set; } = new float[(int)BuffType.SPECIAL];
    public float SEResist(int index)
    {
        return Mathf.Clamp((defaultSEResist[index] + addSEResist[index]) * (1f + increasedSEResist[index]) * (1f - decreasedSEResist[index]), -1f, 1f);
    }

    // Attack
    // ���ݷ�
    // UI : ���ݷ� 0
    // �⺻ ���� ���ط� = ���ݷ�
    // �ּ� 0
    [field: SerializeField] public float defaultAttackPower { get; set; } = 0f;
    public float addAttackPower { get; set; }
    public float increasedAttackPower { get; set; }
    public float decreasedAttackPower { get; set; }
    public float attackPower
    {
        get{ return Mathf.Clamp((defaultAttackPower + addAttackPower) * (1f + increasedAttackPower) * (1f - decreasedAttackPower), 0, 9999f); }
    }

    // Speed
    // �̵� �ӵ� 5
    // �̵� �ӵ� = �̵��ӵ�
    // �ּ� 0
    [field: SerializeField] public float defaultMoveSpeed { get; set; } = 5f;
    public float addMoveSpeed { get; set; }
    public float increasedMoveSpeed { get; set; }
    public float decreasedMoveSpeed { get; set; }
    public float moveSpeed
    {
        get { return Mathf.Clamp((defaultMoveSpeed + addMoveSpeed) * (1f + increasedMoveSpeed) * (1f - decreasedMoveSpeed), 0, 20f); }
    }

    [field: SerializeField] public List<StatusEffect> activeEffects = new List<StatusEffect>();         //���� �����
}
