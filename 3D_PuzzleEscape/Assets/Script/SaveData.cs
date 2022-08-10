using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//아이템 전용의 새로운 구조체(Sprite타입은 들어가면 안됨.)
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
    STAGE_1 = 1,
    STAGE_2,
    STAGE_3,
    END
}

[System.Serializable]
public class SaveData
{
    public List<Save_ItemData> itemdata = new List<Save_ItemData>();
    public STAGE Stage = STAGE.STAGE_1;
    
    //Player
    public float PlayerPositionX = -10f;
    public float PlayerPositionY = 1.5f;
    public float PlayerPositionZ = 4f;
    public float PlayerRotationX = 0f;
    public float PlayerRotationY = -90f;
    public float PlayerRotationZ = 0f;

    public bool Mini_1_Clear = false;
    public bool Mini_2_Clear = false;
    public bool Mini_3_Clear = false;

    public bool b_Mannequin_Data = false;
    public string Mini_3_GameObject_Head = null;
    public string Mini_3_GameObject_ArmL = null;
    public string Mini_3_GameObject_ArmR = null;
    public string Mini_3_GameObject_LegL = null;
    public string Mini_3_GameObject_LegR = null;
}