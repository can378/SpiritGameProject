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
    //�Ҹ� ���� �����̴�
    public GameObject BGSoundSlider;
    public GameObject SFXSoundSlider;

    [Header("Sound")]
    public AudioMixer mixer;//�����? �ͼ�
    public AudioSource bgSound;//�����? �Ŵ���

    [Header("Pooling")]
    [SerializeField]
    private GameObject soundPrefab; //�̸� ������ ������
    Queue<Sound> poolingObjectQueue = new Queue<Sound>(); //ť ����
    public GameObject AudioManagerObj;


    [Header("BGM Audio Source")]
    //�����? �����?
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
    public AudioClip chestOpenSfx;
    public AudioClip UIClickSfx;
    public AudioClip AltarBlessing;
    public AudioClip FireTimer;
    public AudioClip Eat;
    public AudioClip FitItem;
    //public AudioClip fireWooschSfx;
    //public AudioClip healSfx;

    //instance
    public static AudioManager instance;


    //========================================================================================================
    private void Awake()
    {
        instance = this;

        Initialize(10);//10�� �̸� �����?
    }


    private void Start()
    {

        BGSoundSlider.GetComponent<Slider>().value = DataManager.instance.persistentData.BGSoundVolume;
        SFXSoundSlider.GetComponent<Slider>().value = DataManager.instance.persistentData.SFXSoundVolume;
        BGSoundVolume();
        SFXVolume();


        //start bgm
        Bgm_normal(DataManager.instance.userData.nowChapter);


        //�����̵尪 ���Ҷ����� �Ʒ� �Լ� ����
        BGSoundSlider.GetComponent<Slider>().onValueChanged.AddListener(delegate { BGSoundVolume(); });
        SFXSoundSlider.GetComponent<Slider>().onValueChanged.AddListener(delegate { SFXVolume(); });
    }


    //�����? ���� ����========================================================================================================

    public void BGSoundVolume()
    {

        //�����? ��������
        if (BGSoundSlider.GetComponent<Slider>().value == 0)
        { mixer.SetFloat("BG", -80); }
        else
        { mixer.SetFloat("BG", Mathf.Log10(BGSoundSlider.GetComponent<Slider>().value) * 20); }


        DataManager.instance.persistentData.BGSoundVolume = BGSoundSlider.GetComponent<Slider>().value;

    }

    public void SFXVolume()
    {
        //ȿ���� ��������
        if (SFXSoundSlider.GetComponent<Slider>().value == 0) { mixer.SetFloat("SFX", -80); }
        else { mixer.SetFloat("SFX", Mathf.Log10(SFXSoundSlider.GetComponent<Slider>().value) * 20); }

        DataManager.instance.persistentData.SFXSoundVolume = SFXSoundSlider.GetComponent<Slider>().value;
    }





    //========================================================================================================

    //�����? ����
    public void PauseAudio(string clipName)
    {
        GameObject player = GameObject.Find(clipName);
        if (player != null)
        {
            player.GetComponent<AudioSource>().Pause();
        }
    }

    //�����? �簳
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

    
    private void PlayBGM(AudioClip clip, float volume)
    {
        if (!isPlayAudio || clip == null) return;

        bgSound.outputAudioMixerGroup = mixer.FindMatchingGroups("BG")[0];
        bgSound.clip = clip;
        bgSound.loop = true;
        bgSound.volume = volume;
        bgSound.Play();
    }
    
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        var obj = GetObject();
        var source = obj.gameObject.GetComponent<AudioSource>();
        source.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        source.clip = clip;
        source.loop = false;
        source.Play();
    }


    //ȿ���� �÷��� �Լ�====================================================================================
    public void TestAudioPlay()
    { SFXPlayPoolingVersion(testAudio); }

    public void KeyAudioPlay()
    { SFXPlayPoolingVersion(drop_key); }
    public void HitAudioPlay()
    { SFXPlayPoolingVersion(hit); }
    public void FootDirtAudioPlay()
    { SFXPlayPoolingVersion(footStepDirt); }
    public void FootStoneAudioPlay()
    { SFXPlayPoolingVersion(footStepStone); }

    //public void HealAudioPlay() { SFXPlayPoolingVersion(healSfx); }
    public void chestOpenAudioPlay() { SFXPlayPoolingVersion(chestOpenSfx); }

    //public void fireWooschAudio() { SFXPlayPoolingVersion(fireWooschSfx); }
    public void UIClickAudio() { SFXPlayPoolingVersion(UIClickSfx); }
    public void AltarBlessAudioPlay() { SFXPlayPoolingVersion(AltarBlessing); }
    public void FireTimerAudioPlay() { SFXPlayPoolingVersion(FireTimer); }
    public void EatAudioPlay() { SFXPlayPoolingVersion(Eat); }
    public void FitItemAudioPlay()  { SFXPlayPoolingVersion(FitItem); }


    //�������? �÷��� �Լ�===============================================================================
    public void BGMPlay(int index)
    {
        PlayBGM(BgClipList[index], 0.2f);
    }

    public void BGMPlay(AudioClip clip)
    {
        if (clip == null) { Debug.LogWarning("no BGM Audio clip"); return; }
        PlayBGM(clip, 0.2f);
    }
    public void Bgm_normal(int chapterNum)
    {
        

        AudioClip clip;
        clip = AudioManager.instance.ChapterBgm_normal[chapterNum];


        if (isPlayAudio)
        {
            PlayBGM(clip, 0.6f);

            //Debug.Log("bgm normal=" + chapterNum + "," + bgSound.clip.name);
        }

    }

    public void Bgm_boss(int chapterNum)
    {
        AudioClip clip;
        clip = AudioManager.instance.ChapterBgm_boss[chapterNum];

        if (isPlayAudio)
        {
            PlayBGM(clip, 0.6f);
        }
    }
    //������Ʈ Ǯ��========================================================================================================


    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            poolingObjectQueue.Enqueue(CreateNewObject()); //10�� Enqueue
        }
    }


    private Sound CreateNewObject()
    {
        var newObj = Instantiate(soundPrefab).GetComponent<Sound>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(AudioManagerObj.transform);
        return newObj; //�׸��� Queue�� �ְ� ��ȯ
    }

    public static Sound GetObject() // �̸� �����? �����ٰ� ����!
    {
        if (instance.poolingObjectQueue.Count > 0) // �̸� �����Ȱ� �Ⱥ����ϸ�
        {
            var obj = instance.poolingObjectQueue.Dequeue(); // Dequeue
            obj.gameObject.SetActive(true); // �̸� �����Ǿ��ִ°� ON
            return obj;
        }
        else // �����ϸ�
        {
            var newObj = instance.CreateNewObject(); // �ϳ� ���� �����?
            newObj.gameObject.SetActive(true); // �ؿ��� ���� ����
            return newObj;
        }
    }

    public static void ReturnObject(Sound obj) //���? �� �ٽ� ��ȯ
    {
        obj.gameObject.SetActive(false); //����
        instance.poolingObjectQueue.Enqueue(obj); // �ٽ� Enqueue ����
    }
    public void SFXPlayPoolingVersion(AudioClip clip)
    {
        PlaySFX(clip);
    }


    public void SFXPlay(string clipName)
    {
        AudioClip clip = null;

        //find Audio clip
        foreach (AudioClip c in SFXClipList)
        {
            if (c.name == clipName)
            {
                clip = c;
                break;
            }
        }

        if (clip == null) { Debug.LogWarning("cannot find SFX AudioClip"); return; }

        PlaySFX(clip);
    }

    public void SFXPlay(AudioClip clip)
    {
        if (clip == null) { Debug.LogWarning("no SFX Audio clip"); return; }

        PlaySFX(clip);
    }



    // �̼Ǹ� BGM �÷��� �Լ� ==========================================================
    private AudioClip currentChapterBgm;
    private bool isMissionBgmPlaying = false;

    public List<string> ClockBgmMaps = new List<string>();
    public List<string> HeartbpmBgmMaps = new List<string>();

    public void OnEnterMap(GameObject mapObj)
    {
        if (!isPlayAudio) return;

        string mapName = mapObj.name.Replace("(Clone)", "").Trim();

        if (ClockBgmMaps.Contains(mapName))
        {
            currentChapterBgm = bgSound.clip;
            PlayBGM(BgClipList[8], 0.6f);
            isMissionBgmPlaying = true;
        }

        else if (HeartbpmBgmMaps.Contains(mapName))
        {
            if (bgSound.isPlaying) bgSound.Stop();
            currentChapterBgm = bgSound.clip;
            PlayBGM(BgClipList[9], 0.6f);
            isMissionBgmPlaying = true;
        }
    }

    public void OnExitMap(GameObject mapObj)
    {
        if (!isMissionBgmPlaying) return;

        string mapName = mapObj.name.Replace("(Clone)", "").Trim();

        if (ClockBgmMaps.Contains(mapName) || HeartbpmBgmMaps.Contains(mapName))
        {
            if (bgSound.isPlaying)
                bgSound.Stop();

            if (currentChapterBgm != null)
            {
                PlayBGM(currentChapterBgm, 0.6f);
            }

            isMissionBgmPlaying = false;
        }
    }
}