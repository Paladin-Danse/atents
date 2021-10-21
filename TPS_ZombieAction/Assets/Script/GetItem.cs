using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItem : MonoBehaviour
{
    public ITEM_TYPE type;
    public void Looting()
    {
        InventoryManager.instance.Loot(type);
        gameObject.SetActive(false);
    }
}
