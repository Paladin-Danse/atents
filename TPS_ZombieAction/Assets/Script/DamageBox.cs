using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBox : MonoBehaviour
{
    float f_Damage = 0;

    public void Setup(float newDamage)
    {
        f_Damage = newDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            LivingEntity livingEntity = other.GetComponent<LivingEntity>();
            if (livingEntity != null && !livingEntity.b_Dead)
            {
                livingEntity.OnDamage(f_Damage, transform.position, transform.position - other.transform.position);
            }
        }
    }
}
