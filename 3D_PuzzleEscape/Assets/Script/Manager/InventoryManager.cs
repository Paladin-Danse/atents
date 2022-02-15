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
    [SerializeField] private List<MixItemData> MixRecipe;
    [SerializeField] private List<InventoryItem> InventoryList;
    public InventoryItem SelectedItem { get; private set; }
    public InventoryItem SelectedMixItem { get; private set; }//조합하기 위해 선택된 아이템
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
        SelectedMixItem = null;

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
        if(playerInput.MixKey)
        {
            ItemMixing();
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
                if (selectKey > 0)
                {
                    invenItem = InventoryList.FindLast(i => i.data.Quantity > 0 && i.itemNum < SelectNum);
                }
                else
                {
                    invenItem = InventoryList.Find(i => i.data.Quantity > 0 && i.itemNum > SelectNum); 
                }
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

    public void ItemMixing()
    {
        if (SelectedMixItem != null)
        {
            var mixitem = MixRecipe.Find(i => (i.Data.item1.Data.name == SelectedMixItem.data.name || i.Data.item2.Data.name == SelectedMixItem.data.name) &&
                                              (i.Data.item1.Data.name == SelectedItem.data.name || i.Data.item2.Data.name == SelectedItem.data.name));
            if(mixitem)
            {

                UseItem(SelectedItem);
                UseItem(SelectedMixItem);
                GetItem(mixitem.Data.Mixeditem);
            }
        }
        else
        {
            if (SelectedItem != null)
            {
                SelectedMixItem = SelectedItem;
                UIManager.instance.SelectMixUI();
            }
        }
    }
    public void UseItem(InventoryItem item)
    {
        if (item.data.Durability_Max != 0)
        {
            item.data.Durability--;
            if (item.data.Durability <= 0)
            {
                item.data.Quantity--;
                item.data.Durability = item.data.Durability_Max;
            }
        }
        if (item.data.Quantity <= 0)
        {
            SelectNum = 0;
            UIManager.instance.SelectUIDisable();
            UIManager.instance.ItemUIDisable(item);

            item = null;
        }
    }
}
