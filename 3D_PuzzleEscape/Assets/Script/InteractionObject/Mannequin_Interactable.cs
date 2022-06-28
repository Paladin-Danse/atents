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
                GameObject Parts;

                //����ŷ ��ǰ�� �´ٸ� ��� ��ǰ���� �ٽ� üũ
                switch (name)
                {
                    case string key when name.Contains("�����Ӹ�"):
                        Parts = Mannequin_Head;
                        break;
                    case string key when name.Contains("���� ��"):
                        Parts = Mannequin_ArmL;
                        break;
                    case string key when name.Contains("������ ��"):
                        Parts = Mannequin_ArmR;
                        break;
                    case string key when name.Contains("���� �ٸ�"):
                        Parts = Mannequin_LegL;
                        break;
                    case string key when name.Contains("������ �ٸ�"):
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
        if (Part.GetComponent<Mannequin_GetParts>())
        {
            ItemData itemData = new ItemData();
            itemData.InputData(data);
            Part.GetComponent<Mannequin_GetParts>().SetItem(itemData);

            //GameManager�� ItemData�� ġȯ�� �� itemData�� M_PartsData�� �Է��Ѵ�.(�κ��丮 ���� �ִ� �������� InventoryItemŸ���̶� ItemData�� ȣȯ�Ұ�).
            GameManager.instance.M_InteractableData_Read(itemData);
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
