using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    // �ܺο��� ���� �� ���� �ִ� �÷��̾� ������
    // ���������Ϳ� �ֱ� �ָ��� �͵��� �켱 �����

    [field: SerializeField] public Vector2 mousePos { get; set; }
    [field: SerializeField] public Vector2 mouseDir { get; set; }
    [field: SerializeField] public float mouseAngle { get; set; }

    [field: SerializeField] public float runCurrentCoolTime { get; set; }           // �޸��� ���ð�
         
    [field: SerializeField] public bool isPlayerMove { get; set; }

    [field: SerializeField] public bool isReload {get; set;}                        //����
    [field: SerializeField] public bool isSprint { get; set; }                      //�޸���
    [field: SerializeField] public bool isDodge { get; set; }                       //ȸ��

    [field: SerializeField] public bool isInvincible { get; set; }                  //���� ����
    [field: SerializeField] public bool isAttackable { get; set; }                  //���ݰ��� ����

    [field: SerializeField] public bool isAttack { get; set; }                      // ���� ��
    [field: SerializeField] public float attackDelay { get; set; }                  // ���� ���ݱ��� ���ð�
    [field: SerializeField] public bool isAttackReady { get; set; }                 // ���� �غ� �Ϸ�

    [field: SerializeField] public bool isSkill { get; set; }                       // ��ų ��� ��
    [field: SerializeField] public bool isSkillReady { get; set; }                  // ��ų �غ� ��
    [field: SerializeField] public bool isSkillHold { get; set; }                   // ��ų Ȧ�� ��

    [field: SerializeField] public List<StatusEffect> activeEffects = new List<StatusEffect>();         //���� �����
}
