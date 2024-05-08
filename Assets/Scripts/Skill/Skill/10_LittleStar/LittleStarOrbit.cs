using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleStarOrbit : MonoBehaviour
{
    [field: SerializeField] public ObjectBasic user;
    [field: SerializeField] GameObject[] littleStars = new GameObject[3];
    [field: SerializeField] float angular;

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
        this.transform.position = user.transform.position;
    }

    // Å¸°Ù ´ë±âÁß
    void WaitTarget()
    {
        if (user == null)
            return;
        
        if (user.hitTarget == null)
            return;

        OnTarget(user.hitTarget.transform);
            
    }

    // Å¸°Ù °¨Áö
    void OnTarget(Transform target)
    {
        print(target.name);

        foreach (GameObject littleStar in littleStars)
        {
            HitDetection hitDetection = littleStar.GetComponent<HitDetection>();   
            Guiding guiding = littleStar.GetComponent<Guiding>();

            hitDetection.SetHitDetection(true,0,false,-1,10,10,0,0,null);
            guiding.guidingTarget = target;

            littleStar.transform.parent = null;
        }

        Destroy(this.gameObject);
    }
}
