using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private UserData player;


   

    void Start()
    {   
        
        player = DataManager.instance.userData;
        player.coin += 50;


        ScriptManager.instance.ScriptTest();
    }

   


    void Update()
    {
        
    }
}
