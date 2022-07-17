using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Medal : MonoBehaviour {
    Medal_Box MB;

    public Sprite Medal_On;
    public int Medal_Num;
	// Use this for initialization
	void Start () {
        MB = GameObject.Find("GameManager").GetComponent<Medal_Box>();

        if(MB.Get_All_Medal() >= Medal_Num)
        {
            GetComponent<Image>().sprite = Medal_On;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
