using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;

public class NPCbasic : MonoBehaviour
{
    public GameObject NPCTarget;
    //public string NPCName;
    private SpriteRenderer sprite;
    public bool isTalking;
    public GameObject DialogPanel;
    public TMP_Text DialogTextMesh;

    public bool isWalking;


    ScriptManager scriptManager;
    public int chapter=0;

    int index = 0;

    void Start()
    {
        isTalking = false;
        sprite=GetComponent<SpriteRenderer>();
        //GetComponent<PathFinding>().seeker = this.transform;
        scriptManager = GetComponent<ScriptManager>();
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&&DialogPanel!=null)
        {
            ConversationOut();
        }
    }



}
