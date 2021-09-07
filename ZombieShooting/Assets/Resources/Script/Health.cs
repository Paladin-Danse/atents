using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IItem
{
    [SerializeField] private int health = 50;
    public void Use(GameObject target)
    {
        Debug.Log("체력이 증가했다.");
    }
}
