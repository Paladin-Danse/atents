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
    [SerializeField] private List<MixData> MixRecipe;
    [SerializeField] private List<InventoryItem> InventoryList;
    public InventoryItem SelectedItem { get; private set; }
    public InventoryItem SelectedMixItem { get; private set; }//조합하기 위해 선택된 아이템
    private int SelectNum;
    private PlayerInput playerInput;
    private bool b_OnInventoryInput;
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
        b_OnInventoryInput = true;

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
        //디버그용 아이템 획득 코드
        //GetItem(itemDatas.Find(i => i.Data.name == "금속용매"));
    }
    private void Update()
    {
        if (b_OnInventoryInput)
        {
            InventoryInput();
        }
    }
    private void InventoryInput()
    {
        if (playerInput.ItemSelectKey != 0)
        {
            SelectItem(playerInput.ItemSelectKey);
        }
        if (playerInput.MixKey)
        {
            ItemMixing();
        }
        if (playerInput.ItemDescriptionKey)
        {
            Show_ItemDescription();
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
            if(mixitem && SelectedMixItem != SelectedItem)
            {
                UseItem(SelectedItem);
                UseItem(SelectedMixItem);
                GetItem(mixitem.Data.Mixeditem);
            }
            else
            {
                SelectedMixItem = null;
                UIManager.instance.SelectMixUIDisable();
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
    public void LostItem(string itemname)
    {
        var item = InventoryList.Find(i => i.data.name == itemname);

        if (item != null)
        {
            if (item.data.Quantity >= 0)
            {
                item.data.Quantity = 0;
                item.data.Durability = item.data.Durability_Max;

                SelectNum = 0;
                UIManager.instance.SelectUIDisable();
                UIManager.instance.ItemUIDisable(item);

                item = null;
            }
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("'LostItem(string itemname)' In Error : Not Found item!");
#endif
        }
    }

    public void Show_ItemDescription()
    {
        if(SelectedItem != null)
        {
            

#if UNITY_EDITOR
            Debug.Log(SelectedItem.data.Description);
#endif
        }
    }

    public void LockInventory()
    {
        b_OnInventoryInput = false;
    }
    public void UnlockInventory()
    {
        b_OnInventoryInput = true;
    }

    public InventoryItem InventoryitemCheck(ItemData data)
    {
        if (data != null)
        {
            var item = InventoryList.Find(i => i.data.Quantity > 0 && i.data.name == data.Data.name);
            return item;
        }
        else
            return null;
    }
}
