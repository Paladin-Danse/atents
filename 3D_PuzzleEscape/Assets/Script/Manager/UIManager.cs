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
    private List<GameObject> itemUIList;

    private void Awake()
    {
        Content = Inventory_Scroll.transform.Find("Viewport/Content").gameObject;
    }

    private void Start()
    {
        itemUIList = new List<GameObject>();

        for(int i = 0; i < 3; i++)
        {
            var UIObject = Instantiate(ItemUI, Content.transform);
            UIObject.SetActive(false);
            itemUIList.Add(UIObject);
        }
    }

    public void ItemUIEnable(InventoryItem m_item)
    {
        if (m_item.data.Quantity > 0)
        {
            var itemUI = itemUIList.Find(i => i.activeSelf == false);
            if (itemUI)
            {
                var itemUIscript = itemUI.GetComponent<ItemUI>();
                itemUIscript.UIUpdate(m_item.data.ItemSprite, m_item.data.name);
                itemUI.SetActive(true);
            }
        }
    }
    public void ItemUIDisable(InventoryItem m_item)
    {
        if(m_item.data.Quantity <= 0)
        {
            foreach(var itemUI in itemUIList)
            {
                if(itemUI.GetComponent<ItemUI>().item_name.text == m_item.data.name)
                {
                    itemUI.SetActive(false);
                    break;
                }
            }
        }
    }
}
