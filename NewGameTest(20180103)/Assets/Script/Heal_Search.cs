using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Heal_Search : MonoBehaviour {
    public List<Collider2D> UnitList;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Unit")
        {
            UnitList.Add(other);
        }

    }
    void OnTriggerStay2D(Collider2D other)
    {

    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Unit")
        {
            UnitList.Remove(other);
            if (UnitList.Count == 0)
            {
            }
        }
    }
}
