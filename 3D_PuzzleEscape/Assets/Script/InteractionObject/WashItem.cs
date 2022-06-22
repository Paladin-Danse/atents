using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WashItem : InteractionObject
{
    [SerializeField] private ItemData[] Mannequin_Data;//����ŷ��ǰ�� �� �� �ִ� ��� ������ �����Ͱ�.
    [SerializeField] private ItemData Mannequin_Head_data;
    [SerializeField] private ItemData Mannequin_ArmL_data;
    [SerializeField] private ItemData Mannequin_ArmR_data;
    [SerializeField] private ItemData Mannequin_LegL_data;
    [SerializeField] private ItemData Mannequin_LegR_data;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //����ŷ�� ��ǰ���� üũ�ϰ� ����ŷ�� ��ǰ�� �´ٸ� ��� ������ ���ϴ��� Ȯ���ϴ� �Լ�.
    public void Mannequin_PartCheck()
    {
        InventoryItem item = InventoryManager.instance.SelectedItem;
        string name = item.data.name;
        ItemData data;

        foreach (ItemData i in Mannequin_Data)
        {
            if (name == i.Data.name)
            {
                //����ŷ ��ǰ�� �´ٸ� ��� ��ǰ���� �ٽ� üũ(����ġ������ ���氡���ҵ�?)
                if (name.Contains("�����Ӹ�"))
                {
                    data = Mannequin_Head_data;
                }
                else if (name.Contains("���� ��"))
                {
                    data = Mannequin_ArmL_data;
                }
                else if (name.Contains("������ ��"))
                {
                    data = Mannequin_ArmR_data;
                }
                else if (name.Contains("���� �ٸ�"))
                {
                    data = Mannequin_LegL_data;
                }
                else if (name.Contains("������ �ٸ�"))
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
