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
    [SerializeField] private Image InteractUI;
    [SerializeField] private ScrollRect Inventory_Scroll;
    private GameObject Content;
    [SerializeField] private GameObject ItemUI;
    [SerializeField] private GameObject SelectedUI;
    private RectTransform Select;
    private List<ItemUI> itemUIList;

    private void Awake()
    {
        Content = Inventory_Scroll.transform.Find("Viewport/Content").gameObject;
        itemUIList = new List<ItemUI>();
    }
    private void Start()
    {
        if(SelectedUI)
        {
            Select = Instantiate(SelectedUI, Content.transform).GetComponent<RectTransform>();
            Select.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("SelectedUI is Not Found!");
        }
    }


    public void ItemUICreate(InventoryItem m_item)
    {
        var UIObject = Instantiate(ItemUI, Content.transform);
        var UIScript = UIObject.GetComponent<ItemUI>();
        UIScript.item_img.sprite = m_item.data.ItemSprite;
        UIScript.item_name.text = m_item.data.name;

        itemUIList.Add(UIScript);
        UIObject.SetActive(false);
    }

    public void ItemUIEnable(InventoryItem m_item)
    {
        if (m_item.data.Quantity > 0)
        {
            var itemUI = itemUIList.Find(i => i.gameObject.activeSelf == false && i.item_name.text == m_item.data.name);
            if (itemUI)
            {
                itemUI.UIUpdate(m_item.data.ItemSprite, m_item.data.name);
                itemUI.gameObject.SetActive(true);
            }
        }
    }
    public void ItemUIDisable(InventoryItem m_item)
    {
        var itemUI = itemUIList.Find(i => i.item_name.text == m_item.data.name);
        if (itemUI)
        {
            itemUI.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("itemUI is Not Found!");
        }
    }

    public void SelectItemUI(InventoryItem m_item)
    {
        var itemUI = itemUIList.Find(i => i.item_name.text == m_item.data.name);

        if (itemUI)
        {
            //여기에 선택된 UI의 효과연출 입력
            Select.transform.SetParent(itemUI.transform);
            Select.transform.SetAsFirstSibling();

            Select.offsetMin = new Vector2(0, 0);
            Select.offsetMax = new Vector2(0, 0);
            Select.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("itemUI is Not Found!");
        }
    }
    public void SelectUIDisable()
    {
        Select.gameObject.SetActive(false);
    }

    public void OnInteractionUI()
    {
        InteractUI.color = Color.white;
    }

    public void OffInteractionUI()
    {
        InteractUI.color = new Color(1.0f, 1.0f, 1.0f, 100f / 255f);
    }
}
