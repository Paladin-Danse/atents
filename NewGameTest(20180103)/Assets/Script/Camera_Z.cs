using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Z : MonoBehaviour {
    float Fix_Z = -10;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.position.z != Fix_Z)
        {
            Transform MyTransform = transform;
            MyTransform.position = new Vector3(MyTransform.position.x, MyTransform.position.y, Fix_Z);

            transform.position = MyTransform.position;
        }
	}
}
