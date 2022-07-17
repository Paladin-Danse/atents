using UnityEngine;
using System.Collections;

public class Scene_Move : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GoScene(string SceneName)
    {
        Application.LoadLevel(SceneName);
    }

    public void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void Playing_MainSound()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().On_MainSound();
    }

    public void Playing_InGameSound()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().On_InGameSound();
    }
}
