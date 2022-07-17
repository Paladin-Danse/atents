using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Awake : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Monster")
        {
            other.GetComponent<Monster>().On_Awake();
            GameObject.Find("GameManager").GetComponent<GameManager>().On_MonsterBattle();
            other.transform.SetParent(null);
        }
    }
}
