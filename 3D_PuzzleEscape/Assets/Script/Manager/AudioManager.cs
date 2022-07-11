using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer MasterMixer;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void AudioControl()
    {
        float sound;
        if (UIManager.instance.AudioSlider) sound = UIManager.instance.AudioSlider.value;
        else
        {
            Debug.Log("Wrong Acess : Not Found AudioSlider");
            return;
        }
        if (sound <= -40f) MasterMixer.SetFloat("Master", -80f);
        else MasterMixer.SetFloat("Master", sound);
    }
}
