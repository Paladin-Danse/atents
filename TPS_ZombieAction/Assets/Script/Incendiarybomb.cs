using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Incendiarybomb : ThrowItem
{
    [SerializeField] private ParticleSystem FlameEffect;//ºÒ±æ

    protected override void OnEnable()
    {
        base.OnEnable();
        FlameEffect.gameObject.SetActive(false);
    }

    protected override void ExplosionDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, f_ExplosionRange);

        for (int i = 0; i < colliders.Length; i++)
        {
            LivingEntity entity = colliders[i].GetComponent<LivingEntity>();
            if (entity != null && !entity.b_Dead)
            {
                entity.OnDamage(f_Damage, entity.transform.position, transform.position - entity.transform.position);
            }
        }

        OnFlame();
    }

    protected override IEnumerator ExplosionEffect()
    {
        if (boomEffect)
        {
            boomEffect.Play();

            if (FlameEffect)
            {
                FlameEffect.gameObject.SetActive(true);
                FlameEffect.Play();

                yield return new WaitWhile(() => FlameEffect.isPlaying);

                gameObject.SetActive(false);
            }
        }
    }

    protected void OnFlame()
    {
        FlameEffect.gameObject.SetActive(true);
        FlameEffect.Play();
    }
}
