using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : ThrowItem
{
    protected override void ExplosionDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, f_ExplosionRange);
        
        for (int i=0; i<colliders.Length; i++)
        {
            LivingEntity entity = colliders[i].GetComponent<LivingEntity>();
            if (entity != null && !entity.b_Dead && entity.tag.Equals("Enemy"))
            {
                entity.OnDamage(f_Damage, entity.transform.position, transform.position - entity.transform.position);
            }
            else if (entity != null && !entity.b_Dead && entity.tag.Equals("Player"))
            {
                entity.OnDamage((f_Damage * 0.3f), entity.transform.position, transform.position - entity.transform.position);
            }
        }
    }

    
}
