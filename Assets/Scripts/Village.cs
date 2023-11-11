using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Village : MonoBehaviour
{
    public GameObject Inside;
    public GameObject Entrance;
    public GameObject Exit;
    public GameObject Cave;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player") 
        {
            
            if (this.gameObject == Exit) { Inside.SetActive(false); }
            else if(this.gameObject==Entrance) { Inside.SetActive(true); }

            if (this.gameObject == Cave) { SceneManager.LoadScene("Map"); }


        }
    }
}
