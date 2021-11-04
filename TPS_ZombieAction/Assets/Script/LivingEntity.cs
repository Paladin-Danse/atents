using System;//있어야 Action을 쓸수있음
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, I_Damageable
{
    public float f_StartingHealth { get; protected set; } = 100f;
    public float f_Health { get; protected set; }
    public bool b_Dead { get; protected set; }
    protected float f_LastDotDamageTime;
    protected event Action OnDeath;
    public virtual void OnDamage(float m_damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        f_Health -= m_damage;

        if(f_Health <= 0 && !b_Dead) Die();
    }

    public virtual void OnDotDamage(float m_damage, float m_dotTime)
    {
        if (Time.time >= f_LastDotDamageTime + m_dotTime)
        {
            f_Health -= m_damage;
            if (f_Health <= 0 && !b_Dead) Die();
            f_LastDotDamageTime = Time.time;
        }
    }

    public virtual void RestoreHealth(float m_newHealth)
    {
        if (b_Dead || f_Health >= f_StartingHealth) return;

        f_Health += m_newHealth;
        if (f_Health > f_StartingHealth) f_Health = f_Health;
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
        f_LastDotDamageTime = Time.time;
    }
}
