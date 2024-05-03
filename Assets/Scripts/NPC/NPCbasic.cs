using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;

public class NPCbasic : MonoBehaviour
{
    public GameObject NPCTarget;
    //public string NPCName;
    public GameObject DialogPanel;
    public TMP_Text DialogTextMesh;
    public int chapter = 0;

    public bool isTalking;
    public bool isWalking;
    public bool isInvincible;

    protected ScriptManager scriptManager;
    protected SpriteRenderer sprite;
    protected Stats stats;
    
    int index = 0;

    protected virtual void Start()
    {
        isTalking = false;
        sprite=GetComponent<SpriteRenderer>();
        scriptManager = GetComponent<ScriptManager>();
        stats = GetComponent<Stats>();
        //GetComponent<PathFinding>().seeker = this.transform;

    }

    void Update()
    {
        sprite.sortingOrder = Mathf.RoundToInt(transform.position.y) * -1;
    }

    void FixedUpdate()
    {
        //following player
        /*
        if (isWalking)
        {
            float targetDis = Vector2.Distance(transform.position, NPCTarget.transform.position);
            if (targetDis >= 3f && !(targetDis <= 1f))
            {
                GetComponent<PathFinding>().StartFinding
                    ((Vector2)transform.position, (Vector2)NPCTarget.transform.position);
            }
        }
        */
    }

    /*
    IEnumerator startConveration()
    {
        while (true)
        {
            print("대화");
            if (Input.GetKeyDown(KeyCode.F))
            {
                DialogPanel.SetActive(!DialogPanel.activeSelf);
                Conversation(index);
                index++;
            }
            yield return null;
        }
    }
    */

    //대화
    public virtual void Conversation()
    {
        
        DialogPanel.SetActive(true);

        if (scriptManager.NPCScript(chapter, index) == "border")
        {
            index--;
            DialogTextMesh.text = scriptManager.NPCScript(chapter, index - 1);
        }
        else if (scriptManager.NPCScript(chapter, index) == "wrong")
        {
            index++;
            DialogTextMesh.text = scriptManager.NPCScript(chapter, index + 1);
        }
        else
        {
            DialogTextMesh.text = scriptManager.NPCScript(chapter, index);
        }
        index++;
    }

    public virtual void ConversationOut()
    {
        StopAllCoroutines();
        DialogPanel.SetActive(false);
    }



    //Trigger===================================================================================

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerAttack" || other.tag == "EnemyAttack")
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

    public virtual void Attacked(GameObject attacker)
    {
        if(isInvincible)
            return;

        //Damaged
        HitDetection hitDetection = attacker.GetComponent<HitDetection>();

        AudioManager.instance.SFXPlay("Hit_SFX");

        Damaged(hitDetection.damage, hitDetection.critical, hitDetection.criticalDamage);

    }

    public virtual void Damaged(float damage, float critical = 0, float criticalDamage = 0)
    {
        bool criticalHit = Random.Range(0, 100) < critical * 100 ? true : false;
        damage = criticalHit ? damage * criticalDamage : damage;

        print(this.name + " damaged : " + (1 - stats.defensivePower) * damage);
        stats.HP -= (1 - stats.defensivePower) * damage;

        sprite.color = 0 < (1 - stats.defensivePower) * damage ? Color.red : Color.green;

        Invoke("DamagedOut", 0.05f);
        if (stats.HP <= 0f)
        {
            Dead();
        }
    }

    void DamagedOut()
    {
        sprite.color = Color.white;
    }

    public virtual void Dead()
    {
        Destroy(this.gameObject);
    }



}
