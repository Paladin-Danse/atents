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
    public int Durability_Max;
    public string Description;
    public Collider Interaction_Enable_Collider;
}

public class InventoryItem
{
    public int itemNum;//�������� ���ĵǴ� ����.
    public Item data;
}