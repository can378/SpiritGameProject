using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidRain : Equipment
{
    [SerializeField] GameObject acidRainPrefab;
    float coolTime = 0f;

    void Update()
    {
        Passive();
    }

    void Passive()
    {
        if (user == null)
        {
            return;
        }

        coolTime -= Time.deltaTime;

        if (coolTime <= 0f)
        {
            Shild();
        }
    }

    void Shild()
    {
        Destroy(Instantiate(acidRainPrefab, user.transform.position, Quaternion.identity),1f);
        coolTime = 0.1f;
    }

    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            this.user = target;
        }
    }

    // Update is called once per frame
    public override void UnEquip(Player target)
    {
        if (target.tag == "Player")
        {
            this.user = null;
        }
    }
}
