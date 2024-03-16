using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoHead_Body : MonoBehaviour
{
    private bool isTransform=false;

    void Start()
    { StartCoroutine(body()); }

    private void OnEnable()
    { StartCoroutine(body()); }

    private void OnDisable()
    { StopCoroutine(body()); }


    public IEnumerator body() 
    {
        //move randomly

        yield return new WaitForSeconds(0.01f);
    
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isTransform == false)
        {
            if (collision.name == "head")
            {
                //transform
                transform.parent = collision.gameObject.transform;
                isTransform = true;
            }
            else if (collision.tag == "Player")
            {
                //attack animation
            }
        }
        

    }
}
