using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryManager : MonoBehaviour
{
    private static InventoryManager m_instance;
    public static InventoryManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<InventoryManager>();
            }
            return m_instance;
        }
    }

    //[SerializeField] private Image InventoryPanel;//�κ��丮 ������ �̹���
    private List<CItem> InventoryItemList = new List<CItem>();//�κ��丮
    private CItem selectItem;//���� �������� ������
    private int i_SelectNum = 0;//���� ���õ� �������� List�� ��ȣ
    private int i_SelectNum_Max = 3;//�κ��丮 �ִ� �� ����
    private PlayerInput playerInput;//�κ��丮�� ��ȣ�ۿ��ϴ� ����Ű(F)�� ���Ű(G)

    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
    }
    private void Update()
    {
        if (playerInput.itemUse)//���Ű(GŰ)
        {
            if (selectItem)//���õ� �������� Null�� �ƴ϶��
            {
                UseItem();
            }
        }
        if (playerInput.itemSelect)//����Ű(FŰ)
        {
            //������ ����Ű(FŰ)�� ������ �κ��丮�� �ƹ��͵� ���ٸ� �Լ��� �����Ű�� ����
            if (InventoryItemList.Count <= 0)
            {
                return;
            }
            ChoiceItem();
        }
    }

    //�������� �������� �θ��� �Լ�
    public void LootItem(CItem m_item)
    {
        CItem item = InventoryItemList.Find(i => i == m_item);
        //�κ��丮�� ���� �������̶��
        if (item == null)
        {
            //�κ��丮�� �� �� ���
            if(InventoryItemList.Count >= i_SelectNum_Max)
            {
                return;
            }
            m_item.NumUp();
            InventoryItemList.Add(m_item);//���ο� �������� �߰��Ѵ�.
        }
        else
        {
            item.NumUp();
        }
    }
    //�κ��丮 �� �������� ���
    public void UseItem()
    {
        //CItem item = InventoryItemList.Find(i => i == selectItem);
        if (selectItem == null) return ;

        selectItem.Use();

        if (selectItem.Num <= 0)
        {
            InventoryItemList.Remove(selectItem);
            selectItem = null;
            UIManager.instance.InventoryDisable();
        }
    }

    //�������� �����ϴ� �Լ�. ������ ���� Ű(FŰ)�� ���� ��� ���� �������� selectItem�� �����Ѵ�.
    public void ChoiceItem()
    {
        if (InventoryItemList.Count != 0)
        {
            //i_SelectNum++;//���� �������� ����
            i_SelectNum = (int)Mathf.Repeat(++i_SelectNum, i_SelectNum_Max);
            //if (i_SelectNum >= InventoryItemList.Count) i_SelectNum = 0;//������ �������� �κ��丮�� List������ �Ѿ�� 0���� �ʱ�ȭ
            selectItem = InventoryItemList[i_SelectNum];
        }

        UIManager.instance.UpdateInventory(selectItem.InventoryImage, selectItem.Num);
    }
}
