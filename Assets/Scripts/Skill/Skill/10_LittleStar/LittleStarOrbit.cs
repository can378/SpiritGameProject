using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleStarOrbit : MonoBehaviour
{
    [field: SerializeField] public ObjectBasic user {get; set;}
    [field: SerializeField] public GameObject[] littleStars = new GameObject[3];
    [field: SerializeField] float angular;

    [field: SerializeField] float StarSpeed;
    [field: SerializeField] float StarAngularSpeed;

    [field: SerializeField] public float DefaultDamage;
    [field: SerializeField] public float Ratio;


    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rigid.angularVelocity = angular;
    }

    // Update is called once per frame
    void Update()
    {
        WaitTarget();
        this.transform.position = new Vector3(user.CenterPivot.transform.position.x, user.CenterPivot.transform.position.y,0);
        if(user.gameObject.activeSelf == false)
            Destroy(this.gameObject);
    }

    // Å¸°Ù ´ë±âÁß
    void WaitTarget()
    {
        if (user == null)
            return;
        
        if (user.status.hitTarget == null)
            return;

        OnTarget(user.status.hitTarget.transform);
    }

    // Å¸°Ù °¨Áö
    void OnTarget(Transform target)
    {
        print(target.name);

        foreach (GameObject littleStar in littleStars)
        {
            HitDetection hitDetection = littleStar.GetComponent<HitDetection>();

            hitDetection.SetGuiding(true, target.GetComponent<ObjectBasic>().CenterPivot, StarSpeed, StarAngularSpeed,false);
            hitDetection.SetHit_Ratio(DefaultDamage * 2, Ratio * 2, user.stats.SkillPower, hitDetection.knockBack);
            hitDetection.SetDisableTime(5, ENABLE_TYPE.Projectile);

            littleStar.transform.parent = null;
            Destroy(littleStar.gameObject, 10f);
        }

        Destroy(this.gameObject);
    }
}
