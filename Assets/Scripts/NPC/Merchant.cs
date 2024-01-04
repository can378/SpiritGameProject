using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Merchant : MonoBehaviour
{
    public GameObject storePanel;

    private bool playerInRange = false;
        
    void Update()
    {
        // 플레이어가 충돌 영역에 있고 R키를 눌렀을 때
        if (playerInRange && storePanel != null && Input.GetKeyDown(KeyCode.R))
        {
            storePanel.SetActive(!storePanel.activeSelf);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //플레이어가 충돌 영역에 들어왔을 경우
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;


        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        //플레이어가 충돌 영역에서 나간 경우
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;

            // 패널이 존재하고 활성화되어있다면 비활성화
            if (storePanel != null && storePanel.activeSelf)
            {
                storePanel.SetActive(false);
            }
        }
    }
}
