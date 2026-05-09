using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{

    //viode option
    FullScreenMode screenMode;
    int vSyncCount;
    public Dropdown resolutionDropdown;
    public Toggle fullScreenBtn;
    public Toggle vSyncBtn;
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
        if (resolutionDropdown == null || fullScreenBtn == null)
        {
            Debug.LogError("UI references are not assigned in Inspector.");
            return;
        }

        resolutions.Clear();

        // 다양한 주사율을 가진 해상도가 있을 수 있으므로, 중복된 가로/세로 해상도는 하나만 리스트에 추가하도록 수정
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            Resolution res = Screen.resolutions[i];

            // 리스트에 이미 같은 가로/세로를 가진 해상도가 있는지 확인
            bool isDuplicate = false;
            foreach (var r in resolutions)
            {
                if (r.width == res.width && r.height == res.height)
                {
                    isDuplicate = true;
                    break;
                }
            }

            // 중복되지 않은 경우에만 추가 (보통 배열의 뒤쪽이 높은 주사율이므로 덮어씌워도 좋습니다)
            if (!isDuplicate)
            {
                resolutions.Add(res);
            }
        }

        resolutionDropdown.options.Clear();

        int optionNum = 0;
        foreach (Resolution item in resolutions)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = item.width + "x" + item.height;   // + " " + item.refreshRate + "hz";
            resolutionDropdown.options.Add(option);

            if (item.width == Screen.width && item.height == Screen.height)
            {
                resolutionDropdown.value = optionNum;
                resolutionNum = optionNum;
            }

            optionNum++;
        }

        resolutionDropdown.RefreshShownValue();

        fullScreenBtn.isOn = Screen.fullScreenMode == FullScreenMode.FullScreenWindow;
        screenMode = Screen.fullScreenMode;

        // 기본 수직 동기화 상태를 불러와서 적용합니다.
        vSyncCount = PlayerPrefs.GetInt("VSync", 1);
        if (vSyncCount == 1)
        {
            // 수직 동기화 On
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = -1; // 제한을 풀어야 모니터 주사율(144 등)까지 나옵니다.
            Debug.Log("수직 동기화 활성화: 모니터 주사율을 따릅니다.");
        }
        else
        {
            // 수직 동기화 Off
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60; // 개발자가 의도한 60프레임으로 제한합니다.
            Debug.Log("수직 동기화 비활성화: 60 FPS로 고정됩니다.");
        }
        vSyncBtn.isOn = QualitySettings.vSyncCount > 0;
    }

    public void DropboxOptionChange(int x)
    {
        resolutionNum = x;
    }



    public void FullScreenBtn(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    public void VSyncBtn(bool isvSync)
    {
        vSyncCount = isvSync ? 1 : 0;
    }

    public void OKBtnClick()
    {
        if (resolutions == null || resolutions.Count == 0)
        {
            Debug.LogError("Resolution list is empty.");
            return;
        }

        if (resolutionNum < 0 || resolutionNum >= resolutions.Count)
        {
            Debug.LogError("resolutionNum is out of range: " + resolutionNum);
            return;
        }

        Screen.SetResolution(
            resolutions[resolutionNum].width,
            resolutions[resolutionNum].height,
            screenMode
        );

        if (vSyncCount == 1)
        {
            // 수직 동기화 On
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = -1; // 제한을 풀어야 모니터 주사율(144 등)까지 나옵니다.
            Debug.Log("수직 동기화 활성화: 모니터 주사율을 따릅니다.");
        }
        else
        {
            // 수직 동기화 Off
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60; // 개발자가 의도한 60프레임으로 제한합니다.
            Debug.Log("수직 동기화 비활성화: 60 FPS로 고정됩니다.");
        }
        SaveVSync();
    }

    /// <summary>
    /// 현재 수직 동기화 상태를 적용합니다.
    /// </summary>
    void SaveVSync()
    {
        PlayerPrefs.SetInt("VSync", vSyncCount);
        PlayerPrefs.Save();
    }




    //sensitive======================================================================
    private void InitializeSensitivitySlider() //감도 초기화
    {
        if (sensitivitySlider == null)
        {
            Debug.LogError("Sensitivity slider is not assigned.");
            return;
        }

        sensitivitySlider.value = PlayerPrefs.GetFloat("MouseSensitivity", 0.5f);
    }

    public void OnSensitivityChange()//감도 변경
    {
        float sensitivityValue = sensitivitySlider.value;
        SaveSensitivity(sensitivityValue);
    }

    public void OnConfirmButtonClicked()//감도 조절 확인 버튼
    {
        ConfirmSensitivity();
    }

    public void OnResetButtonClicked()//설정 초기화 버튼
    {
        ResetSensitivity();
    }

    private void SaveSensitivity(float sensitivityValue)//마우스 민감도 저장
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
        sensitivitySlider.value = 0.5f;//슬라이더를 초기값으로 설정
        SaveSensitivity(0.5f);//초기값으로 마우스 감도 저장하는 함수 호출
    }
}
