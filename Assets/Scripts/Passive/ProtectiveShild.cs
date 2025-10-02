using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "Passive/ProtectiveShild")]
public class ProtectiveShild : PassiveData
{
    [SerializeField] float shildCoolTime = 0f;
    [SerializeField] GameObject shildPrefab;
    GameObject shild;

    public override void Update_Passive(ObjectBasic _User)
    {
        if (_User == null)
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
            GameObject shildGO = Instantiate(shildPrefab, _User.transform.position, Quaternion.identity);
            shildGO.transform.parent = _User.gameObject.transform;
            shild = shildGO;
        }
    }


    public override void Apply(ObjectBasic target)
    {

    }

    // Update is called once per frame
    public override void Remove(ObjectBasic target)
    {

    }
}
