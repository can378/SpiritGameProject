using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    // �÷��̾� ����
    [field : SerializeField] public float runCurrentCoolTime { get; set; }       // �޸��� ���ð�
    [field: SerializeField] public float attackDelay { get; set; }              // ���� ���ð�
    [field: SerializeField] public float subWeaponDelay {get; set; }            // subWeaponDelay �غ� �ð�

    [field: SerializeField] public bool isReload {get; set;}               //����
    [field: SerializeField] public bool isSprint { get; set; }                 //�޸���
    [field: SerializeField] public bool isDodge { get; set; }                //ȸ��
    [field: SerializeField] public bool isAttack { get; set; }                //����
    [field: SerializeField] public bool isAttackReady { get; set; }           //���� �غ� �Ϸ�
    [field: SerializeField] public bool isEquip { get; set; }                 //���� ���
    [field: SerializeField] public bool isInvincible { get; set; }            //���� ����
    [field: SerializeField] public bool isAttackable { get; set; }             //���ݰ��� ����

    [field: SerializeField] public bool isSubWeapon { get; set; }               // �������� �����
    [field: SerializeField] public bool isGuard { get; set; }                   // ����
    [field: SerializeField] public bool isParry { get; set; }                   // �ݰ�
    [field: SerializeField] public bool isSubWeaponReady { get; set; }          // �������� �غ� �Ϸ�

    [field: SerializeField] public bool isStun { get; set; }                    //�ൿ �Ҵ�



    // �� �Ʒ� �ִ� �͵��� player �ɷ�ġ
    // userdata�� �Űܾ� ��
    public float speed = 5;                     
    public float runSpeed = 1.33f;                
    public float dodgeSpeed = 1.66f;                
    public float dodgeFrame = 0.5f;               
    public float runCoolTime = 5f;
    public bool isPlayerMove = true;

    public float playerPower;
    public int playerCritical;
    public float playerCriticalDamage;
    public float playerDrain;
}
