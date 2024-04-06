using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossJigui : MonoBehaviour
{
    public GameObject fire;
    void Start()
    {
        StartCoroutine(jigui());
    }

    private void OnEnable()
    {
        //StartCoroutine(jigui());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }


    IEnumerator jigui()
    {
        //Throw fire balls
        for (int i = 0; i < 20; i++)
        {
            GameObject fireBall = ObjectPoolManager.instance.Get(4);
            fireBall.transform.position = transform.position;
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(3f);

        //Fire Area attack
        fire.SetActive(true);
        fire.GetComponent<SpriteRenderer>().color = new Color(67, 9, 9);
        yield return new WaitForSeconds(2f);
        fire.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
        yield return new WaitForSeconds(2f);
        fire.SetActive(false);
        //fire에 플레이어가 닿으면 불붙게하는 너프 적용!!!!!!!!!!!!!

        
        //플레이어에 틈이 보이면 돌진해서 공격??????????

        StartCoroutine(jigui());
    }


}
