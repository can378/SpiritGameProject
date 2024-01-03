using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public class AudioManager : MonoBehaviour
{
    public bool isPlayAudio;

    [Header("object")]
    //소리 조절 슬라이더
    public GameObject BGSoundSlider;
    public GameObject SFXSoundSlider;

    [Header("Sound")]
    public AudioMixer mixer;//오디오 믹서
    public AudioSource bgSound;//오디오 매니저

    [Header("Pooling")]
    [SerializeField]
    private GameObject soundPrefab; //미리 생성될 프리팹
    Queue<Sound> poolingObjectQueue = new Queue<Sound>(); //큐 생성
    public GameObject AudioManagerObj;



    //배경음 오디오
    public AudioClip[] BgClipList;

    //효과음 오디오
    public AudioClip testAudio;
    

    //instance
    public static AudioManager instance;


    //========================================================================================================
    private void Awake()
    {

        //instance
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else { Destroy(gameObject); }


        Initialize(10);//10개 미리 만들기
    }


    private void Start()
    {
        BGSoundSlider.GetComponent<Slider>().value = DataManager.instance.userData.BGSoundVolume;
        SFXSoundSlider.GetComponent<Slider>().value = DataManager.instance.userData.SFXSoundVolume;
        BGSoundVolume();
        SFXVolume();

        
        AudioManager.instance.BGMPlay(0);


        //슬라이드값 변할때마다 아래 함수 실행
        BGSoundSlider.GetComponent<Slider>().onValueChanged.AddListener(delegate { BGSoundVolume(); });
        SFXSoundSlider.GetComponent<Slider>().onValueChanged.AddListener(delegate { SFXVolume(); });
    }


    //오디오 음량 조절========================================================================================================

    public void BGSoundVolume()
    {

        //배경음 음량조절
        if (BGSoundSlider.GetComponent<Slider>().value == 0) 
        { mixer.SetFloat("BG", -80); }
        else 
        { mixer.SetFloat("BG", Mathf.Log10(BGSoundSlider.GetComponent<Slider>().value) * 20); }


        DataManager.instance.userData.BGSoundVolume = BGSoundSlider.GetComponent<Slider>().value;

    }

    public void SFXVolume()
    {
        //효과음 음량조절
        if (SFXSoundSlider.GetComponent<Slider>().value == 0) { mixer.SetFloat("SFX", -80); }
        else { mixer.SetFloat("SFX", Mathf.Log10(SFXSoundSlider.GetComponent<Slider>().value) * 20); }

        DataManager.instance.userData.SFXSoundVolume = SFXSoundSlider.GetComponent<Slider>().value;
    }





    //========================================================================================================
    
    //오디오 멈춤
    public void PauseAudio(string clipName)
    {
        GameObject player = GameObject.Find(clipName);
        if (player != null)
        {
            player.GetComponent<AudioSource>().Pause();
        }
    }

    //오디오 재개
    public void ResumePlayAudio(string clipName)
    {
        GameObject player = GameObject.Find(clipName);
        if (player != null)
        {
            //Debug.Log($"Resume {clipName}");
            player.GetComponent<AudioSource>().Play();
        }
        //else Debug.Log($"player {clipName} is null!");
    }



    //효과음 플레이 함수
    public void TestAudioPlay()
    { SFXPlayPoolingVersion("testAudio", testAudio); }
    
   

    //배경음악 플레이 함수
    public void BGMPlay(int index)
    {

        AudioClip clip;
        clip = AudioManager.instance.BgClipList[index];

        if (isPlayAudio == true)
        {
            bgSound.outputAudioMixerGroup = mixer.FindMatchingGroups("BG")[0];
            bgSound.clip = clip;
            bgSound.loop = true;
            bgSound.volume = 0.2f;
            bgSound.Play();
        }
    }


    //오브젝트 풀링========================================================================================================


    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            poolingObjectQueue.Enqueue(CreateNewObject()); //10번 Enqueue
        }
    }


    private Sound CreateNewObject()
    {
        var newObj = Instantiate(soundPrefab).GetComponent<Sound>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(AudioManagerObj.transform);
        return newObj; //그리고 Queue에 넣게 반환
    }

    public static Sound GetObject() // 미리 만든거 가져다가 쓴다!
    {
        if (instance.poolingObjectQueue.Count > 0) // 미리 생성된게 안부족하면
        {
            var obj = instance.poolingObjectQueue.Dequeue(); // Dequeue
            obj.gameObject.SetActive(true); // 미리 생성되어있는거 ON
            return obj;
        }
        else // 부족하면
        {
            var newObj = instance.CreateNewObject(); // 하나 새로 만들어서
            newObj.gameObject.SetActive(true); // 밑에는 위와 같음
            return newObj;
        }
    }

    public static void ReturnObject(Sound obj) //썼던 거 다시 반환
    {
        obj.gameObject.SetActive(false); //끄고
        instance.poolingObjectQueue.Enqueue(obj); // 다시 Enqueue 삽입
    }
    public void SFXPlayPoolingVersion(string sfxName, AudioClip clip)
    {
        var obj = GetObject();
        obj.gameObject.GetComponent<AudioSource>().clip = clip;
        obj.gameObject.GetComponent<AudioSource>().loop = false;
        obj.gameObject.GetComponent<AudioSource>().Play();
        obj.gameObject.GetComponent<AudioSource>().outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];

    }


}