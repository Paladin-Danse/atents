using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public float Speed;
    public float Damage;

    bool Hit = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!Hit)
        {
            transform.Translate(Vector2.left * Speed);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<Player>().HPLost(Damage);
            Destroy_Object();
        }
    }

    public void Destroy_Object()
    {
        GetComponent<Animator>().SetTrigger("InDestroy");
        Hit = true;
        Destroy(GetComponent<Collider2D>());
    }
}
