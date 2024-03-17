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

    // ���� ����
    public float defaultReduction = 0;
    public float increasedReduction { get; set; }
    public float addReduction { get; set; }
    public float reduction
    {
        get { return (1 + increasedReduction) * (0f + addReduction); }
    }
    
    // �Ӽ� ����
    public float[] resist = new float[11];  //������ ������ ���� ��������

    //Attack
    //���ݷ�
    public float defaultPower = 0f;
    public float increasedPower { get; set; }
    public float addPower { get; set; }
    public float power
    {
        get { return (1 + increasedPower) * (defaultPower + addPower); }
    }

    //Speed
    public float defaultMoveSpeed = 5f;
    public float increasedMoveSpeed { get; set; }
    public float addMoveSpeed { get; set; }
    public float moveSpeed
    {
        get { return (1 + increasedMoveSpeed) * (defaultMoveSpeed + addMoveSpeed); }
    }

    public List<StatusEffect> activeEffects = new List<StatusEffect>();         //���� �����
}
