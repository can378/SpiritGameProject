using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageArea : HitDetection
{
    public SafeArea safeArea;
    public CircleCollider2D circleCollider;

    private void Start()
    {
        circleCollider=GetComponent<CircleCollider2D>();
    }
    /*
    protected override void Update()
    {
        if (safeArea.isPlayerSafe)
        { circleCollider.enabled = false; }
        else 
        { circleCollider.enabled = true; base.Update(); }
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (safeArea.isPlayerSafe) { print("safe"); return; }
        print("not safe");
        base.OnTriggerEnter2D(collision);
    }
    */
    
}
