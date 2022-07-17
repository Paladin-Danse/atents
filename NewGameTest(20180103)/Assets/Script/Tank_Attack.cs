using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Attack : MonoBehaviour {
    public GameObject Boom;
    
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Attack_Boom(Collider2D other)
    {
        Vector3 other_Pos = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z - 1f);
        Instantiate(Boom, other_Pos, Quaternion.identity);
    }
}
