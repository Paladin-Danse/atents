using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPack : MonoBehaviour, i_Item
{
    [SerializeField] private int i_Ammo = 30;
    public void Loot(GameObject target)
    {
        PlayerShooter playerShooter = target.GetComponent<PlayerShooter>();

        if(playerShooter != null)
        {
            if (playerShooter.GetAmmo(i_Ammo))
            {
                Destroy(gameObject);
            }
        }
    }
}
