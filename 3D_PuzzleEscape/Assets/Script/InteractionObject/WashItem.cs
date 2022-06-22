using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WashItem : InteractionObject
{
    [SerializeField] private ItemData[] Mannequin_Data;//마네킹부품이 될 수 있는 모든 아이템 데이터값.
    [SerializeField] private ItemData Mannequin_Head_data;
    [SerializeField] private ItemData Mannequin_ArmL_data;
    [SerializeField] private ItemData Mannequin_ArmR_data;
    [SerializeField] private ItemData Mannequin_LegL_data;
    [SerializeField] private ItemData Mannequin_LegR_data;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //마네킹의 부품인지 체크하고 마네킹의 부품이 맞다면 어디 부위에 속하는지 확인하는 함수.
    public void Mannequin_PartCheck()
    {
        InventoryItem item = InventoryManager.instance.SelectedItem;
        string name = item.data.name;
        ItemData data;

        foreach (ItemData i in Mannequin_Data)
        {
            if (name == i.Data.name)
            {
                //마네킹 부품이 맞다면 어디 부품인지 다시 체크(스위치문으로 변경가능할듯?)
                if (name.Contains("인형머리"))
                {
                    data = Mannequin_Head_data;
                }
                else if (name.Contains("왼쪽 팔"))
                {
                    data = Mannequin_ArmL_data;
                }
                else if (name.Contains("오른쪽 팔"))
                {
                    data = Mannequin_ArmR_data;
                }
                else if (name.Contains("왼쪽 다리"))
                {
                    data = Mannequin_LegL_data;
                }
                else if (name.Contains("오른쪽 다리"))
                {
                    data = Mannequin_LegR_data;
                }
                else
                {
                    data = null;
                }

                InventoryManager.instance.GetItem(data);
                InventoryManager.instance.UseItem(item);
            }
        }
    }
}
