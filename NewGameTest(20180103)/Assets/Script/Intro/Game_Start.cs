using UnityEngine;
using System.Collections;

public class Game_Start : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void game_Start()
    {
        Application.LoadLevel("Stage_Select");
    }
}
