using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowItem : MonoBehaviour
{
    [SerializeField] protected ParticleSystem boomEffect;
    [SerializeField] protected float f_Damage;
    [SerializeField] protected float f_ExplosionTime;
    [SerializeField] protected float f_ExplosionRange;
    protected MeshRenderer mesh;

    protected virtual void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    protected virtual void OnEnable()
    {
        mesh.enabled = true;
    }

    public virtual void Explosion()
    {
        StartCoroutine(Boom());
    }

    protected virtual void ExplosionDamage()
    {
        
    }

    protected virtual IEnumerator ExplosionEffect()
    {
        if (boomEffect)
        {
            boomEffect.Play();

            yield return new WaitWhile(() => boomEffect.isPlaying);

            gameObject.SetActive(false);
        }
    }

    protected IEnumerator Boom()
    {
        yield return new WaitForSeconds(f_ExplosionTime);

        StartCoroutine(ExplosionEffect());
        ExplosionDamage();
        mesh.enabled = false;
    }
}
