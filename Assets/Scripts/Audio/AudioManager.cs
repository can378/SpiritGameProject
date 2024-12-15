using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;
using System.Reflection;


public class AudioManager : MonoBehaviour
{
    public bool isPlayAudio;
    GameObject Canvas;

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


    [Header("BGM Audio Source")]
    //배경음 오디오
    public AudioClip[] BgClipList;
    public AudioClip[] ChapterBgm_normal;
    public AudioClip[] ChapterBgm_boss;

    [Header("SFX Audio Source")]
    public AudioClip testAudio;
    public AudioClip drop_key;
    public AudioClip hit;
    public AudioClip hit_Critical;
    public AudioClip footStepDirt;
    public AudioClip footStepStone;
    public AudioClip[] SFXClipList;
    public AudioClip healSfx;
    public AudioClip chestOpenSfx;
    public AudioClip fireWooschSfx;
    public AudioClip UIClickSfx;

   
    [field:SerializeField, Header("Player Weapon")]
    public AudioClip[] weaponAttack {get; private set; } = new AudioClip[(int)WEAPON_TYPE.NONE];

    //instance
    public static AudioManager instance;
   

    //========================================================================================================
    private void Awake()
    {
        instance = this;
        
        Initialize(10);//10개 미리 만들기
    }


    private void Start()
    {

        BGSoundSlider.GetComponent<Slider>().value = DataManager.instance.persistentData.BGSoundVolume;
        SFXSoundSlider.GetComponent<Slider>().value = DataManager.instance.persistentData.SFXSoundVolume;
        BGSoundVolume();
        SFXVolume();


        //start bgm
        Bgm_normal(DataManager.instance.userData.nowChapter);


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


        DataManager.instance.persistentData.BGSoundVolume = BGSoundSlider.GetComponent<Slider>().value;

    }

    public void SFXVolume()
    {
        //효과음 음량조절
        if (SFXSoundSlider.GetComponent<Slider>().value == 0) { mixer.SetFloat("SFX", -80); }
        else { mixer.SetFloat("SFX", Mathf.Log10(SFXSoundSlider.GetComponent<Slider>().value) * 20); }

        DataManager.instance.persistentData.SFXSoundVolume = SFXSoundSlider.GetComponent<Slider>().value;
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


    
    //효과음 플레이 함수====================================================================================
    public void TestAudioPlay()
    { SFXPlayPoolingVersion(testAudio); }

    public void WeaponAttackAudioPlay(WEAPON_TYPE weaponType) 
    { SFXPlayPoolingVersion(weaponAttack[(int)weaponType]); }
    public void KeyAudioPlay() 
    { SFXPlayPoolingVersion(drop_key); }
    public void HitAudioPlay() 
    { SFXPlayPoolingVersion(hit); }
    public void FootDirtAudioPlay() 
    { SFXPlayPoolingVersion(footStepDirt); }
    public void FootStoneAudioPlay() 
    { SFXPlayPoolingVersion(footStepStone); }

    public void HealAudioPlay() { SFXPlayPoolingVersion(healSfx); }
    public void chestOpenAudioPlay() { SFXPlayPoolingVersion(chestOpenSfx); }

    public void fireWooschAudio() { SFXPlayPoolingVersion(fireWooschSfx); }
    public void UIClickAudio() { SFXPlayPoolingVersion(UIClickSfx); }
    

    //배경음악 플레이 함수===============================================================================
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
    public void Bgm_normal(int chapterNum) 
    {
        

        AudioClip clip;
        clip = AudioManager.instance.ChapterBgm_normal[chapterNum];

        
        if (isPlayAudio)
        {
            
            bgSound.outputAudioMixerGroup = mixer.FindMatchingGroups("BG")[0];
            bgSound.clip = clip;
            bgSound.loop = true;
            bgSound.volume = 0.6f;
            bgSound.Play();

            //Debug.Log("bgm normal=" + chapterNum + "," + bgSound.clip.name);
        }

    }

    public void Bgm_boss(int chapterNum)
    {
        AudioClip clip;
        clip = AudioManager.instance.ChapterBgm_boss[chapterNum];

        if (isPlayAudio)
        {
            bgSound.outputAudioMixerGroup = mixer.FindMatchingGroups("BG")[0];
            bgSound.clip = clip;
            bgSound.loop = true;
            bgSound.volume = 0.6f;
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
    public void SFXPlayPoolingVersion(AudioClip clip)
    {
        var obj = GetObject();
        obj.gameObject.GetComponent<AudioSource>().clip = clip;
        obj.gameObject.GetComponent<AudioSource>().loop = false;
        obj.gameObject.GetComponent<AudioSource>().Play();
        obj.gameObject.GetComponent<AudioSource>().outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];

    }

    
    public void SFXPlay(string clipName) 
    { 
        AudioClip clip=null;

        //find Audio clip
        foreach (AudioClip c in SFXClipList)
        { if (c.name == clipName) { clip = c; break; } }

        if (clip == null) { Debug.LogWarning("cannot find SFX AudioClip"); return; }

        var obj = GetObject();
        obj.gameObject.GetComponent<AudioSource>().clip = clip;
        obj.gameObject.GetComponent<AudioSource>().loop = false;
        obj.gameObject.GetComponent<AudioSource>().Play();
        obj.gameObject.GetComponent<AudioSource>().outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];

    }

}