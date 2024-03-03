using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWorm : MonoBehaviour
{
    public GameObject wormAttack;


    IEnumerator visionObstruct() 
    {
        GameObject visionObst = Instantiate(wormAttack);
        visionObst.transform.parent = GameObject.FindWithTag("Canvas").transform;
        visionObst.transform.Rotate(new Vector3(0, 0, Random.Range(0, 360)));
        //visionObst.transform.position = new Vector3(0, 0, 0);
        print(visionObst.transform.position);
        yield return new WaitForSeconds(15f);
        Destroy(visionObst);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StartCoroutine(visionObstruct());
        }
    }
}
