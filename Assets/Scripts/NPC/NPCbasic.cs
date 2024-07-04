using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class NPCbasic : ObjectBasic
{
    public bool isTalking;
    public bool isFollow;
    public bool isChase;

    // 적
    public ObjectBasic enemyTarget;
    public float enemyTargetDis = 99f;
    public float maxEnemyTargetDis;         // 사정거리

    // 동료
    public ObjectBasic companionTarget;
    public float companionTargetDis = 99f;
    public float maxCompanionTargetDis;     // 동료와 최대로 떨어질 수 있는 거리

    public GameObject DialogPanel;
    //public TMP_Text DialogTextMesh;
    public int chapter = 0;
    public int side = 0;    // 0 : 중립, 1 : 아군, 2 : 적군
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
        defaultLayer = this.gameObject.layer;
    }

    void Update()
    {
        Attack();
        Move();
        ChaseEnemy();
        FollowCompanion();
        WaitTarget();
    }

    #region Attack

    void Attack()
    {
        if (!enemyTarget)
            return;

        if (!isFlinch && !isAttack && isAttackReady && enemyTargetDis <= maxEnemyTargetDis)
        {
            moveVec = Vector2.zero;
            Pattern();
        }
    }

    protected virtual void Pattern()
    {
        isAttack = false;
        isAttackReady = false;
    }


    #endregion Attack

    #region Move

    void Move()
    {
        // 경직과 공격 중에는 직접 이동 불가
        if (isFlinch || isTalking || isAttack)
        {
            rigid.velocity = moveVec * stats.moveSpeed;
            return;
        }
       
        if(isChase)
        {
            moveVec = (enemyTarget.transform.position - transform.position).normalized;
        }
        else if (isFollow)
        {
            moveVec = (companionTarget.transform.position - transform.position).normalized;
        }
        else
        {
            moveVec = Vector3.zero;
        }

        rigid.velocity = moveVec * stats.moveSpeed;

    }

    // 타겟 대기중
    void WaitTarget()
    {
        if (!companionTarget)
            return;

        if (companionTarget.hitTarget == null)
            return;
        
        if(companionTarget.hitTarget == this.gameObject)
            return;

        OnTarget(companionTarget.hitTarget.GetOrAddComponent<ObjectBasic>());
    }

    // 타겟 감지
    void OnTarget(ObjectBasic target)
    {
        enemyTarget = target;
    }

    void ChaseEnemy()
    {
        //타겟이 없으면 반환
        if (!enemyTarget)
        {
            isChase = false;
            enemyTargetDis = 99f;
            return;
        }

        enemyTargetDis = Vector2.Distance(transform.position, enemyTarget.transform.position);

        // 적이 사정거리 내에 있을 시
        if (enemyTargetDis <= maxEnemyTargetDis )
        {
            isChase = false;
        }
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
            isFollow = false;
            companionTargetDis = 99f;
            return;
        }

        companionTargetDis = Vector2.Distance(transform.position, companionTarget.transform.position);
        
        if (companionTargetDis <= 5f)
        {
            isFollow = false;
        }
        else if (companionTargetDis < maxCompanionTargetDis)
        {
            isFollow = true;
        }
        else if (maxCompanionTargetDis <= companionTargetDis)
        {
            isFollow = true;
            enemyTarget = null;
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
            BeAttacked(other.gameObject.GetComponent<HitDetection>());
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Interaction") && DialogPanel != null)
        {
            ConversationOut();
        }
    }

    public override void Dead()
    {
        base.Dead();
    }



}
