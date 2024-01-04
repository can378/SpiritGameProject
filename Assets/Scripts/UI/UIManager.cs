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

    public Slider Hpslider;
    public float maxHpValue = 100f;

    void Start()
    {
        UpdateHealthUI();
    }

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

    public void UpdateHealthUI()
    {
        // DataManager�� �ʱ�ȭ���� �ʾҰų� instance�� null�̸� �� �̻� �������� ����
        if (DataManager.instance == null || DataManager.instance.userData == null)
        {
            return;
        }

        float normalizedHealth = DataManager.instance.userData.playerHealth / maxHpValue;
        Debug.Log("Player Health: " + DataManager.instance.userData.playerHealth);
        Debug.Log("Normalized Health: " + normalizedHealth);
        Hpslider.value = normalizedHealth;
        //Hpscrollbar.GetComponentInChildren<Text>().text = "Health: " + DataManager.instance.userData.playerHealth; //�ϴ� ���� �ؽ�Ʈ ����
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

        tabkeyPanel.SetActive(false);
        esckeyPanel.SetActive(false);
        diePanel.SetActive(false);
    }
}
