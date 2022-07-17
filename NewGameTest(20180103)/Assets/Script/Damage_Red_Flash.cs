using UnityEngine;
using System.Collections;

public class Damage_Red_Flash : MonoBehaviour {
    Renderer renderer;
    float _time;
    public float Damage_Time;
    
    // Use this for initialization
    void Start () {
        renderer = GetComponent<Renderer>();
    }
	
	// Update is called once per frame
	void Update () {
        if (renderer.material.color != Color.white)//데미지를 입을경우 붉은색에서 점차 하얗게 바꿔줌
        {
            renderer.material.color = Color.Lerp(Color.red, Color.white, (Time.time - _time) / Damage_Time);
        }
    }
    public void Red_Flash()
    {
        _time = Time.time;
        if (renderer.material.color != Color.red)
        {
            renderer.material.color = Color.red;
        }
    }
}
