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
    private int Volume;

    private void Awake()
    {
        //���� ���̺굥���Ϳ��� �����;� �� ���� ��������� ��ü�Ͽ���. ���� ���̺����� �����Ǹ� �ٽ� �պ� ��!
        Volume = 100;
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    //������ �����̴��� �����̸� �ҷ����� �Լ�
    public void AudioControl()
    {
        float sound;
        if (UIManager.instance.Slider_Audio)
        {
            sound = UIManager.instance.Slider_Audio.value;
            Volume = (int)((sound - UIManager.instance.Slider_Audio.minValue) / (UIManager.instance.Slider_Audio.maxValue - UIManager.instance.Slider_Audio.minValue) * 100.0f);
        }
        else
        {
            Debug.Log("Wrong Acess : Not Found AudioSlider");
            return;
        }
        if (sound <= -40f) MasterMixer.SetFloat("Master", -80f);
        else MasterMixer.SetFloat("Master", sound);

        UIManager.instance.SetVolumeText(Volume.ToString());
    }
    public int GetVolume()
    {
        return Volume;
    }
}
