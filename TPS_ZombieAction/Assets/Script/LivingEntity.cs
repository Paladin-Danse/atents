using System;//�־�� Action�� ��������
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, I_Damageable
{
    [SerializeField] protected float f_StartingHealth = 100f;
    public float f_Health { get; protected set; }
    public bool b_Dead { get; protected set; }
    public event Action e_OnDeath;
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
        if(e_OnDeath != null)
        {
            e_OnDeath();
        }

        b_Dead = true;
    }

    protected virtual void OnEnable()
    {
        b_Dead = false;
        f_Health = f_StartingHealth;
    }
}
