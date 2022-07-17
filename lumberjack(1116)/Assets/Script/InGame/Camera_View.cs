using UnityEngine;
using System.Collections;

public class Camera_View : MonoBehaviour {
    Transform Target;

    Vector3 Distance;
    public float x;
    public float y;
    public float z;
	// Use this for initialization
	void Start () {
        Target = GameObject.Find("Player").transform;
        Distance = new Vector3(x, y, z);
    }
	
	// Update is called once per frame
	void Update () {
        
        gameObject.transform.position = new Vector3(Target.position.x + Distance.x, Distance.y, Distance.z);
        
    }
}
