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
    private PlayerShooter playerShooter;
    private PlayerHealth playerHealth;
    private PlayerInput playerInput;

    private void Start()
    {
        playerShooter = GameManager.instance.playerShooter;
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

    public void Loot(Type type)
    {
        switch(type)
        {
            case Type.AMMO :
                InventoryItemUse(type);
                break;
            case Type.POTION:
                InventoryItemList.Find(i => i.data.type == Type.POTION).data.quantity += 1;//��������
                if(selectItem == null)
                {
                    selectItem = InventoryItemList.Find(i => i.data.type == Type.POTION);//���� �������� ���
                }
                if (selectItem.data.type == Type.POTION)
                {
                    UIManager.instance.UpdateInventory(selectItem.data.iconName, selectItem.data.quantity);
                }
                break;
            case Type.GRANADE:
                InventoryItemList.Find(i => i.data.type == Type.GRANADE).data.quantity += 1;
                break;
            case Type.FLASHBANG:
                InventoryItemList.Find(i => i.data.type == Type.FLASHBANG).data.quantity += 1;
                break;
            case Type.INCENDIARY_BOMB:
                InventoryItemList.Find(i => i.data.type == Type.INCENDIARY_BOMB).data.quantity += 1;
                break;
        }
    }

    public void InventoryItemUse(Type type)
    {
        UseItem Item;

        switch (type)
        {
            case Type.AMMO://ź��
                Item = InventoryItemList.Find(i => i.data.type == Type.AMMO);
                if (Item != null)
                {
                    playerShooter.GetAmmo(Item.data.value);
                }
                break;
            case Type.POTION://ȸ��������
                Item = InventoryItemList.Find(i => i.data.type == Type.POTION);
                
                //������ ���
                if (Item != null && Item.data.quantity > 0)//������ ������ 0���� ���ƾ� ����� �� �ִ�.
                {
                    Item.data.quantity--;
                    playerHealth.RestoreHealth(Item.data.value);
                    UIManager.instance.UpdateInventory(selectItem.data.iconName, selectItem.data.quantity);
                }
                break;
                //�Ʒ��δ� ���� �̱���
            case Type.GRANADE:                
                InventoryItemList.Find(i => i.data.type == Type.GRANADE).data.quantity -= 1;
                break;
            case Type.FLASHBANG:
                InventoryItemList.Find(i => i.data.type == Type.FLASHBANG).data.quantity -= 1;
                break;
            case Type.INCENDIARY_BOMB:
                InventoryItemList.Find(i => i.data.type == Type.INCENDIARY_BOMB).data.quantity -= 1;
                break;
        }
    }

    public void ChoiceItem()
    {
        if (InventoryItemList.Count != 0)
        {
            i_SelectNum = (int)Mathf.Repeat(++i_SelectNum, i_SelectNum_Max);//������ �������� �κ��丮�� List������ �Ѿ�� 0���� �ʱ�ȭ �ƴϸ� 1�� ����
            if (InventoryItemList[i_SelectNum].data.type == Type.AMMO)//�Ѿ��� �κ��丮�� ���� �ʴ� ����� ������ �������� �Ѿ��� ������ ���� ���������� �ѱ��.
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
