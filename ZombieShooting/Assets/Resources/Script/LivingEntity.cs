using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LivingEntity : MonoBehaviour, iDamageable
{
    public float startingHealth = 100f;
    public float health { get; protected set; }
    public bool dead { get; protected set; }
    public event Action OnDeath;

    protected virtual void OnEnable()
    {
        dead = false;
        health = startingHealth;
    }

    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        health -= damage;
        if(health <= 0 && !dead)
        {
            Die();
        }
    }

    public virtual void RestoreHealth(float newHealth)
    {
        if (dead) return;

        health += newHealth;
    }

    public virtual void Die()
    {
        //OnDeath 이벤트에 등록된 메소드가 있을때만 실행
        if(OnDeath != null)
        {
            OnDeath();
        }
        dead = true;
    }
}
