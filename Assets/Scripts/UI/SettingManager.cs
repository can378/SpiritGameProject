using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{

    //viode option
    FullScreenMode screenMode;
    public Dropdown resolutionDropdown;
    public Toggle fullScreenBtn;
    List<Resolution> resolutions = new List<Resolution>();
    public int resolutionNum;


    //sensitivity
    public Slider sensitivitySlider;


    
    void Start()
    {
        initVideoOption();
        InitializeSensitivitySlider();
    }




//Vide Option======================================================================
    void initVideoOption() 
    {


        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].refreshRate == 60)//�������60�ΰ͸� �����
            { resolutions.Add(Screen.resolutions[i]); }
        }

        resolutionDropdown.options.Clear();

        int optionNum = 0;
        foreach (Resolution item in resolutions) 
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = item.width + "x" + item.height + " " + item.refreshRate + "hz";
            resolutionDropdown.options.Add(option);

            if (item.width == Screen.width && item.height == Screen.height)
                resolutionDropdown.value = optionNum;
            optionNum++;
        }
        resolutionDropdown.RefreshShownValue();
        fullScreenBtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    
    }

    public void DropboxOptionChange(int x) 
    {
        resolutionNum = x;
    }



    public void FullScreenBtn(bool isFull) 
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    public void OKBtnClick() 
    {
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, screenMode);
    }






    //sensitive======================================================================
    private void InitializeSensitivitySlider() //���� �ʱ�ȭ
    {
        sensitivitySlider.value = PlayerPrefs.GetFloat("MouseSensitivity", 0.5f);
    }

    public void OnSensitivityChange() //���� ����
    {
        float sensitivityValue = sensitivitySlider.value;
        SaveSensitivity(sensitivityValue);
    }

    public void OnConfirmButtonClicked() //���� ���� Ȯ�� ��ư
    {
        ConfirmSensitivity();
    }

    public void OnResetButtonClicked() //���� �ʱ�ȭ ��ư
    {
        ResetSensitivity();
    }

    private void SaveSensitivity(float sensitivityValue) //���콺 �ΰ��� ����
    {
        PlayerPrefs.SetFloat("MouseSensitivity", sensitivityValue); 
        PlayerPrefs.Save();
    }

    private void ConfirmSensitivity()
    {
        Debug.Log("Sensitivity Confirmed!");
    }

    private void ResetSensitivity()
    {
        sensitivitySlider.value = 0.5f; //�����̴��� �ʱⰪ���� ����
        SaveSensitivity(0.5f); //�ʱⰪ���� ���콺 ���� �����ϴ� �Լ� ȣ��
    }
}
