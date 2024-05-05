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

    // 적
    public Transform enemyTarget;
    public float enemyTargetDis = 99f;
    public float maxEnemyTargetDis;         // 적에게 공격할 수 있는 최대 거리

    // 동료
    public Transform companionTarget;
    public float companionTargetDis = 99f;
    public float maxCompanionTargetDis;     // 동료와 최대로 떨어질 수 있는 거리

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
        // 경직과 공격 중에는 직접 이동 불가
        if (isFlinch || isAttack)
            return;

        // 대화 중 이동 불가
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
        //타겟이 없으면 반환
        if (!enemyTarget)
        {
            enemyTargetDis = 99f;
            return;
        }

        enemyTargetDis = Vector2.Distance(transform.position, enemyTarget.position);

        // 적과 너무 가깝거나
        if (enemyTargetDis <= 2f || enemyTargetDis <= maxEnemyTargetDis)
        {
            isChase = false;
        }
        // 적군이 충분히 가까이 있으면 추적
        else
        {
            isChase = true;
        }


    }

    void FollowCompanion()
    {
        // 아군이 없으면 반환
        if (!companionTarget)
        {
            companionTargetDis = 99f;
            return;
        }

        companionTargetDis = Vector2.Distance(transform.position, companionTarget.position);
        
        //아군과 너무 가깝거나
        if (companionTargetDis <= 5f)
        {
            isFollow = false;
        }
        //아군과 너무 멀리 떨어져있으면 따라가기
        else if (maxCompanionTargetDis <= companionTargetDis)
        {
            isFollow = true;
        }
    }

    #endregion Move

    #region Interaction

    //대화
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
