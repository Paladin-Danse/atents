using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Boom : MonoBehaviour {
    List<Collider2D> UnitList;
    public float ATK_Point;
    float _time = 0;

    public bool Enemy;
    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        _time += Time.deltaTime;
        if(_time >= 0.1f)
        {
            Destroy(gameObject.GetComponent<Collider2D>());
            if(_time >= 0.8f)
            {
                Destroy(gameObject);
            }
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (Enemy)
        {
            if (other.tag == "Unit")
            {
                Debug.Log(other.gameObject);
                other.gameObject.GetComponent<A_to_B>().HPLost(ATK_Point);
            }
        }
        else
        {
            if (other.tag == "Enemy")
            {
                Debug.Log(other.gameObject);
                other.gameObject.GetComponent<A_to_B>().HPLost(ATK_Point);
            }
        }
    }
}
