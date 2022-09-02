using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager m_instance;
    public static AudioManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = GameObject.FindObjectOfType<AudioManager>();
            }
            return m_instance;
        }
    }

    [SerializeField] private AudioMixer MasterMixer;
    //오디오매니저인데 BGM소스 하나밖에 못쓴다...
    //오디오매니저(오브젝트) 밑에 하위 오브젝트를 두고 거기에 오디오소스를 쓰는게 이상적일 것 같다.
    //그리고 이제 하위오브젝트들을 각 사운드(BGM, Effect etc...)담당으로 만들고 오디오소스를 하나씩 넣으면 사운드별로 하나씩 소스를 가지게 된다.
    //이제 다음 게임 혹은 다음 업데이트에 적용시키면 된다...
    private AudioSource BGM;
    [SerializeField] private AudioClip bgm_Intro;
    [SerializeField] private AudioClip bgm_Game;
    private float maxVolume;
    private float minVolume;
    private int SliderValue;

    private string VolumeKey = "Volume";
    private int Volume = 0;
    
    private void Awake()
    {
        if (AudioManager.instance.gameObject != gameObject) Destroy(gameObject);
        //원래 세이브데이터에서 가져와야 할 값을 상수값으로 대체하였다. 이후 세이브기능이 구현되면 다시 손볼 것!
        //Volume = 50;
        BGM = GetComponent<AudioSource>();
        Volume = PlayerPrefs.GetInt(VolumeKey, 50);
    }
    private void Start()
    {
        maxVolume = UIManager.instance.Slider_Audio.maxValue;
        minVolume = UIManager.instance.Slider_Audio.minValue;

        SliderValue = (int)-((maxVolume - minVolume) - (maxVolume - minVolume) * Volume / 100);
        MasterMixer.SetFloat("Master", SliderValue);
        UIManager.instance.Slider_Audio.value = SliderValue;
        
        DontDestroyOnLoad(gameObject);
    }
    public void OnScene(string sceneName)
    {
        if(sceneName == "IntroScene")
        {
            BGM.clip = bgm_Intro;
            if (BGM.clip)
            {
                BGM.Play();
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log("Error(AudioManager) : bgm_Intro is Not Found");
#endif
            }
        }
        else if(sceneName == "MainScene")
        {
            BGM.clip = bgm_Game;
            if (BGM.clip)
            {
                BGM.Play();
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log("Error(AudioManager) : bgm_Game is Not Found");
#endif
            }
        }
    }

    //볼륨의 슬라이더를 움직이면 불려오는 함수
    public void AudioControl()
    {
        if (UIManager.instance.Slider_Audio)
        {
            SliderValue = (int)UIManager.instance.Slider_Audio.value;
            Volume = (int)((SliderValue - minVolume) / (maxVolume - minVolume) * 100.0f);
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("Wrong Acess : Not Found AudioSlider");
#endif
            return;
        }
        if (SliderValue <= -40f) MasterMixer.SetFloat("Master", -80f);
        else MasterMixer.SetFloat("Master", SliderValue);

        UIManager.instance.SetVolumeText(Volume.ToString());
    }
    public int GetVolume()
    {
        return Volume;
    }

    public void SaveAudioOption()
    {
        PlayerPrefs.SetInt(VolumeKey, Volume);
    }
}
