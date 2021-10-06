using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItem : MonoBehaviour
{
    [SerializeField] Type type;
    public void Looting()
    {
        InventoryManager.instance.Loot(type);
    }
}
