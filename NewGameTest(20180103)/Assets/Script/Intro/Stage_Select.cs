using UnityEngine;
using System.Collections;

public class Stage_Select : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Go_Stage(int n)
    {
        Application.LoadLevel(n.ToString());
    }
}
