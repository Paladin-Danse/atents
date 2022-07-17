using UnityEngine;
using System.Collections;

public class Background_Move : MonoBehaviour
{
    public Renderer renderer;
    float x;
    public float speed;
    bool B_Stop = false;
	// Use this for initialization
	void Start () {
        renderer = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!B_Stop)
        {
            x += speed * Time.deltaTime;
            renderer.material.SetTextureOffset("_MainTex", new Vector2(x, 0));
        }
    }

    public void Background_Stop()
    {
        B_Stop = true;
    }

    public void Background_OnMove()
    {
        B_Stop = false;
    }
}
