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
    //������Ŵ����ε� BGM�ҽ� �ϳ��ۿ� ������...
    //������Ŵ���(������Ʈ) �ؿ� ���� ������Ʈ�� �ΰ� �ű⿡ ������ҽ��� ���°� �̻����� �� ����.
    //�׸��� ���� ����������Ʈ���� �� ����(BGM, Effect etc...)������� ����� ������ҽ��� �ϳ��� ������ ���庰�� �ϳ��� �ҽ��� ������ �ȴ�.
    //���� ���� ���� Ȥ�� ���� ������Ʈ�� �����Ű�� �ȴ�...
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
        //���� ���̺굥���Ϳ��� �����;� �� ���� ��������� ��ü�Ͽ���. ���� ���̺����� �����Ǹ� �ٽ� �պ� ��!
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

    //������ �����̴��� �����̸� �ҷ����� �Լ�
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
