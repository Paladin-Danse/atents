using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum STATE
    {
        STATE_READY,
        STATE_EMPTY,
        STATE_RELOADING
    }
    public STATE e_State;

    public enum TYPE
    {
        TYPE_AUTO,
        TYPE_SEMIAUTO
    }
    public TYPE e_Type;

    public Transform fireTransform;

    public ParticleSystem muzzleFlashEffect;
    public ParticleSystem shellEjectEffect;

    private float f_Damage;
}
