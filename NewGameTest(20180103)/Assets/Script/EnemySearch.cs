using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySearch : MonoBehaviour {
    public List<Collider2D> EnemyList;
    A_to_B AB;

	// Use this for initialization
	void Start () {
        AB = gameObject.transform.parent.GetComponent<A_to_B>();
        EnemyList = new List<Collider2D>();
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Unit"), LayerMask.NameToLayer("Unit"), true);
	}
	
	// Update is called once per frame
	void Update () {
        if (EnemyList.Count != 0)
        {
            if (EnemyList[0] == null)
            {
                EnemyList.Remove(EnemyList[0]);
            }
        }
	}
    
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            //EnemyList.Add(other);
            SendEnemy(EnemyList[0]);
        }
        if (other.tag == "CP")
        {
            SendCP(other);
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            EnemyList.Add(other);
            SendEnemy(EnemyList[0]);
        }
        if (other.tag == "CP")
        {
            SendCP(other);
        }
    }
    
    
    void OnTriggerExit2D(Collider2D other)
    {
        
        if (other.tag == "Enemy")
        {
            EnemyList.Remove(other);
            if (EnemyList.Count == 0)
            {
                AB.SetEnemyContact(false);
            }
        }
        if (other.tag == "CP")
        {
            AB.SetCPContact(false);
        }
    }

    void SendEnemy(Collider2D other)
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
