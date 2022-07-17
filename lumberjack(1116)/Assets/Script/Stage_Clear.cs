using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage_Clear : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void On_Stage_Clear()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().GameWin();
        GameObject.Find("GameManager").GetComponent<GameManager>().On_ClearSound();
    }
}
