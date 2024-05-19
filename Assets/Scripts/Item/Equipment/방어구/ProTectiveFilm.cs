using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectiveFilm : Equipment
{
    [SerializeField] float shildCoolTime = 100f;
    [SerializeField] GameObject shildPrefab;
    GameObject shild;

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

        if (shild != null)
        {
            shildCoolTime = 100f;
            return;
        }

        shildCoolTime -= Time.deltaTime;

        if (shildCoolTime <= 0f && shild == null)
        {
            Shild();
        }
    }

    void Shild()
    {
        GameObject shildGO = Instantiate(shildPrefab, user.transform.position, Quaternion.identity);
        shildGO.transform.parent = user.gameObject.transform;
        shild = shildGO;
    }

    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            this.user = target;
            Debug.Log("10�� ���� ���� ���� �ƴϸ� ����");
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
