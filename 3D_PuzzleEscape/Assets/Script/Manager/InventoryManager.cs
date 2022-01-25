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
    private int SelectNum;
    private PlayerInput playerInput;
    private void Awake()
    {
        playerInput = GameObject.Find("Player").GetComponent<PlayerInput>();
    }
    private void Start()
    {
        InventoryList = new List<InventoryItem>();
        SelectNum = 0;

        int i = 0;
        foreach(var data in itemDatas)
        {
            InventoryItem invenitem = new InventoryItem();
            invenitem.itemNum = i;
            invenitem.data = data.Data;
            invenitem.data.Quantity = 0;
            UIManager.instance.ItemUICreate(invenitem);

            InventoryList.Add(invenitem);
        }
    }
    private void Update()
    {
        if(playerInput.ItemSelectKey != 0)
        {
            SelectItem(playerInput.ItemSelectKey);
        }
    }


    public void GetItem(ItemData item)
    {
        InventoryItem getitem = InventoryList.Find(i => i.data.name == item.Data.name);
        if (getitem != null)
        {
            getitem.data.Quantity += item.Data.Quantity;
            UIManager.instance.ItemUIEnable(getitem);
        }
        else
            Debug.Log("getitem is Not Found!");
    }

    public void SelectItem(float selectKey)
    {
        InventoryItem invenItem = null;
        invenItem = InventoryList.Find(i => i.data.Quantity > 0 && selectKey > 0 ? i.itemNum < SelectNum : i.itemNum > SelectNum);
        
        if (invenItem != null)
        {
            SelectedItem = invenItem;
            SelectNum = Mathf.Clamp(invenItem.itemNum, 0, itemDatas.Count);
        }
        UIManager.instance.SelectItemUI(SelectedItem);
    }

    public void UseItem()
    {

    }
}
