using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ش� ������Ʈ�� ���¸� ��Ÿ���� Ŭ����
/// ���������� ���� ������ ����
/// </summary>
public class Status : MonoBehaviour
{
    public Vector2 moveVec;                     // �̵� ����

    /// <summary>
    /// �߰� �ӵ� (Ư�� ������ �� ��� ���� �Ǵ� �ӵ�, �켱 Status ���� ������Ʈ�� ����� ��)
    /// </summary>
    public float moveSpeedMultiplier = 1f;
    public Coroutine beAttackedCoroutine;       // �ǰ� �ڷ�ƾ
    public GameObject hitTarget;                // ������ ���� ���

    public bool isBeAttaked;                 // ���� ���� ����
    public float isFlinch;                   // ���� ����, ���� �Ұ�
    public bool isSuperArmor;               // ���� ����
    public bool isInvincible;               // ����
    public bool isAttack;                   // ����
    public bool isAttackReady = true;       // ���� �غ� (true�� ���� ����, false�� ���� �Ұ�)

    public Transform fearTarget;            // ������ �� ���
    public Coroutine runCoroutine;          // �������� �ڷ�ƾ

    public Coroutine watingCoroutine;       // ��� �ڷ�ƾ





}
