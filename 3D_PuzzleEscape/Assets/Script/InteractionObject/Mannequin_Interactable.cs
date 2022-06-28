using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mannequin_Interactable : InteractionObject
{
    [SerializeField] private GameObject Mannequin_Head;
    [SerializeField] private GameObject Mannequin_ArmL;
    [SerializeField] private GameObject Mannequin_ArmR;
    [SerializeField] private GameObject Mannequin_LegL;
    [SerializeField] private GameObject Mannequin_LegR;
    [SerializeField] private ItemData[] Mannequin_Data;//마네킹부품이 될 수 있는 모든 아이템 데이터값.
    [SerializeField] private Material Red_Mat;
    [SerializeField] private Material Blue_Mat;
    [SerializeField] private Material Green_Mat;
    [SerializeField] private Material Default_Mat;

    // Start is called before the first frame update
    private new void Start()
    {
        base.Start();
        InteractionEvent += Mannequin_PartCheck;

        Mannequin_Head.SetActive(false);
        Mannequin_ArmL.SetActive(false);
        Mannequin_ArmR.SetActive(false);
        Mannequin_LegL.SetActive(false);
        Mannequin_LegR.SetActive(false);
    }

    //마네킹을 장비하고 있는 아이템으로 상호작용할 때 마네킹의 부품인지 체크하고 마네킹의 부품이 맞다면 어디 부위에 속하는지 확인하는 함수.
    public void Mannequin_PartCheck()
    {
        InventoryItem item = InventoryManager.instance.SelectedItem;
        string name = item.data.name;

        foreach (ItemData i in Mannequin_Data)
        {
            if(name == i.Data.name)
            {
                GameObject Parts;

                //마네킹 부품이 맞다면 어디 부품인지 다시 체크
                switch (name)
                {
                    case string key when name.Contains("인형머리"):
                        Parts = Mannequin_Head;
                        break;
                    case string key when name.Contains("왼쪽 팔"):
                        Parts = Mannequin_ArmL;
                        break;
                    case string key when name.Contains("오른쪽 팔"):
                        Parts = Mannequin_ArmR;
                        break;
                    case string key when name.Contains("왼쪽 다리"):
                        Parts = Mannequin_LegL;
                        break;
                    case string key when name.Contains("오른쪽 다리"):
                        Parts = Mannequin_LegR;
                        break;
                    default:
                        Parts = null;
                        break;
                }

                if (Parts)
                {
                    Mannequin_fit(Parts, item.data, name);
                    InventoryManager.instance.UseItem(item);

                    if (Mannequin_Head.activeSelf &&
                        Mannequin_ArmL.activeSelf &&
                        Mannequin_ArmR.activeSelf &&
                        Mannequin_LegL.activeSelf &&
                        Mannequin_LegR.activeSelf)
                        GameManager.instance.M_Example_Compare_Data();
                }
                else
                {
                    Debug.Log("Error(Mannequin_Interactable) : Not Found Parts Object!");
                }
                break;
            }
        }
    }

    //마네킹 부품을 활성화하고 무슨 색을 입혔는지 확인해서 해당 Material을 입히는 함수.
    void Mannequin_fit(GameObject Part, Item data, string name)
    {
        Material mat;

        Part.SetActive(true);
        
        if (name.Contains("적"))
            mat = Red_Mat;
        else if(name.Contains("청"))
            mat = Blue_Mat;
        else if(name.Contains("녹"))
            mat = Green_Mat;
        else
            mat = Default_Mat;

        //만약 틀린 색을 끼워맞췄을 경우 부품을 다시 뽑을 수 있게 해당 부품을 주울 수 있는 아이템으로 생성한다.
        if (Part.GetComponent<Mannequin_GetParts>())
        {
            ItemData itemData = new ItemData();
            itemData.InputData(data);
            Part.GetComponent<Mannequin_GetParts>().SetItem(itemData);

            //GameManager에 ItemData로 치환한 값 itemData를 M_PartsData에 입력한다.(인벤토리 내에 있는 아이템은 InventoryItem타입이라 ItemData와 호환불가).
            GameManager.instance.M_InteractableData_Read(itemData);
        }
        //Material 씌우기.
        if (mat)
        {
            foreach(Renderer iter in Part.GetComponentsInChildren<Renderer>())
            {
                iter.material = mat;
            }
        }
    }
}
