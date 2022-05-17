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
    [SerializeField] private ItemData[] Mannequin_Data;

    private ItemData[] Mannequin_PartsData;
    

    // Start is called before the first frame update
    private new void Start()
    {
        base.Start();
        Mannequin_PartsData = new ItemData[5];
        InteractionEvent += Mannequin_PartCheck;
    }

    public void Mannequin_PartCheck()
    {
        foreach (ItemData i in Mannequin_Data)
        {
            if(InventoryManager.instance.SelectedItem.data.name == i.Data.name)
            {

            }
        }
    }

    void Mannequin_fit(GameObject Part)
    {

    }
}
