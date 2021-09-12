using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour, IItem
{
    [SerializeField] private int ammo = 30;
    
    public void Use(GameObject target)
    {
        PlayerShooter playerShooter = target.GetComponent<PlayerShooter>();

        if(playerShooter != null && playerShooter.gun != null)
        {
            playerShooter.gun.ammoRemain += ammo;
        }
        Destroy(gameObject);
    }
}
