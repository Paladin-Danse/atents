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
public enum STAGE
{
    START = 1,
    STAGE_1 = 1,
    STAGE_2,
    STAGE_3,
    END
}

[System.Serializable]
public class SaveData
{
    public List<Save_ItemData> itemdata;
    public STAGE Stage = STAGE.STAGE_1;
}