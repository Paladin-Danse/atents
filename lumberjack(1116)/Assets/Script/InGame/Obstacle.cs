using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {
    public float DamagePoint;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void On_Destroy()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.SendMessage("HPLost", DamagePoint);
        }
    }
}
