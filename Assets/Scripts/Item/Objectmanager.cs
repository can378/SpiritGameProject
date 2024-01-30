using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Objectmanager : MonoBehaviour
{
    //MerchantStore merchantStore;
    UserData userData;

    private void Start()
    {
        //merchantStore = FindObjectOfType<MerchantStore>();
        userData = FindObjectOfType<DataManager>().userData;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        
        
    }

    
}
