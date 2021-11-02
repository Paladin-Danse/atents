using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashbang : ThrowItem
{
    protected override void ExplosionDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, f_ExplosionRange);

        for (int i = 0; i < colliders.Length; i++)
        {
            Enemy entity = colliders[i].GetComponent<Enemy>();
            if (entity != null && !entity.b_Dead)
            {
                entity.OnSupDamage(f_Damage);
            }
        }
    }
}
