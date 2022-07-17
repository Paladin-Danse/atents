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
}
