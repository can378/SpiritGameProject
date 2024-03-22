using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tiger_tiger : EnemyBasic
{
    private bool isTransform=false;

    void Start()
    {
        StartCoroutine(tiger());
    }
    void OnEnable() { StartCoroutine(tiger()); }
    void OnDisable() { StopAllCoroutines(); }



    IEnumerator tiger() 
    { 
        yield return null;

        //hunt

        //hit

        //scream
        if (isTransform) 
        { }
        else 
        { }
 
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //blood debuff
        }
        if (collision.tag == "TigerHead" && isTransform == false)
        {
            //transform, stronger
            isTransform = true;
        }   
    }
}
