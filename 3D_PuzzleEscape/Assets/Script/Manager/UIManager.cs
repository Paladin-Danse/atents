using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager m_instance;
    public static UIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = GameObject.FindObjectOfType<UIManager>();
            }
            return m_instance;
        }
    }
    [SerializeField] private ScrollRect Inventory_Scroll;
    private GameObject Content;
    [SerializeField] private GameObject ItemUI;
    private Image ItemUIImage;

    private void Start()
    {
        Content = Inventory_Scroll.transform.Find("Content").gameObject;
    }

    public void ItemUIUpdate(List<InventoryItem> m_itemList)
    {
        foreach(var i in m_itemList)
        {
            
        }
    }

}
