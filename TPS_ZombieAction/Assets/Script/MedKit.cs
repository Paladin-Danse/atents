using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedKit : CItem
{
    [SerializeField] private float f_HealPoint = 20f;
    public override void Loot(GameObject target)
    {
        //InventoryManager.instance.
    }

    public override CItem NewItem()
    {
        throw new System.NotImplementedException();
    }

    public override void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }
}
