using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;

public class NPCbasic : ObjectBasic
{
    public bool isTalking;
    public bool isFollow;
    public bool isChase;

    // ��
    public Transform enemyTarget;
    public float enemyTargetDis = 99f;
    public float maxEnemyTargetDis;         // ������ ������ �� �ִ� �ִ� �Ÿ�

    // ����
    public Transform companionTarget;
    public float companionTargetDis = 99f;
    public float maxCompanionTargetDis;     // ����� �ִ�� ������ �� �ִ� �Ÿ�

    public GameObject DialogPanel;
    //public TMP_Text DialogTextMesh;
    public int chapter = 0;
    public int side = 0;
    int index = 0;

    protected ScriptManager scriptManager;

    protected override void Awake()
    {
        base.Awake();
        stats = GetComponent<Stats>();
        scriptManager = GetComponent<ScriptManager>();

        //GetComponent<PathFinding>().seeker = this.transform;
    }

    protected virtual void Start()
    {
        isTalking = false;
    }

    void Update()
    {
        Attack();
        Move();
        ChaseEnemy();
        FollowCompanion();
    }

    #region Attack

    protected virtual void Attack()
    {

    }

    protected void AttackOut()
    {
        isAttack = false;
    }

    #endregion Attack

    #region Move

    protected virtual void Move()
    {
        // ������ ���� �߿��� ���� �̵� �Ұ�
        if (isFlinch || isAttack)
            return;

        // ��ȭ �� �̵� �Ұ�
        if (isTalking)
        {
            moveVec = Vector3.zero;
        }

        if(isChase && isFollow)
        {
            moveVec = (companionTarget.position - transform.position).normalized;
        }
        else if(isChase)
        {
            moveVec = (enemyTarget.position - transform.position).normalized;
        }
        else if(isFollow)
        {
            moveVec = (companionTarget.position - transform.position).normalized;
        }

        rigid.velocity = moveVec * stats.moveSpeed;

    }

    void ChaseEnemy()
    {
        //Ÿ���� ������ ��ȯ
        if (!enemyTarget)
        {
            enemyTargetDis = 99f;
            return;
        }

        enemyTargetDis = Vector2.Distance(transform.position, enemyTarget.position);

        // ���� �ʹ� �����ų�
        if (enemyTargetDis <= 2f || enemyTargetDis <= maxEnemyTargetDis)
        {
            isChase = false;
        }
        // ������ ����� ������ ������ ����
        else
        {
            isChase = true;
        }


    }

    void FollowCompanion()
    {
        // �Ʊ��� ������ ��ȯ
        if (!companionTarget)
        {
            companionTargetDis = 99f;
            return;
        }

        companionTargetDis = Vector2.Distance(transform.position, companionTarget.position);
        
        //�Ʊ��� �ʹ� �����ų�
        if (companionTargetDis <= 5f)
        {
            isFollow = false;
        }
        //�Ʊ��� �ʹ� �ָ� ������������ ���󰡱�
        else if (maxCompanionTargetDis <= companionTargetDis)
        {
            isFollow = true;
        }
    }

    #endregion Move

    #region Interaction

    //��ȭ
    public virtual void Conversation()
    {
        
        DialogPanel.SetActive(true);

        if (scriptManager.NPCScript(chapter, index) == "border")
        {
            index--;
            DialogPanel.GetComponent<TMP_Text>().text = scriptManager.NPCScript(chapter, index - 1);
        }
        else if (scriptManager.NPCScript(chapter, index) == "wrong")
        {
            index++;
            DialogPanel.GetComponent<TMP_Text>().text = scriptManager.NPCScript(chapter, index + 1);
        }
        else
        {
            DialogPanel.GetComponent<TMP_Text>().text = scriptManager.NPCScript(chapter, index);
        }
        index++;
    }

    public virtual void ConversationOut()
    {
        DialogPanel.SetActive(false);
    }

    #endregion Interaction

    //Trigger===================================================================================

    void OnTriggerEnter2D(Collider2D other)
    {
        if (((side == 0 || side == 2) && other.tag == "PlayerAttack") || ((side == 0 || side == 1) && other.tag == "EnemyAttack"))
        {
            Attacked(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && DialogPanel != null)
        {
            ConversationOut();
        }
    }

    public override void Dead()
    {
        base.Dead();
        Destroy(this.gameObject);
    }



}
