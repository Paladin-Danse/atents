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
    public int itemNum;//아이템이 정렬되는 순서.
    public Item data;
}