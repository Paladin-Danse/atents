using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IItem
{
    [SerializeField] private int health = 50;
    public void Use(GameObject target)
    {
        LivingEntity life = target.GetComponent<LivingEntity>();

        if (life != null)
        {
            life.RestoreHealth(health);
        }
        Destroy(gameObject);
    }
}
