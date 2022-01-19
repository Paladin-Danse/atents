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

    private List<InventoryItem> InventoryList;
    private InventoryItem SelectedItem;

    private void Start()
    {
        InventoryList = new List<InventoryItem>();
    }

    public void GetItem(ItemData item)
    {
        InventoryItem invenitem = new InventoryItem();

        invenitem.data = item;

        InventoryList.Add(invenitem);
    }

    public void SelectItem()
    {

    }

    public void UseItem()
    {

    }
}
