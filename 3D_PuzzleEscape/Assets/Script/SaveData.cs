using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ ������ ���ο� ����ü(SpriteŸ���� ���� �ȵ�.)
[System.Serializable]
public struct Save_ItemData
{
    public string name;
    public int Quantity;
    public string SpriteName;
    public int Durability;
    public int Durability_Max;
    public string Description;
}

[System.Serializable]
public class SaveData
{
    /*private string SaveFile_path;
    private string LoadFile_path;*/
    public List<Save_ItemData> itemdata;
    public int Volume;
}