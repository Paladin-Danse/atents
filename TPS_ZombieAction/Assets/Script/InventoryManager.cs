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

    [SerializeField] private ItemData[] itemDatas;
    private List<UseItem> InventoryItemList = new List<UseItem>();//�κ��丮
    private UseItem selectItem;
    private int i_SelectNum = 0;
    private int i_SelectNum_Max;
    private PlayerAttacks playerAttack;
    private PlayerHealth playerHealth;
    private PlayerInput playerInput;

    private void Start()
    {
        playerAttack = GameManager.instance.playerAttack;
        playerHealth = GameManager.instance.playerHealth;
        playerInput = GameManager.instance.playerInput;

        for(int i = 0; itemDatas.Length > i; i++)
        {
            UseItem data = new UseItem();
            data.data.iconName = itemDatas[i].Data.iconName;
            data.data.type = itemDatas[i].Data.type;
            data.data.value = itemDatas[i].Data.value;
            data.data.quantity = 0;

            data.UseEvent += () => { InventoryItemUse(data.data.type); };

            InventoryItemList.Add(data);
        }
        i_SelectNum_Max = InventoryItemList.Count;
    }
    private void Update()
    {
        if (playerInput.itemUse)//���Ű(GŰ)
        {
            if (selectItem != null)//���õ� �������� Null�� �ƴ϶��
            {
                selectItem.Use();
            }
        }
        if (playerInput.itemSelect)//����Ű(FŰ)
        {
            //������ ����Ű(FŰ)�� ������ �κ��丮�� �ƹ��͵� ���ٸ� �Լ��� �����Ű�� ����
            if (InventoryItemList.Count <= 0)
            {
                return;
            }
            ChoiceItem();
        }
    }

    public void Loot(ITEM_TYPE type)
    {
        UseItem item;

        switch(type)
        {
            case ITEM_TYPE.AMMO :
                InventoryItemUse(type);
                break;
            case ITEM_TYPE.POTION:
                item = InventoryItemList.Find(i => i.data.type == ITEM_TYPE.POTION);
                if (item != null)
                {
                    item.data.quantity++;//��������
                    if (selectItem == null)
                    {
                        selectItem = InventoryItemList.Find(i => i.data.type == ITEM_TYPE.POTION);//���� �������� ���
                    }
                    if (selectItem.data.type == ITEM_TYPE.POTION)
                    {
                        UIManager.instance.UpdateInventory(selectItem.data.iconName, selectItem.data.quantity);
                    }
                }
                break;
            case ITEM_TYPE.GRENADE:
                item = InventoryItemList.Find(i => i.data.type == ITEM_TYPE.GRENADE);
                if(item != null)
                {
                    item.data.quantity++;
                    if(selectItem == null)
                    {
                        selectItem = InventoryItemList.Find(i => i.data.type == ITEM_TYPE.GRENADE);//���� �������� ���
                    }
                    if (selectItem.data.type == ITEM_TYPE.GRENADE)
                    {
                        UIManager.instance.UpdateInventory(selectItem.data.iconName, selectItem.data.quantity);
                    }
                }
                break;
            case ITEM_TYPE.FLASHBANG:
                item = InventoryItemList.Find(i => i.data.type == ITEM_TYPE.FLASHBANG);
                if (item != null)
                {
                    item.data.quantity++;
                }
                break;
            case ITEM_TYPE.INCENDIARY_BOMB:
                item = InventoryItemList.Find(i => i.data.type == ITEM_TYPE.INCENDIARY_BOMB);
                if (item != null)
                {
                    item.data.quantity++;
                }
                break;
        }
    }

    public void InventoryItemUse(ITEM_TYPE type)
    {
        UseItem item;

        switch (type)
        {
            case ITEM_TYPE.AMMO://ź��
                item = InventoryItemList.Find(i => i.data.type == ITEM_TYPE.AMMO);
                if (item != null)
                {
                    playerAttack.GetAmmo(item.data.value);
                }
                break;
            case ITEM_TYPE.POTION://ȸ��������
                item = InventoryItemList.Find(i => i.data.type == ITEM_TYPE.POTION);
                
                //������ ���
                if (item != null && item.data.quantity > 0)//������ ������ 0���� ���ƾ� ����� �� �ִ�.
                {
                    item.data.quantity--;
                    playerHealth.RestoreHealth(item.data.value);
                    UIManager.instance.UpdateInventory(selectItem.data.iconName, selectItem.data.quantity);
                }
                break;
            case ITEM_TYPE.GRENADE:
                item = InventoryItemList.Find(i => i.data.type == ITEM_TYPE.GRENADE);
                if (item != null && item.data.quantity > 0)
                {
                    //�Ŵ������� ���ǿ� ���� �÷��̾� ���� �Լ��� �����Ű�� ���. ������ �� �ѹ� �Ҹ��� �Լ��̱⿡ �ѹ� �Ҹ��� ���� ���������� ����Ұ����� ������� �� �� ������.
                    /*
                    if (playerInput.itemUsing)//�����ۻ��Ű(GŰ)�� ������ ��
                    {
                        playerAttack.ThrowAiming(type);
                        if(playerInput.useCancel)
                        {
                            playerAttack.ThrowCancel(type);
                            break;
                        }
                    }
                    else if (playerInput.throwing)//�����ۻ��Ű(GŰ)�� ���� ��
                    {
                    */
                        item.data.quantity--;
                        //�̰��� ����ź ����ڵ� �Է�
                        UIManager.instance.UpdateInventory(selectItem.data.iconName, selectItem.data.quantity);
                    //}
                }
                break;
            //�Ʒ��δ� ���� �̱��� ������
            case ITEM_TYPE.FLASHBANG:
                item = InventoryItemList.Find(i => i.data.type == ITEM_TYPE.FLASHBANG);
                if (item != null)
                {
                    item.data.quantity--;
                }
                break;
            case ITEM_TYPE.INCENDIARY_BOMB:
                item = InventoryItemList.Find(i => i.data.type == ITEM_TYPE.INCENDIARY_BOMB);
                if (item != null)
                {
                    item.data.quantity--;
                }
                break;
        }
    }

    public void ChoiceItem()
    {
        if (InventoryItemList.Count != 0)
        {
            i_SelectNum = (int)Mathf.Repeat(++i_SelectNum, i_SelectNum_Max);//������ �������� �κ��丮�� List������ �Ѿ�� 0���� �ʱ�ȭ �ƴϸ� 1�� ����
            if (InventoryItemList[i_SelectNum].data.type == ITEM_TYPE.AMMO)//�Ѿ��� �κ��丮�� ���� �ʴ� ����� ������ �������� �Ѿ��� ������ ���� ���������� �ѱ��.
                i_SelectNum = (int)Mathf.Repeat(++i_SelectNum, i_SelectNum_Max);
            
            selectItem = InventoryItemList[i_SelectNum];
        }
        if (selectItem != null)
        {
            UIManager.instance.UpdateInventory(selectItem.data.iconName, selectItem.data.quantity);
        }
    }

    /*
    //[SerializeField] private Image InventoryPanel;//�κ��丮 ������ �̹���
    
    private CItem selectItem;//���� �������� ������
    private int i_SelectNum = 0;//���� ���õ� �������� List�� ��ȣ
    private int i_SelectNum_Max = 3;//�κ��丮 �ִ� �� ����
    private PlayerInput playerInput;//�κ��丮�� ��ȣ�ۿ��ϴ� ����Ű(F)�� ���Ű(G)

    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
    }
    private void Update()
    {
        if (playerInput.itemUse)//���Ű(GŰ)
        {
            if (selectItem)//���õ� �������� Null�� �ƴ϶��
            {
                UseItem();
            }
        }
        if (playerInput.itemSelect)//����Ű(FŰ)
        {
            //������ ����Ű(FŰ)�� ������ �κ��丮�� �ƹ��͵� ���ٸ� �Լ��� �����Ű�� ����
            if (InventoryItemList.Count <= 0)
            {
                return;
            }
            ChoiceItem();
        }
    }

    //�������� �������� �θ��� �Լ�
    public void LootItem(CItem m_item)
    {
        CItem item = InventoryItemList.Find(i => i == m_item);
        //�κ��丮�� ���� �������̶��
        if (item == null)
        {
            //�κ��丮�� �� �� ���
            if(InventoryItemList.Count >= i_SelectNum_Max)
            {
                return;
            }
            m_item.NumUp();
            InventoryItemList.Add(m_item);//���ο� �������� �߰��Ѵ�.
        }
        else
        {
            item.NumUp();
        }
    }
    //�κ��丮 �� �������� ���
    public void UseItem()
    {
        //CItem item = InventoryItemList.Find(i => i == selectItem);
        if (selectItem == null) return ;

        selectItem.Use();

        if (selectItem.Num <= 0)
        {
            InventoryItemList.Remove(selectItem);
            selectItem = null;
            UIManager.instance.InventoryDisable();
        }
    }

    //�������� �����ϴ� �Լ�. ������ ���� Ű(FŰ)�� ���� ��� ���� �������� selectItem�� �����Ѵ�.
    public void ChoiceItem()
    {
        if (InventoryItemList.Count != 0)
        {
            //i_SelectNum++;//���� �������� ����
            i_SelectNum = (int)Mathf.Repeat(++i_SelectNum, i_SelectNum_Max);
            //if (i_SelectNum >= InventoryItemList.Count) i_SelectNum = 0;//������ �������� �κ��丮�� List������ �Ѿ�� 0���� �ʱ�ȭ
            selectItem = InventoryItemList[i_SelectNum];
        }

        UIManager.instance.UpdateInventory(selectItem.InventoryImage, selectItem.Num);
    }
    */
}
