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
    [SerializeField] private GameObject MiniGameUI;
    private GameObject Content;
    [SerializeField] private GameObject ItemUI;
    [SerializeField] private GameObject SelectedUI;
    [SerializeField] private GameObject SelectedMixUI;
    private RectTransform Select;
    private RectTransform SelectMix;
    private List<ItemUI> itemUIList;

    private Image GreenPotion_Chk;
    private Image RedPotion_Chk;
    private Image BluePotion_Chk;

    private void Awake()
    {
        Content = Inventory_Scroll.transform.Find("Viewport/Content").gameObject;
        itemUIList = new List<ItemUI>();

        GreenPotion_Chk = MiniGameUI.transform.Find("MixPotionUI").Find("GreenPotion").Find("Empty").GetComponent<Image>();
        RedPotion_Chk = MiniGameUI.transform.Find("MixPotionUI").Find("RedPotion").Find("Empty").GetComponent<Image>();
        BluePotion_Chk = MiniGameUI.transform.Find("MixPotionUI").Find("BluePotion").Find("Empty").GetComponent<Image>();
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

        if (SelectedMixUI)
        {
            SelectMix = Instantiate(SelectedMixUI, Content.transform).GetComponent<RectTransform>();
            SelectMix.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("SelectedMixUI is Not Found!");
        }

        UIEnable();
        Off_MiniUI();
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
            Select.SetParent(itemUI.transform);
            Select.SetAsFirstSibling();

            Select.offsetMin = new Vector2(0, 0);
            Select.offsetMax = new Vector2(0, 0);
            Select.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("itemUI is Not Found!");
        }
        //아이템창의 스크롤바를 조작하는 코드. 스크롤바가 나타날 정도로 많은 아이템이 들어왔을 때만 작동.
        if (Inventory_Scroll.verticalScrollbar.IsActive())
        {
            List<ItemUI> items = itemUIList.FindAll(i => i.gameObject.activeSelf == true);
            float itemScrollValue = 1.0f - ((float)items.FindIndex(i => i == itemUI) / (float)(items.Count - 1));
            Inventory_Scroll.verticalScrollbar.value = itemScrollValue;
        }
    }
    public void SelectUIDisable()
    {
        Select.gameObject.SetActive(false);
    }
    public void SelectMixUI()
    {
        if(Select.gameObject.activeSelf)
        {
            SelectMix.SetParent(Select.parent);
            SelectMix.SetAsFirstSibling();

            SelectMix.offsetMin = new Vector2(0, 0);
            SelectMix.offsetMax = new Vector2(0, 0);
            SelectMix.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Select(RactTransfrom) is Not Active!!");
        }
    }
    public void SelectMixUIDisable()
    {
        SelectMix.gameObject.SetActive(false);
    }

    public void OnInteractionUI()
    {
        InteractUI.color = Color.white;
    }

    public void OffInteractionUI()
    {
        InteractUI.color = new Color(1.0f, 1.0f, 1.0f, 100f / 255f);
    }

    public void UIDisable()
    {
        InteractUI.gameObject.SetActive(false);
        Inventory_Scroll.gameObject.SetActive(false);
    }

    public void UIEnable()
    {
        InteractUI.gameObject.SetActive(true);
        Inventory_Scroll.gameObject.SetActive(true);
    }

    public void On_MiniUI()
    {
        MiniGameUI.SetActive(true);
    }
    public void Off_MiniUI()
    {
        MiniGameUI.SetActive(false);
    }

    public void MixPotionUICheck(string name)
    {
        if(name == "녹색물약")
        {
            GreenPotion_Chk.gameObject.SetActive(false);
        }
        else if(name == "빨간물약")
        {
            RedPotion_Chk.gameObject.SetActive(false);
        }
        else if (name == "파란물약")
        {
            BluePotion_Chk.gameObject.SetActive(false);
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("Error(UIManager.MixPotionUICheck) : The name of that UI could not be found.");
#endif
        }
    }
    public bool MixPotionUIOnOffCheck(string name)
    {
        if(name == "녹색물약")
        {
            return !GreenPotion_Chk.gameObject.activeSelf;
        }
        else if (name == "빨간물약")
        {
            return !RedPotion_Chk.gameObject.activeSelf;
        }
        else if (name == "파란물약")
        {
            return !BluePotion_Chk.gameObject.activeSelf;
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("Error(UIManager.MixPotionUIOnOffCheck) : The name of that UI could not be found.");
#endif
            return false;
        }
    }
}
