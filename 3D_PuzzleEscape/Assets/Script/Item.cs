using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Item
{
    public string name;
    public int Quantity;
    public Sprite ItemSprite;
    public int Durability;
    public string Description;
}

public class InventoryItem
{
    public Item data;
}