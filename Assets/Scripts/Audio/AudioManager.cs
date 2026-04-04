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
	//ï¿½Ò¸ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½Ì´ï¿½
	public GameObject BGSoundSlider;
	public GameObject SFXSoundSlider;

	[Header("Sound")]
	public AudioMixer mixer;//ï¿½ï¿½ï¿½ï¿½ï¿? ï¿½Í¼ï¿½
	public AudioSource bgSound;//ï¿½ï¿½ï¿½ï¿½ï¿? ï¿½Å´ï¿½ï¿½ï¿½

	[Header("Pooling")]
	[SerializeField]
	private GameObject soundPrefab; //ï¿½Ì¸ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
	Queue<Sound> poolingObjectQueue = new Queue<Sound>(); //Å¥ ï¿½ï¿½ï¿½ï¿½
	public GameObject AudioManagerObj;


	[Header("BGM Audio Source")]
	//ï¿½ï¿½ï¿½ï¿½ï¿? ï¿½ï¿½ï¿½ï¿½ï¿?
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

		Initialize(10);//10ï¿½ï¿½ ï¿½Ì¸ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿?
	}


	private void Start()
	{

		BGSoundSlider.GetComponent<Slider>().value = DataManager.instance.persistentData.BGSoundVolume;
		SFXSoundSlider.GetComponent<Slider>().value = DataManager.instance.persistentData.SFXSoundVolume;
		BGSoundVolume();
		SFXVolume();


		//start bgm
		Bgm_normal(DataManager.instance.userData.nowChapter);


		//ï¿½ï¿½ï¿½ï¿½ï¿½Ìµå°ª ï¿½ï¿½ï¿½Ò¶ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½Æ·ï¿½ ï¿½Ô¼ï¿½ ï¿½ï¿½ï¿½ï¿½
		BGSoundSlider.GetComponent<Slider>().onValueChanged.AddListener(delegate { BGSoundVolume(); });
		SFXSoundSlider.GetComponent<Slider>().onValueChanged.AddListener(delegate { SFXVolume(); });
	}


	//ï¿½ï¿½ï¿½ï¿½ï¿? ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½========================================================================================================

	public void BGSoundVolume()
	{

		//ï¿½ï¿½ï¿½ï¿½ï¿? ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
		if (BGSoundSlider.GetComponent<Slider>().value == 0)
		{ mixer.SetFloat("BG", -80); }
		else
		{ mixer.SetFloat("BG", Mathf.Log10(BGSoundSlider.GetComponent<Slider>().value) * 20); }


		DataManager.instance.persistentData.BGSoundVolume = BGSoundSlider.GetComponent<Slider>().value;

	}

	public void SFXVolume()
	{
		//È¿ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
		if (SFXSoundSlider.GetComponent<Slider>().value == 0) { mixer.SetFloat("SFX", -80); }
		else { mixer.SetFloat("SFX", Mathf.Log10(SFXSoundSlider.GetComponent<Slider>().value) * 20); }

		DataManager.instance.persistentData.SFXSoundVolume = SFXSoundSlider.GetComponent<Slider>().value;
	}





	//========================================================================================================

	//ï¿½ï¿½ï¿½ï¿½ï¿? ï¿½ï¿½ï¿½ï¿½
	public void PauseAudio(string clipName)
	{
		GameObject player = GameObject.Find(clipName);
		if (player != null)
		{
			player.GetComponent<AudioSource>().Pause();
		}
	}

	//ï¿½ï¿½ï¿½ï¿½ï¿? ï¿½ç°³
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
	
	public SoundInstance PlaySFX(AudioClip clip, bool IsLoop = false)
	{
		if (clip == null) return default;

		var obj = GetObject();
		var source = obj.gameObject.GetComponent<AudioSource>();
		source.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
		source.clip = clip;
		source.loop = IsLoop;
		source.Play();

		return new SoundInstance(obj);
    }


	//È¿ï¿½ï¿½ï¿½ï¿½ ï¿½Ã·ï¿½ï¿½ï¿½ ï¿½Ô¼ï¿½====================================================================================
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


	//ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿? ï¿½Ã·ï¿½ï¿½ï¿½ ï¿½Ô¼ï¿½===============================================================================
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
	//ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Æ® Ç®ï¿½ï¿½========================================================================================================


	private void Initialize(int initCount)
	{
		for (int i = 0; i < initCount; i++)
		{
			poolingObjectQueue.Enqueue(CreateNewObject()); //10ï¿½ï¿½ Enqueue
		}
	}


	private Sound CreateNewObject()
	{
		var newObj = Instantiate(soundPrefab).GetComponent<Sound>();
		newObj.gameObject.SetActive(false);
		newObj.transform.SetParent(AudioManagerObj.transform);
		return newObj; //ï¿½×¸ï¿½ï¿½ï¿½ Queueï¿½ï¿½ ï¿½Ö°ï¿½ ï¿½ï¿½È¯
	}

	public static Sound GetObject() // ï¿½Ì¸ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿? ï¿½ï¿½ï¿½ï¿½ï¿½Ù°ï¿½ ï¿½ï¿½ï¿½ï¿½!
	{
		if (instance.poolingObjectQueue.Count > 0) // Ç®¸µµÈ ¿ÀºêÁ§Æ®°¡ ÃæºÐÇÏ¸é
		{
			var obj = instance.poolingObjectQueue.Dequeue(); // Dequeue
			obj.gameObject.SetActive(true); // ï¿½Ì¸ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½Ç¾ï¿½ï¿½Ö´Â°ï¿½ ON
			obj.UpdateSessionID();
            return obj;
		}
		else // Ç®¸µµÈ ¿ÀºêÁ§Æ®°¡ ºÎÁ·ÇÏ¸é »õ·Î »ý¼º
		{
			var newObj = instance.CreateNewObject(); // ï¿½Ï³ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½î¼?
			newObj.gameObject.SetActive(true); // ï¿½Ø¿ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
            newObj.UpdateSessionID();
            return newObj;
		}
	}

	public static void ReturnObject(Sound obj) //ï¿½ï¿½ï¿? ï¿½ï¿½ ï¿½Ù½ï¿½ ï¿½ï¿½È¯
	{
		var source = obj.GetComponent<AudioSource>();
		source.clip = null;
		obj.gameObject.SetActive(false);
		instance.poolingObjectQueue.Enqueue(obj);
	}
	public SoundInstance SFXPlayPoolingVersion(AudioClip clip, bool IsLoop = false)
	{
		return PlaySFX(clip, IsLoop);
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



	// ï¿½Ì¼Ç¸ï¿½ BGM ï¿½Ã·ï¿½ï¿½ï¿½ ï¿½Ô¼ï¿½ ==========================================================
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