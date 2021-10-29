using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowItem : MonoBehaviour
{
    [SerializeField] protected ParticleSystem boomEffect;
    [SerializeField] protected float f_Damage;
    [SerializeField] protected float f_ExplosionTime;
    [SerializeField] protected float f_ExplosionRange;

    public virtual void Explosion()
    {
        
    }
}
