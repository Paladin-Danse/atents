using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Mannequin_Example : MonoBehaviour
{
    [SerializeField] private GameObject[] Mannequin_Parts;
    [SerializeField] private ItemData Mannequin_Head;
    [SerializeField] private ItemData Mannequin_ArmL;
    [SerializeField] private ItemData Mannequin_ArmR;
    [SerializeField] private ItemData Mannequin_LegL;
    [SerializeField] private ItemData Mannequin_LegR;

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
                case "dummy_head":
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
    }

    public void GetMannequinData()
    {

    }
    public void RandomMaterial_output(GameObject Parts, ref ItemData PartsData)
    {
        //전체적으로 수정필요
        //마네킹 랜덤 색 지정 하는 순서
        //부품 지정 -> 랜덤색 지정 -> 지정된 색의 ItemData불러와야 됨.(복잡함)

        //오히려 마네킹에 부품 끼울 때처럼 아이템 데이터를 한 배열변수에 전부 저장.
        //ex)머리데이터만 뽑아서 임시배열변수에 넣고 거기서 랜덤값을 산출, 스위치문에서 ItemData와 material입히기까지 하면?

        Material mat = UnityEngine.Random.Range(0, 4) switch
        {
            int i when i == 0 => Red_Mat,
            int i when i == 1 => Blue_Mat,
            int i when i == 2 => Green_Mat,
            _ => Default_Mat
        };
        
        switch(UnityEngine.Random.Range(0, 4))
        {
            case 0:
                mat = Red_Mat;
                //PartsData = Mannequin_PartsData.Find()
                break;
            case 1:
                mat = Blue_Mat;
                break;
            case 2:
                mat = Green_Mat;
                break;
            default:
                mat = Default_Mat;
                break;
        }
        
        if (mat)
        {
            foreach (Renderer iter in Parts.GetComponentsInChildren<Renderer>())
            {
                iter.material = mat;
            }
        }
        
    }
}
