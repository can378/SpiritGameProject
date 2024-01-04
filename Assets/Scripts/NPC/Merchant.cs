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
        // �÷��̾ �浹 ������ �ְ� RŰ�� ������ ��
        if (playerInRange && storePanel != null && Input.GetKeyDown(KeyCode.R))
        {
            storePanel.SetActive(!storePanel.activeSelf);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //�÷��̾ �浹 ������ ������ ���
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;


        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        //�÷��̾ �浹 �������� ���� ���
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;

            // �г��� �����ϰ� Ȱ��ȭ�Ǿ��ִٸ� ��Ȱ��ȭ
            if (storePanel != null && storePanel.activeSelf)
            {
                storePanel.SetActive(false);
            }
        }
    }
}
