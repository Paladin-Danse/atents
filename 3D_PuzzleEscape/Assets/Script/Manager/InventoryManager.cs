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
    public InventoryItem SelectedItem { get; private set; }
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
        SelectedItem = null;

        int i = 0;
        foreach(var data in itemDatas)
        {
            InventoryItem invenitem = new InventoryItem();
            invenitem.itemNum = i++;
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

        if (InventoryList.FindAll(i => i.data.Quantity > 0).Count >= 2)
        {
            if (SelectedItem != null)
            {
                invenItem = InventoryList.Find(i => i.data.Quantity > 0 && selectKey > 0 ? i.itemNum < SelectNum : i.itemNum > SelectNum);
            }
            else
            {
                invenItem = InventoryList.Find(i => i.data.Quantity > 0 && i.itemNum >= SelectNum);
            }
        }
        else if (InventoryList.FindAll(i => i.data.Quantity > 0).Count == 1)
        {
            invenItem = InventoryList.Find(i => i.data.Quantity > 0);
        }
        else
        {
            return;
        }

        if (invenItem != null)
        {
            SelectedItem = invenItem;
            SelectNum = Mathf.Clamp(invenItem.itemNum, 0, itemDatas.Count);
        }

        if (SelectedItem != null)
        {
            UIManager.instance.SelectItemUI(SelectedItem);
        }
        else
        {
            Debug.Log("'SelectItem(float selectKey)' In Error!!");
        }
    }

    public void UseItem()
    {
        if (SelectedItem.data.Durability_Max != 0)
        {
            SelectedItem.data.Durability--;
            if (SelectedItem.data.Durability <= 0)
            {
                SelectedItem.data.Quantity--;
                SelectedItem.data.Durability = SelectedItem.data.Durability_Max;
            }
        }
        if (SelectedItem.data.Quantity <= 0)
        {
            SelectNum = 0;
            UIManager.instance.SelectUIDisable();
            UIManager.instance.ItemUIDisable(SelectedItem);

            SelectedItem = null;
        }
    }
}
