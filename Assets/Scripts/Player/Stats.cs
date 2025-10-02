using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [Header("������")]
    [field: SerializeField, ReadOnly] float _Value;
    [Header("�⺻��")]
    [field: SerializeField] float _DefaultValue;
    float _MaxValue;
    float _MinValue;
    [Header("������")]
    [field: SerializeField, ReadOnly] float _AddValue;
    [field: SerializeField, ReadOnly] float _IncreasedValue;
    [field: SerializeField, ReadOnly] float _DecreasedValue;

    public Stat(float _DefalutValue, float _MaxValue = float.MaxValue, float _MinValue = float.MinValue)
    {
        this._DefaultValue = _DefalutValue;
        this._MaxValue = _MaxValue;
        this._MinValue = _MinValue;
    }

    public void SetDefaultValue(float _DefalutValue)
    {
        this._DefaultValue = _DefalutValue;
        ResetValue();
    }

    //public void SetDefaultValue(float _DefalutValue) { this._DefalutValue = _DefalutValue; }

    // ���� ���� �����մϴ�.
    void ResetValue()
    {
        _Value = Mathf.Clamp((_DefaultValue + _AddValue) * (1f + _IncreasedValue) * (1f - _DecreasedValue), _MinValue, _MaxValue);
    }

    public float Value
    {
        get
        {
            ResetValue();
            return _Value;
        }
    }

    public float AddValue
    {
        get {return _AddValue;}
        set
        {
            _AddValue = value;
            ResetValue();
        }
    }

    public float IncreasedValue
    {
        get { return _IncreasedValue; }
        set
        {
            _IncreasedValue = value;
            ResetValue();
        }
    }

    public float DecreasedValue
    {
        get { return _DecreasedValue; }
        set
        {
            _DecreasedValue = value;
            ResetValue();
        }
    }

}

public class Stats : MonoBehaviour
{
    /// <summary>
    /// default�� �����ڰ� ���ϴ� �⺻ ��ġ
    /// increased,decreased�� ������, ������
    /// add,sub�� + �߰�,����
    /// ������ ���� ��ġ
    /// </summary>

    //HP
    [field: SerializeField] public Stat HPMax = new Stat(100, 999999, 0);
    [field: SerializeField] public float HP { get; set; } = 100f;
    [field: SerializeField] public float tempHP { get; set; } = 0;
    // ���ε�
    [field: SerializeField] public float poiseMax { get; set; } = 20f;
    [field: SerializeField] public float poise { get; set; } = 20f;

    // ����
    // UI : ���� 0%
    // �޴� ���� = ���ط� * ����
    // �ּ� -50%, �ִ� 50%
    [field: SerializeField] public Stat DefensivePower = new Stat(0.0f,0.5f,-0.5f);

    // �����̻� ����
    // UI : �����̻� ���� 0%
    // �����̻� ȿ�� = ���ӽð� �Ǵ� ���ط� * �����̻� ����
    // �ּ� -75%, �ִ� 75%
    [field: SerializeField] public Stat[] SEResist { get; set; } = 
    {
        new Stat(0.0f, 0.75f, -0.75f), 
        new Stat(0.0f, 0.75f, -0.75f), 
        new Stat(0.0f, 0.75f, -0.75f), 
        new Stat(0.0f, 0.75f, -0.75f)
    };

    // Attack
    // ���ݷ�
    // UI : ���ݷ� 0
    // �⺻ ���� ���ط� = ���ݷ�
    // �ּ� 0
    [field: SerializeField] public Stat AttackPower = new Stat(0.0f, float.MaxValue, 0);

    // ������Ʈ�� ���� �� ȿ��
    // �����̻� ȿ��
    [field: SerializeField] public List<SE_TYPE> SEType { get; set; } = new List<SE_TYPE>();

    // ���� ���� �� ȿ��
    [field: SerializeField] public List<COMMON_TYPE> CommonType { get; set; } = new List<COMMON_TYPE>();

    // ����ü ���� ȿ��
    [field: SerializeField] public List<PROJECTILE_TYPE> ProjectileType { get; set; } = new List<PROJECTILE_TYPE>();

    // Skill
    // SkillPower
    // UI ���� 0
    // ���� ���ط� = ���� �⺻ ���ط� + ���� * ��ų ���
    // �ּ� 0
    [field: SerializeField] public Stat SkillPower = new Stat(0.0f, float.MaxValue, 0);

    // Speed
    // �̵� �ӵ� 5
    // �̵� �ӵ� = �̵��ӵ�
    // �ּ� 0
    [field: SerializeField] public Stat MoveSpeed = new Stat(5f, float.MaxValue, 0);

    
    [field: SerializeField] public SerializedDictionary<int, PassiveData> activePassive = new SerializedDictionary<int, PassiveData>();         //���� �����
    [field: SerializeField] public SerializedDictionary<int, Buff> activeEffects = new SerializedDictionary<int, Buff>();         //���� �����
}
