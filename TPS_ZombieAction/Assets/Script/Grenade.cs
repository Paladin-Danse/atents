using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : ThrowItem
{
    [SerializeField] private GameObject ExplosionEffect;
    private ParticleSystem FireBall;
    MeshRenderer mesh;

    private void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        mesh.enabled = true;
    }

    public override void Explosion()
    {
        base.Explosion();

        StartCoroutine(Boom());
    }

    protected void ExplosionDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, f_ExplosionRange);
        for(int i=0; i<colliders.Length; i++)
        {
            LivingEntity entity = colliders[i].GetComponent<LivingEntity>();
            if(entity != null && !entity.b_Dead)
            {
                entity.OnDamage(f_Damage, entity.transform.position, transform.position - entity.transform.position);
            }
        }
    }

    private IEnumerator GrenadeExplosionEffect()
    {
        boomEffect.Play();

        yield return new WaitWhile(() => boomEffect.isPlaying);

        gameObject.SetActive(false);
    }

    private IEnumerator Boom()
    {
        yield return new WaitForSeconds(f_ExplosionTime);

        StartCoroutine(GrenadeExplosionEffect());
        ExplosionDamage();
        mesh.enabled = false;
    }
}
