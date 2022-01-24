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
    private List<ItemUI> itemUIList;
    private int i_Actived_ItemNum;//Ȱ��ȭ�� ������UI�� �������� ������ ������UI�� UI_Num�� �������� ��.
    private int i_Selected_ItemNum;//���õ� ������UI�� �ѹ�

    private void Awake()
    {
        Content = Inventory_Scroll.transform.Find("Viewport/Content").gameObject;
    }

    private void Start()
    {
        itemUIList = new List<ItemUI>();

        for(int i = 0; i < 3; i++)
        {
            var UIObject = Instantiate(ItemUI, Content.transform);
            UIObject.SetActive(false);
            itemUIList.Add(UIObject.GetComponent<ItemUI>());
        }
        i_Actived_ItemNum = 0;
        i_Selected_ItemNum = 0;
    }

    public void ItemUIEnable(InventoryItem m_item)
    {
        if (m_item.data.Quantity > 0)
        {
            if(itemUIList.Find(i => i.item_name.text == m_item.data.name))
            {
                return;
            }
            
            var itemUI = itemUIList.Find(i => i.gameObject.activeSelf == false);
            if (itemUI)
            {
                itemUI.UIUpdate(m_item.data.ItemSprite, m_item.data.name, ++i_Actived_ItemNum);
                itemUI.gameObject.SetActive(true);
            }
        }
    }
    public void ItemUIDisable(InventoryItem m_item)
    {
        if(m_item.data.Quantity <= 0)
        {
            int DisItemUINum = 0;//Disable�� ������UI�� UI_Num
            foreach(var itemUI in itemUIList)
            {
                if(itemUI.item_name.text == m_item.data.name)
                {
                    DisItemUINum = itemUI.UI_Num;
                    itemUI.gameObject.SetActive(false);
                    i_Actived_ItemNum--;
                    break;
                }
            }
            if (DisItemUINum != 0)
            {
                foreach (var itemUI in itemUIList)
                {
                    itemUI.Pull_Num(DisItemUINum);
                }
            }
        }
    }

    public string SelectItemUI(float selectKey)
    {
        //���⼭���� �̾ �ϸ� ��.
        if(selectKey > 0)
        {
            
        }
        else
        {

        }
        return null;
    }
}
