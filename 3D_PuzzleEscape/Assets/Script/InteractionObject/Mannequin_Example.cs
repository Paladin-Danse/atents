using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Mannequin_Example : MonoBehaviour
{
    [SerializeField] private GameObject[] Mannequin_Parts;
    public ItemData Mannequin_Head { get; private set; }
    public ItemData Mannequin_ArmL { get; private set; }
    public ItemData Mannequin_ArmR { get; private set; }
    public ItemData Mannequin_LegL { get; private set; }
    public ItemData Mannequin_LegR { get; private set; }

    [SerializeField] private Material Red_Mat;
    [SerializeField] private Material Blue_Mat;
    [SerializeField] private Material Green_Mat;
    [SerializeField] private Material Default_Mat;

    [SerializeField] private List<ItemData> Mannequin_PartsData;
    
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.M_Example_RandData_Read(this);
    }

    public void RandomMaterial_output(GameObject Parts, ref ItemData PartsData)
    {
        /*
        Material mat = UnityEngine.Random.Range(0, 4) switch
        {
            int i when i == 0 => Red_Mat,
            int i when i == 1 => Blue_Mat,
            int i when i == 2 => Green_Mat,
            _ => Default_Mat
        };
        */

        Material mat;
        string Partsname = Parts.name;
        
        switch(UnityEngine.Random.Range(0, 4))
        {
            case 0:
                mat = Red_Mat;
                //Mannequin_PartsData의 이름에 Red를 포함하고 dummy_뒤에 붙는 부품의 이름이 Parts의 이름을 포함할 경우 PartsData에 그 ItemData를 담는다.
                //ex)Parts의 이름이 dummy_Head이면 Mannequin_PartsData에서 이름에 Red가 포함되고, Head가 포함된 Puppet_Head_Red를 가져옴.
                PartsData = Mannequin_PartsData.Find(i => i.name.Contains("Red") && i.name.Contains(Partsname.Substring(Partsname.IndexOf("_") + 1)));
                break;
            case 1:
                mat = Blue_Mat;
                PartsData = Mannequin_PartsData.Find(i => i.name.Contains("Blue") && i.name.Contains(Partsname.Substring(Partsname.IndexOf("_") + 1)));
                break;
            case 2:
                mat = Green_Mat;
                PartsData = Mannequin_PartsData.Find(i => i.name.Contains("Green") && i.name.Contains(Partsname.Substring(Partsname.IndexOf("_") + 1)));
                break;
            default:
                //아무 색도 없는 경우 반대로 이름에 Red나 Blue나 Green이 포함되지 않은 경우 그리고 해당 부품의 이름이 Parts의 이름을 포함할 경우 PartsData에 그 ItemData를 담는다.
                mat = Default_Mat;
                PartsData = Mannequin_PartsData.Find(i => !(i.name.Contains("Red") || i.name.Contains("Blue") || i.name.Contains("Green")) && i.name.Contains(Partsname.Substring(Partsname.IndexOf("_") + 1)));
                break;
        }
        
        if (mat)
        {
            foreach (Renderer iter in Parts.GetComponentsInChildren<Renderer>())
            {
                iter.material = mat;
            }
        }
        if(!PartsData)
        {
            Debug.Log("Error(Mannequin_Example) : Not Found PartsData!");
        }
    }
    //게임을 새로시작할 때 랜덤한 값을 가져와 마네킹에 색을 입힘.
    public void newRandomParts_Pit()
    {
        foreach (GameObject iter in Mannequin_Parts)
        {
            ItemData data = new ItemData();

            RandomMaterial_output(iter, ref data);

            switch (iter.name)
            {
                case "dummy_Head":
                    Mannequin_Head = data;
                    if (GameManager.instance.mySavedata.Mini_3_GameObject_Head == null)
                        GameManager.instance.mySavedata.Mini_3_GameObject_Head = Mannequin_Head.Data.name;
                    break;
                case "dummy_left_arm":
                    Mannequin_ArmL = data;
                    if (GameManager.instance.mySavedata.Mini_3_GameObject_ArmL == null)
                        GameManager.instance.mySavedata.Mini_3_GameObject_ArmL = Mannequin_ArmL.Data.name;
                    break;
                case "dummy_right_arm":
                    Mannequin_ArmR = data;
                    if (GameManager.instance.mySavedata.Mini_3_GameObject_ArmR == null)
                        GameManager.instance.mySavedata.Mini_3_GameObject_ArmR = Mannequin_ArmR.Data.name;
                    break;
                case "dummy_left_leg":
                    Mannequin_LegL = data;
                    if (GameManager.instance.mySavedata.Mini_3_GameObject_LegL == null)
                        GameManager.instance.mySavedata.Mini_3_GameObject_LegL = Mannequin_LegL.Data.name;
                    break;
                case "dummy_right_leg":
                    Mannequin_LegR = data;
                    if (GameManager.instance.mySavedata.Mini_3_GameObject_LegR == null)
                        GameManager.instance.mySavedata.Mini_3_GameObject_LegR = Mannequin_LegR.Data.name;
                    break;
                default:
                    Debug.Log("Error(Mannequin_Example) : Not Found Mannequin_Parts");
                    break;
            }
            /*
            data = iter.name switch
            {
                string key when key.Contains("head") => Mannequin_Head,
                string key when key.Contains("left_arm") => Mannequin_ArmL,
                string key when key.Contains("right_arm") => Mannequin_ArmR,
                string key when key.Contains("left_leg") => Mannequin_LegL,
                string key when key.Contains("right_leg") => Mannequin_LegR,
                _ => null
            };
            */
        }
    }
    //게임을 로드할때 세이브 데이터에서 마네킹 데이터를 가져오고 해당 데이터에 맞춰 마네킹에 색을 입힘.
    public void MannequinEx_DataLoad()
    {
        SaveData savedata = GameManager.instance.mySavedata;

        foreach(GameObject iter in Mannequin_Parts)
        {
            Material mat = null;

            switch(iter.name)
            {
                case "dummy_Head":
                    Mannequin_Head = Mannequin_PartsData.Find(i => i.Data.name == savedata.Mini_3_GameObject_Head);
                    if (GameManager.instance.mySavedata.Mini_3_GameObject_Head == null)
                        GameManager.instance.mySavedata.Mini_3_GameObject_Head = Mannequin_Head.Data.name;
                    mat = GetColor(Mannequin_Head.Data.name);
                    break;
                case "dummy_left_arm":
                    Mannequin_ArmL = Mannequin_PartsData.Find(i => i.Data.name == savedata.Mini_3_GameObject_ArmL);
                    if (GameManager.instance.mySavedata.Mini_3_GameObject_ArmL == null)
                        GameManager.instance.mySavedata.Mini_3_GameObject_ArmL = Mannequin_ArmL.Data.name;
                    mat = GetColor(Mannequin_ArmL.Data.name);
                    break;
                case "dummy_right_arm":
                    Mannequin_ArmR = Mannequin_PartsData.Find(i => i.Data.name == savedata.Mini_3_GameObject_ArmR);
                    if (GameManager.instance.mySavedata.Mini_3_GameObject_ArmR == null)
                        GameManager.instance.mySavedata.Mini_3_GameObject_ArmR = Mannequin_ArmR.Data.name;
                    mat = GetColor(Mannequin_ArmR.Data.name);
                    break;
                case "dummy_left_leg":
                    Mannequin_LegL = Mannequin_PartsData.Find(i => i.Data.name == savedata.Mini_3_GameObject_LegL);
                    if (GameManager.instance.mySavedata.Mini_3_GameObject_LegL == null)
                        GameManager.instance.mySavedata.Mini_3_GameObject_LegL = Mannequin_LegL.Data.name;
                    mat = GetColor(Mannequin_LegL.Data.name);
                    break;
                case "dummy_right_leg":
                    Mannequin_LegR = Mannequin_PartsData.Find(i => i.Data.name == savedata.Mini_3_GameObject_LegR);
                    if (GameManager.instance.mySavedata.Mini_3_GameObject_LegR == null)
                        GameManager.instance.mySavedata.Mini_3_GameObject_LegR = Mannequin_LegR.Data.name;
                    mat = GetColor(Mannequin_LegR.Data.name);
                    break;
                default:
                    Debug.Log("Error(Mannequin_Example) : Not Found Mannequin_Parts");
                    break;
            }

            if (mat)
            {
                foreach (Renderer render in iter.GetComponentsInChildren<Renderer>())
                {
                    render.material = mat;
                }
            }
        }
    }
    private Material GetColor(string PartsName)
    {
        switch(PartsName.Substring(PartsName.IndexOf("(") + 1, 1))
        {
            case "적":
                return Red_Mat;
            case "청":
                return Blue_Mat;
            case "녹":
                return Green_Mat;
            default:
                return Default_Mat;
        }
    }
}
