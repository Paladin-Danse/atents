using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_Damageable
{
    public void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal);
}
