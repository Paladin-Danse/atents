using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct Item
{
    public string name;
    public int Quantity;
    public InteractionObject InteractableObj;
}

public class InventoryItem
{
    public ItemData data;
}