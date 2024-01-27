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
        GetComponent<PathFinding>().seeker = this.transform;
        scriptManager = GetComponent<ScriptManager>();
    }



    void Update()
    {
        sprite.sortingOrder = Mathf.RoundToInt(transform.position.y) * -1;
    }



    private void FixedUpdate()
    {
        //following player
        if (isWalking)
        {
            float targetDistance = Vector2.Distance(transform.position, NPCTarget.transform.position);
            if (targetDistance >= 3f && !(targetDistance <= 1f))
            {
                GetComponent<PathFinding>().StartFinding
                    ((Vector2)transform.position, (Vector2)NPCTarget.transform.position);
            }
        }
        
    }




    IEnumerator startConveration()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                DialogPanel.SetActive(!DialogPanel.activeSelf);
                Conversation(index);
                index++;
            }
            yield return null;
        }
    }

    //¥Î»≠
    void Conversation(int i)
    {
        DialogPanel.SetActive(true);

        if (scriptManager.NPCScript(chapter, i) == "border")
        {
            index--;
            DialogTextMesh.text = scriptManager.NPCScript(chapter, i - 1);
        }
        else if (scriptManager.NPCScript(chapter, i) == "wrong")
        {
            index++;
            DialogTextMesh.text = scriptManager.NPCScript(chapter, i + 1);
        }
        else
        {
            DialogTextMesh.text = scriptManager.NPCScript(chapter, i);
        }
    }



    //Trigger===================================================================================

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(startConveration());
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&&DialogPanel!=null)
        {
            StopAllCoroutines();
            DialogPanel.SetActive(false);
        }
    }



}
