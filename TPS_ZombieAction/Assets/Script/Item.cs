using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public enum ITEM_TYPE
{
    NONE = 0,
    POTION,
    AMMO,
    GRENADE,
    FLASHBANG,
    INCENDIARY_BOMB
}
//아이템류 인터페이스
[System.Serializable]
public struct Item// : MonoBehaviour
{
    public ITEM_TYPE type;
    public int quantity;
    public string iconName;
    public int value;
    //[SerializeField] protected Sprite m_InventoryImage;
    //public Sprite InventoryImage { get { return m_InventoryImage; } }
    //public int Num { get; protected set; }
    //public abstract void Loot(GameObject target);
    //public abstract void SetPosition(Vector3 pos);
    //public void NumUp() { Num++; }
    //public void NumDown() { Num--; }
    
    /*
    public virtual void Use()
    {
        NumDown();
        UIManager.instance.UpdateInventory(InventoryImage, Num);
    }
    */
}

public class UseItem
{
    public Item data;
    public event Action UseEvent;
    public bool isThrowing
    {
        get
        {
            switch(data.type)
            {
                case ITEM_TYPE.POTION:
                case ITEM_TYPE.AMMO:
                    return false;
                case ITEM_TYPE.GRENADE:
                case ITEM_TYPE.FLASHBANG:
                case ITEM_TYPE.INCENDIARY_BOMB:
                    return true;
            }
            return false;
        }
    }
    public void Use()
    {
        if(null != UseEvent) UseEvent();
    }
}
