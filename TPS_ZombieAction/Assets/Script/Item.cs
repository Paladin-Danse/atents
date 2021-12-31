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
    INCENDIARY_BOMB,
    KEY_ITEM
}
//아이템류 인터페이스
[System.Serializable]
public struct Item// : MonoBehaviour
{
    public ITEM_TYPE type;
    public int quantity;
    public string iconName;
    public int value;
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
                case ITEM_TYPE.KEY_ITEM:
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
