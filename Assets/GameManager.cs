using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private UserData player;

    GameObject touchedObject;

   

    void Start()
    {   
        
        player = DataManager.instance.userData;
        player.coin += 50;


        ScriptManager.instance.ScriptTest();
    }

   


    void Update()
    {
        MouseClick();
    }

    void MouseClick() 
    {

        if (Input.GetMouseButton(0))
        {
            Vector2 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(clickPos, Camera.main.transform.forward);

            if (hit.collider != null)
            {
                touchedObject = hit.transform.gameObject;
                print(touchedObject.name);
            }
        }

    }
}
