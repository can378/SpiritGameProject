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
    //�Ҹ� ���� �����̴�
    public GameObject BGSoundSlider;
    public GameObject SFXSoundSlider;

    [Header("Sound")]
    public AudioMixer mixer;//����� �ͼ�
    public AudioSource bgSound;//����� �Ŵ���

    [Header("Pooling")]
    [SerializeField]
    private GameObject soundPrefab; //�̸� ������ ������
    Queue<Sound> poolingObjectQueue = new Queue<Sound>(); //ť ����
    public GameObject AudioManagerObj;



    //����� �����
    public AudioClip[] BgClipList;

    //ȿ���� �����
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


        Initialize(10);//10�� �̸� �����
    }


    private void Start()
    {
        BGSoundSlider.GetComponent<Slider>().value = DataManager.instance.userData.BGSoundVolume;
        SFXSoundSlider.GetComponent<Slider>().value = DataManager.instance.userData.SFXSoundVolume;
        BGSoundVolume();
        SFXVolume();

        
        AudioManager.instance.BGMPlay(0);


        //�����̵尪 ���Ҷ����� �Ʒ� �Լ� ����
        BGSoundSlider.GetComponent<Slider>().onValueChanged.AddListener(delegate { BGSoundVolume(); });
        SFXSoundSlider.GetComponent<Slider>().onValueChanged.AddListener(delegate { SFXVolume(); });
    }


    //����� ���� ����========================================================================================================

    public void BGSoundVolume()
    {

        //����� ��������
        if (BGSoundSlider.GetComponent<Slider>().value == 0) 
        { mixer.SetFloat("BG", -80); }
        else 
        { mixer.SetFloat("BG", Mathf.Log10(BGSoundSlider.GetComponent<Slider>().value) * 20); }


        DataManager.instance.userData.BGSoundVolume = BGSoundSlider.GetComponent<Slider>().value;

    }

    public void SFXVolume()
    {
        //ȿ���� ��������
        if (SFXSoundSlider.GetComponent<Slider>().value == 0) { mixer.SetFloat("SFX", -80); }
        else { mixer.SetFloat("SFX", Mathf.Log10(SFXSoundSlider.GetComponent<Slider>().value) * 20); }

        DataManager.instance.userData.SFXSoundVolume = SFXSoundSlider.GetComponent<Slider>().value;
    }





    //========================================================================================================
    
    //����� ����
    public void PauseAudio(string clipName)
    {
        GameObject player = GameObject.Find(clipName);
        if (player != null)
        {
            player.GetComponent<AudioSource>().Pause();
        }
    }

    //����� �簳
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



    //ȿ���� �÷��� �Լ�
    public void TestAudioPlay()
    { SFXPlayPoolingVersion("testAudio", testAudio); }
    
   

    //������� �÷��� �Լ�
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

    public static Sound GetObject() // �̸� ����� �����ٰ� ����!
    {
        if (instance.poolingObjectQueue.Count > 0) // �̸� �����Ȱ� �Ⱥ����ϸ�
        {
            var obj = instance.poolingObjectQueue.Dequeue(); // Dequeue
            obj.gameObject.SetActive(true); // �̸� �����Ǿ��ִ°� ON
            return obj;
        }
        else // �����ϸ�
        {
            var newObj = instance.CreateNewObject(); // �ϳ� ���� ����
            newObj.gameObject.SetActive(true); // �ؿ��� ���� ����
            return newObj;
        }
    }

    public static void ReturnObject(Sound obj) //��� �� �ٽ� ��ȯ
    {
        obj.gameObject.SetActive(false); //����
        instance.poolingObjectQueue.Enqueue(obj); // �ٽ� Enqueue ����
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