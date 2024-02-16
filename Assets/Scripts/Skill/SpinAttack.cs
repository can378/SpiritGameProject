using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAttack : Skill
{
    [field: SerializeField] public int damage { get; private set; }
    [field: SerializeField] public GameObject spinEffect { get; private set; }

    public override void Use(GameObject user)
    {
        this.user = user;
        StartCoroutine("Attack");
    }

    IEnumerator Attack()
    {
        Debug.Log("SpinAttack");
        yield return new WaitForSeconds(0.1f);
        GameObject instant = Instantiate(spinEffect, user.transform.position,user.transform.rotation);
        Destroy(instant, 0.5f);
        
        yield return new WaitForSeconds(0.1f);

        StartCoroutine("CoolDown");

    }
}
