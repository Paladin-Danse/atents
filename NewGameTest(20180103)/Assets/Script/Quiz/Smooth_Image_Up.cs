using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Smooth_Image_Up : MonoBehaviour {
    public Image Effect1;
    public Image Effect2;
    public Image Image_Text;

    public float Effect_Time;
    float Effect1_time = 0;
    float Effect2_time = 0;
    float Image_Text_time = 0;
    // Use this for initialization
    void Start () {
        Effect1.color = Color.clear;
        if(Effect2) Effect2.color = Color.clear;
        Image_Text.color = Color.clear;
    }
	
	// Update is called once per frame
	void Update () {
		if(Effect1.color.a != 1)
        {
            Effect1_time += Time.deltaTime;
            Effect1.color = Color.Lerp(Color.clear, Color.white, Effect1_time / Effect_Time);
        }
        if (Effect2)
        {
            if (Effect2.color.a != 1)
            {
                Effect2_time += Time.deltaTime;
                Effect2.color = Color.Lerp(Color.clear, Color.white, Effect2_time / Effect_Time);
            }
        }
        if (Image_Text.color.a != 1)
        {
            Image_Text_time += Time.deltaTime;
            Image_Text.color = Color.Lerp(Color.clear, Color.white, Image_Text_time / Effect_Time);
        }
    }
}
