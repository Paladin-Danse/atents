using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitSearch : MonoBehaviour {
    public List<Collider2D> UnitList;
    A_to_B AB;
    
    // Use this for initialization
    void Start () {
        AB = gameObject.transform.parent.GetComponent<A_to_B>();
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"), true);
    }
	
	// Update is called once per frame
	void Update () {
        if (UnitList.Count != 0)
        {
            if (UnitList[0] == null)
            {
                UnitList.Remove(UnitList[0]);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Unit")
        {
            UnitList.Add(other);
            SendUnit(UnitList[0]);
        }
        if (other.tag == "CP")
        {
            SendCP(other);
        }

    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Unit")
        {
            SendUnit(UnitList[0]);
        }
        if (other.tag == "CP")
        {
            SendCP(other);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Unit")
        {
            UnitList.Remove(other);
            if (UnitList.Count == 0)
            {
                AB.SetEnemyContact(false);
            }
        }
        if (other.tag == "CP")
        {
            AB.SetCPContact(false);
        }
    }



    void SendUnit(Collider2D other)
    {
        AB.SetEnemyContact(true);
        AB.SetEnemyTarget(other);
    }

    void SendCP(Collider2D other)
    {
        AB.SetCPContact(true);
        AB.SetCPTarget(other);
    }
}
