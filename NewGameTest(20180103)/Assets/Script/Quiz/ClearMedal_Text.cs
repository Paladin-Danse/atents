using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearMedal_Text : MonoBehaviour {
    //Medal_Box MB;
    int Medal;

    public Sprite ClearMedal_2;
    public Sprite ClearMedal_3;
    public Sprite GameOver_Medal;
	// Use this for initialization
	void Start () {
        //MB = GameObject.Find("GameManager").GetComponent<Medal_Box>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Set_Medal(int medal)
    {
        Medal = medal;
        if (Medal == 2)
        {
            GetComponent<Image>().sprite = ClearMedal_2;
        }
        else if(Medal == 3)
        {
            GetComponent<Image>().sprite = ClearMedal_3;
        }
        else
        {
            GetComponent<Image>().sprite = GameOver_Medal;
        }
    }
}
