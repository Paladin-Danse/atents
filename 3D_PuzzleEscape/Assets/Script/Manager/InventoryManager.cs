using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private static InventoryManager m_instance;
    public static InventoryManager instance
    {
        get
        {
            if(m_instance == null)
            {
                m_instance = GameObject.FindObjectOfType<InventoryManager>();
            }
            return m_instance;
        }
    }
    [SerializeField] private List<ItemData> itemDatas;
    [SerializeField] private List<InventoryItem> InventoryList;
    private InventoryItem SelectedItem;

    private void Start()
    {
        InventoryList = new List<InventoryItem>();

        foreach(var data in itemDatas)
        {
            InventoryItem invenitem = new InventoryItem();
            invenitem.data = data.Data;
            invenitem.data.Quantity = 0;

            InventoryList.Add(invenitem);
        }
    }

    public void GetItem(ItemData item)
    {
        InventoryItem getitem = InventoryList.Find(i => i.data.name == item.Data.name);
        getitem.data.Quantity += item.Data.Quantity;

        UIManager.instance.ItemUIEnable(getitem);
    }

    public void SelectItem()
    {

    }

    public void UseItem()
    {

    }
}
