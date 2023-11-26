using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet2 : MonoBehaviour
{
    public int dmg;
    public bool isRotate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isRotate)
            transform.Rotate(Vector3.forward * 10);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BorderBullet")
        {
            gameObject.SetActive(false);
        }
    }
}
