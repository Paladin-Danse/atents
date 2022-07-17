using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    GameObject UnitTarget;

    public float speed;

    bool UnitContact = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector2.left * speed);
    }

    public void SetEnemyTarget(GameObject other)
    {
        UnitTarget = other;
    }
    public void SetEnemyContact(bool Contact)
    {
        UnitContact = Contact;
    }
}
