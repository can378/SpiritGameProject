using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcicleExplosion : HitDetection
{

    private bool isExploding=false;
    public GameObject originalForm;
    public GameObject explosinForm;


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy"||
            collision.gameObject.tag == "Wall" || 
            collision.gameObject.tag == "Door" || 
            collision.gameObject.tag == "ShabbyWall" &&isExploding==false)
        {

            explosionSprite();
        }

        //base.OnTriggerEnter2D(collision);
        
       
    }

    public void explosionSprite() 
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        originalForm.SetActive(false);
        //explosinForm.SetActive(true);
        GameObject sfx=ObjectPoolManager.instance.ExplosionSFX(explosinForm.GetComponent<SpriteRenderer>().sprite);
        sfx.transform.position = this.transform.position;
    }


    

}
