using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "Passive/AcidRain")]
public class AcidRain : PassiveData
{
    [SerializeField] GameObject acidRainPrefab;
    float coolTime = 0f;

    public override void Update_Passive(ObjectBasic _User)
    {
        if (_User == null)
        {
            return;
        }

        coolTime -= Time.deltaTime;

        if (coolTime <= 0f)
        {
            Destroy(Instantiate(acidRainPrefab, _User.transform.position, Quaternion.identity), 1f);
            coolTime = 0.1f;
        }
    }

    public override void Apply(ObjectBasic _User)
    {
        
    }

    // Update is called once per frame
    public override void Remove(ObjectBasic _User)
    {

    }
}
