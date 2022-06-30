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
        foreach (GameObject iter in Mannequin_Parts)
        {
            ItemData data = new ItemData();

            RandomMaterial_output(iter, ref data);

            switch(iter.name)
            {
                case "dummy_Head":
                    Mannequin_Head = data;
                    break;
                case "dummy_left_arm":
                    Mannequin_ArmL = data;
                    break;
                case "dummy_right_arm":
                    Mannequin_ArmR = data;
                    break;
                case "dummy_left_leg":
                    Mannequin_LegL = data;
                    break;
                case "dummy_right_leg":
                    Mannequin_LegR = data;
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
}
