using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeBox : EnemyBasic
{
    public GameObject mouth;
    void Start()
    {
        StartCoroutine(fakeBox());
    }

    private void OnEnable()
    {
        StartCoroutine(fakeBox());
    }

    private void OnDisable()
    {
        StopCoroutine(fakeBox());
    }
    IEnumerator fakeBox() 
    {
        targetDis = Vector2.Distance(transform.position, enemyTarget.position);
        print("fakeBox1");
        
        if (targetDis < 3f && Input.GetKeyDown(KeyCode.F))
        {
            print("fakeBox2");
            mouth.SetActive(true);
            yield return new WaitForSeconds(5f);
            mouth.SetActive(false);
        }
        yield return new WaitForSeconds(0.01f);

        StartCoroutine(fakeBox());
        
    }
}
