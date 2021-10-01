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
    private CItem SelectItem;//���� �������� ������
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
    }
    private void Update()
    {
        if(playerInput.itemUse)
        {
            UseItem(SelectItem);
        }
    }

    public void LootItem(CItem m_item)
    {
        CItem item = InventoryItemList.Find(i => i == m_item);
        //�κ��丮�� ���� �������̶��
        if (item == null)
        {
            m_item.NumUp();
            InventoryItemList.Add(m_item);//���ο� �������� �߰��Ѵ�.
        }
        else
        {
            item.NumUp();
        }
    }
    public void UseItem(CItem m_item)
    {
        CItem item = InventoryItemList.Find(i => i == m_item);
        if (item == null) return;

        item.Use();

    }
}
