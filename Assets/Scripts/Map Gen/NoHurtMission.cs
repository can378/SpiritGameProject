using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoHurtMission : MonoBehaviour
{
    public GameObject Enemies;
    public GameObject NPC;
    public GameObject NPCSprite;

    private bool isFirst;
    [HideInInspector]
    public bool noHurtMissionStart=false;

    void Start()
    {
        Enemies.SetActive(false);
        isFirst = true;
    }


    void Update()
    {
        if (NPC.GetComponent<NPCbasic>().isTalking&&isFirst)
        { 
            StartCoroutine(NPCDisapear()); 
            isFirst = false;
            print("start npc disapear");
        }
    }

    IEnumerator NPCDisapear()
    {
        float alpha = 1f;
        while (alpha > 0f)
        {
            NPCSprite.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f,alpha);
            alpha -= 0.1f;
            yield return new WaitForSeconds(0.05f);
        }
        NPC.SetActive(false);
        noHurtMissionStart = true;
        Enemies.SetActive(true);

        GetComponent<Mission>().enabled = true;
        GetComponent<Mission>().startMission();
    }
}
