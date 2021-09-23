using System;//있어야 Action을 쓸수있음
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, I_Damageable
{
    public float f_StartingHealth { get; protected set; } = 100f;
    public float f_Health { get; protected set; }
    public bool b_Dead { get; protected set; }
    protected event Action OnDeath;
    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        f_Health -= damage;

        if(f_Health <= 0 && !b_Dead) Die();
    }

    public virtual void RestoreHealth(float newHealth)
    {
        if (b_Dead) return;

        f_Health += newHealth;
    }

    public virtual void Die()
    {
        if(OnDeath != null)
        {
            OnDeath();
        }

        b_Dead = true;
    }

    protected virtual void OnEnable()
    {
        b_Dead = false;
        f_Health = f_StartingHealth;
    }
}
