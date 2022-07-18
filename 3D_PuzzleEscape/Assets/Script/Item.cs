using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Item
{
    public string name;
    public int Quantity;
    public Sprite ItemSprite;
    //public string SpriteName;
    public int Durability;
    public int Durability_Max;
    public string Description;
}

[System.Serializable]
public struct MixItem
{
    /*
    public Item item1;
    public Item item2;
    public Item Mixeditem;
    */
    public ItemData item1;
    public ItemData item2;
    public ItemData Mixeditem;
}

[System.Serializable]
public class InventoryItem
{
    public int itemNum;//아이템이 정렬되는 순서.
    public Item data;
}