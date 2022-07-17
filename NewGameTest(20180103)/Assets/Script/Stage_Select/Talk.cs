using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Talk : MonoBehaviour {
    float _time;
    public float Talk_Time;

    public string[] Rabbit_Talk_bundle;
    int R_Num;
    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        

        _time += Time.deltaTime;
        if(_time >= Talk_Time)
        {
            gameObject.SetActive(false);
        }
	}

    void OnEnable()
    {
        if (gameObject.active)
        {
            R_Num = Random.Range(0, Rabbit_Talk_bundle.Length);
            gameObject.GetComponentInChildren<Text>().text = Rabbit_Talk_bundle[R_Num];
            _time = 0;
        }
    }
}
