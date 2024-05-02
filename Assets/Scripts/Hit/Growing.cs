using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Growing : MonoBehaviour
{
    [SerializeField] float ratio;
    float timer;
    
    void Update()
    {
        this.transform.localScale = new Vector3(1 + timer * ratio, 1 + timer * ratio, 1);
        timer += Time.deltaTime;
    }
}
