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
    [SerializeField] private ItemData[] Mannequin_Data;//����ŷ��ǰ�� �� �� �ִ� ��� ������ �����Ͱ�.
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

    //����ŷ�� ����ϰ� �ִ� ���������� ��ȣ�ۿ��� �� ����ŷ�� ��ǰ���� üũ�ϰ� ����ŷ�� ��ǰ�� �´ٸ� ��� ������ ���ϴ��� Ȯ���ϴ� �Լ�.
    public void Mannequin_PartCheck()
    {
        InventoryItem item = InventoryManager.instance.SelectedItem;
        string name = item.data.name;

        foreach (ItemData i in Mannequin_Data)
        {
            if(name == i.Data.name)
            {
                //����ŷ ��ǰ�� �´ٸ� ��� ��ǰ���� �ٽ� üũ
                if(name.Contains("�����Ӹ�"))
                {
                    Mannequin_fit(Mannequin_Head, item.data, name);
                }
                else if (name.Contains("���� ��"))
                {
                    Mannequin_fit(Mannequin_ArmL, item.data, name);
                }
                else if (name.Contains("������ ��"))
                {
                    Mannequin_fit(Mannequin_ArmR, item.data, name);
                }
                else if (name.Contains("���� �ٸ�"))
                {
                    Mannequin_fit(Mannequin_LegL, item.data, name);
                }
                else if (name.Contains("������ �ٸ�"))
                {
                    Mannequin_fit(Mannequin_LegR, item.data, name);
                }

                InventoryManager.instance.UseItem(item);
            }
        }
    }

    //����ŷ ��ǰ�� Ȱ��ȭ�ϰ� ���� ���� �������� Ȯ���ؼ� �ش� Material�� ������ �Լ�.
    void Mannequin_fit(GameObject Part, Item data, string name)
    {
        Material mat;

        Part.SetActive(true);
        
        if (name.Contains("��"))
            mat = Red_Mat;
        else if(name.Contains("û"))
            mat = Blue_Mat;
        else if(name.Contains("��"))
            mat = Green_Mat;
        else
            mat = Default_Mat;

        //���� Ʋ�� ���� ���������� ��� ��ǰ�� �ٽ� ���� �� �ְ� �ش� ��ǰ�� �ֿ� �� �ִ� ���������� �����Ѵ�.
        if (Part.GetComponent<ItemInteraction>())
        {
            ItemData itemData = new ItemData();
            itemData.InputData(data);
            Part.GetComponent<ItemInteraction>().SetItem(itemData);
        }
        //Material �����.
        if (mat)
        {
            foreach(Renderer iter in Part.GetComponentsInChildren<Renderer>())
            {
                iter.material = mat;
            }
        }
    }
}
