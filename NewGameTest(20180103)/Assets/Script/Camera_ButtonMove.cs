using UnityEngine;
using System.Collections;

public class Camera_ButtonMove : MonoBehaviour {
    public float MoveSpeed;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    

    public void Camera_Up()
    {
        transform.position = new Vector3(0, Mathf.Clamp(transform.position.y + MoveSpeed, 0, 9), -10);
    }

    public void Camera_Down()
    {
        transform.position = new Vector3(0, Mathf.Clamp(transform.position.y - MoveSpeed, 0, 9), -10);
    }
}
