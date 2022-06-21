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
    [SerializeField] private Mannequin_Example example;
    [SerializeField] private ItemData[] Mannequin_Data;//마네킹부품이 될 수 있는 모든 아이템 데이터값.

    private ItemData[] Mannequin_PartsData;
    

    // Start is called before the first frame update
    private new void Start()
    {
        base.Start();
        Mannequin_PartsData = new ItemData[5];
        InteractionEvent += Mannequin_PartCheck;
    }

    //마네킹을 장비하고 있는 아이템으로 상호작용할 때 마네킹의 부품인지 체크하고 마네킹의 부품이 맞다면 어디 부위에 속하는지 확인하는 함수.
    public void Mannequin_PartCheck()
    {
        InventoryItem item = InventoryManager.instance.SelectedItem;
        foreach (ItemData i in Mannequin_Data)
        {
            if(item.data.name == i.Data.name)
            {
                //마네킹 부품이 맞다면 어디 부품인지 다시 체크 string.indexOf() 사용.
                switch(item.data.name)
                {
                    
                }

                InventoryManager.instance.UseItem(item);
            }
        }
    }

    void Mannequin_fit(GameObject Part)
    {

    }
}
