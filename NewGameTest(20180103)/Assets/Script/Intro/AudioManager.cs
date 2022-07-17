using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
    public AudioClip MainSound;
    public AudioClip InGameSound;
    AudioSource AS;
	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(transform.gameObject);
        AS = GetComponent<AudioSource>();

    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void On_MainSound()
    {
        AS.Stop();
        if (MainSound)
        {
            AS.clip = MainSound;
            AS.Play();
        }
        else
        {
            Debug.Log("Not MainSound");
        }
    }

    public void On_InGameSound()
    {
        AS.Stop();
        if (InGameSound)
        {
            AS.clip = InGameSound;
            AS.Play();
        }
        else
        {
            Debug.Log("Not InGameSound");
        }
    }
}
