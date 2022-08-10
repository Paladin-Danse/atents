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
                //Mannequin_PartsData�� �̸��� Red�� �����ϰ� dummy_�ڿ� �ٴ� ��ǰ�� �̸��� Parts�� �̸��� ������ ��� PartsData�� �� ItemData�� ��´�.
                //ex)Parts�� �̸��� dummy_Head�̸� Mannequin_PartsData���� �̸��� Red�� ���Եǰ�, Head�� ���Ե� Puppet_Head_Red�� ������.
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
                //�ƹ� ���� ���� ��� �ݴ�� �̸��� Red�� Blue�� Green�� ���Ե��� ���� ��� �׸��� �ش� ��ǰ�� �̸��� Parts�� �̸��� ������ ��� PartsData�� �� ItemData�� ��´�.
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
    //������ ���ν����� �� ������ ���� ������ ����ŷ�� ���� ����.
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
    //������ �ε��Ҷ� ���̺� �����Ϳ��� ����ŷ �����͸� �������� �ش� �����Ϳ� ���� ����ŷ�� ���� ����.
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
            case "��":
                return Red_Mat;
            case "û":
                return Blue_Mat;
            case "��":
                return Green_Mat;
            default:
                return Default_Mat;
        }
    }
}
