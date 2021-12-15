using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum WEAPON_TYPE
{
    NONE = 0,
    MELEE,
    MAIN_WEAPON,
    SUB_WEAPON
}

[System.Serializable]
public struct Weapon
{
    public WEAPON_TYPE WeaponType;
    public string Name;
    public Image UI_Image;
    public GameObject Prefeb;
}
