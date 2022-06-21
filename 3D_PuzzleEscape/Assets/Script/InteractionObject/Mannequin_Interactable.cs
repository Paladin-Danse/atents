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
    [SerializeField] private ItemData[] Mannequin_Data;//����ŷ��ǰ�� �� �� �ִ� ��� ������ �����Ͱ�.

    private ItemData[] Mannequin_PartsData;
    

    // Start is called before the first frame update
    private new void Start()
    {
        base.Start();
        Mannequin_PartsData = new ItemData[5];
        InteractionEvent += Mannequin_PartCheck;
    }

    //����ŷ�� ����ϰ� �ִ� ���������� ��ȣ�ۿ��� �� ����ŷ�� ��ǰ���� üũ�ϰ� ����ŷ�� ��ǰ�� �´ٸ� ��� ������ ���ϴ��� Ȯ���ϴ� �Լ�.
    public void Mannequin_PartCheck()
    {
        InventoryItem item = InventoryManager.instance.SelectedItem;
        foreach (ItemData i in Mannequin_Data)
        {
            if(item.data.name == i.Data.name)
            {
                //����ŷ ��ǰ�� �´ٸ� ��� ��ǰ���� �ٽ� üũ string.indexOf() ���.
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
