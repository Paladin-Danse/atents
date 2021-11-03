using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour
{
    [SerializeField] private float f_FireDamege;
    [SerializeField] private float f_DamageToTime;
    private float f_LastDamageTime = 0f;
    private void OnTriggerStay(Collider other)
    {
        LivingEntity entity = other.GetComponent<LivingEntity>();
        if(entity)
        {
            if(entity.b_Dead == false)
            {
                if(Time.deltaTime > f_LastDamageTime + f_DamageToTime)
                {
                    entity.OnDamage(f_FireDamege, entity.transform.position, transform.position - entity.transform.position);
                }
            }
        }
    }
}
