using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject tabkeyPanel;
    public GameObject esckeyPanel;
    public GameObject diePanel;

    private int currentHP;
    private int maxHP = 1000;

    private bool isPanelActive = false;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            AudioManager.instance.TestAudioPlay();
            tabkeyPanel.SetActive(!tabkeyPanel.activeSelf);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            AudioManager.instance.TestAudioPlay();
            esckeyPanel.SetActive(!tabkeyPanel.activeSelf);
        }
    }
    public void ContinueBtn()
    {
        esckeyPanel.SetActive(false);
    }
    public void QuitBtn() //�����ϰ� ������
    {
        PlayerPrefs.Save();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void HomeBtn() //Ȩ ��ư
    {
        PlayerPrefs.Save();
        SceneManager.LoadScene("Main");
    }
    public void RestartBtn() //����� ��ư
    {
        SceneManager.LoadScene("Map"); //���� �� �ٽ� �ε�
        currentHP = maxHP; //�÷��̾� ü�� ����

        tabkeyPanel.SetActive(false);
        esckeyPanel.SetActive(false);
        diePanel.SetActive(false);
    }
}
