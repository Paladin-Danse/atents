using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour, IItem
{
    [SerializeField] private int ammo = 30;
    public void Use(GameObject target)
    {
        Debug.Log("탄약이 증가했다.");
    }
}
