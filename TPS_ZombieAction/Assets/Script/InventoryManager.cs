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

    [SerializeField] private Image InvnetoryPanel;
    private List<CItem> InventoryItemList;
    
    public void LootItem(CItem item)
    {
        if(InventoryItemList.Find(i => i == item) != null)
        {
            
        }
        else
        {

        }
    }
}
