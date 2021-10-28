using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowItem : MonoBehaviour
{
    [SerializeField] private ParticleSystem boomEffect;
    [SerializeField] private float f_Damage;
    [SerializeField] private float f_ExploTime;

    protected IEnumerator Explosion()
    {
        yield return new WaitForSeconds(f_ExploTime);

        boomEffect.Play();
    }

    protected void ExplosionDamage()
    {
        LivingEntity[] entities;

        Collider[] colliders = Physics.OverlapSphere(transform.position, 20f);
    }
}
