using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Option_OnOff : MonoBehaviour {
    GameManager GM;
    public GameObject Option_menu;
	// Use this for initialization
	void Start () {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Option_On()
    {
        Option_menu.SetActive(true);
        GM.GameStay();
    }
    public void Option_Off()
    {
        Option_menu.SetActive(false);
        GM.GamePlay();
    }
}
