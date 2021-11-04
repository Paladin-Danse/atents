using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour
{
    [SerializeField] private float f_FireDamege;
    [SerializeField] private float f_DamageToTime;

    private void OnTriggerStay(Collider other)
    {
        LivingEntity entity = other.GetComponent<LivingEntity>();
        if (entity)
        {
            if (entity.b_Dead == false)
            {
                entity.OnDotDamage(f_FireDamege, f_DamageToTime);
            }
        }
    }
}
