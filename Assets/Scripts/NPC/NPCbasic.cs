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
    private bool playerInRange = false;
    public bool isWalking;


    int index = 0;

    void Start()
    {
        isTalking = false;
        sprite=GetComponent<SpriteRenderer>();
        GetComponent<PathFinding>().seeker = this.transform;
    }



    void Update()
    {
        sprite.sortingOrder = Mathf.RoundToInt(transform.position.y) * -1;
   

        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            Conversation(index);
            index++;
        }
    }

    private void FixedUpdate()
    {
        if (isWalking)
        {
            float targetDistance = Vector2.Distance(transform.position, NPCTarget.transform.position);


            if (targetDistance >= 3f && !(targetDistance <= 1f))
            {
                //print("follow");
                GetComponent<PathFinding>().StartFinding
                    ((Vector2)transform.position, (Vector2)NPCTarget.transform.position);
            }
        }
        
    }


    



    //Trigger===================================================================================

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;

        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&&DialogPanel!=null)
        {
            playerInRange = false;
            
            DialogPanel.SetActive(false);
            
        }
    }


    //¥Î»≠
    void Conversation(int i) 
    {

        DialogPanel.SetActive(true);
        DialogTextMesh.text = ScriptManager.instance.NPCScript(i);

    }

    void FollowTarget() { }

    void NPCMove() { }


}
