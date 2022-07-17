using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Plan : MonoBehaviour {
    Plan_Spawner PS;
	// Use this for initialization
	void Start () {
        PS = GameObject.Find("GameManager").GetComponent<Plan_Spawner>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D()
    {
        PS.Spawn_Trigger();
    }
}
