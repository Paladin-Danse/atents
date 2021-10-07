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

    [SerializeField] ItemData[] itemDatas;
    private List<UseItem> InventoryItemList = new List<UseItem>();//�κ��丮
    PlayerShooter playerShooter;
    PlayerHealth playerHealth;

    private void Start()
    {
        playerShooter = GameManager.instance.playerShooter;
        playerHealth = GameManager.instance.playerHealth;
        for(int i = 0; itemDatas.Length > i; i++)
        {
            UseItem data = new UseItem();
            data.data.iconName = itemDatas[i].Data.iconName;
            data.data.type = itemDatas[i].Data.type;
            data.data.value = itemDatas[i].Data.value;
            data.data.quantity = 0;

            data.UseEvent += (target) => { InventoryItemUse(target, data.data.type); };

            InventoryItemList.Add(data);
        }
    }

    public void Loot(Type type)
    {
        switch(type)
        {
            case Type.AMMO :
                InventoryItemUse(playerShooter.gameObject, type);
                break;
            case Type.POTION:
                InventoryItemList.Find(i => i.data.type == Type.POTION).data.quantity += 1;
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
            default:
                break;
        }
    }

    public void InventoryItemUse(GameObject target, Type type)
    {
        int value;

        switch (type)
        {
            case Type.AMMO://ź��
                value = InventoryItemList.Find(i => i.data.type == Type.AMMO).data.value;
                playerShooter.GetAmmo(value);
                break;
            case Type.POTION://ȸ��������
                InventoryItemList.Find(i => i.data.type == Type.POTION).data.quantity -= 1;
                value = InventoryItemList.Find(i => i.data.type == Type.POTION).data.value;
                playerHealth.RestoreHealth(value);
                break;
                //�Ʒ��δ� ���� �̱���
            case Type.GRANADE:
                InventoryItemList.Find(i => i.data.type == Type.GRANADE).data.quantity += 1;
                break;
            case Type.FLASHBANG:
                InventoryItemList.Find(i => i.data.type == Type.FLASHBANG).data.quantity += 1;
                break;
            case Type.INCENDIARY_BOMB:
                InventoryItemList.Find(i => i.data.type == Type.INCENDIARY_BOMB).data.quantity += 1;
                break;
            default:
                break;
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
