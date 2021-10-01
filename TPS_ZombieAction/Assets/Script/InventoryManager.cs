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

    //[SerializeField] private Image InventoryPanel;//인벤토리 아이템 이미지
    private List<CItem> InventoryItemList = new List<CItem>();//인벤토리
    private CItem SelectItem;//현재 선택중인 아이템
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
        //인벤토리에 없는 아이템이라면
        if (item == null)
        {
            m_item.NumUp();
            InventoryItemList.Add(m_item);//새로운 아이템을 추가한다.
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
