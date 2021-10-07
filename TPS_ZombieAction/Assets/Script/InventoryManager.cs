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
    private List<UseItem> InventoryItemList = new List<UseItem>();//인벤토리
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
            case Type.AMMO://탄약
                value = InventoryItemList.Find(i => i.data.type == Type.AMMO).data.value;
                playerShooter.GetAmmo(value);
                break;
            case Type.POTION://회복아이템
                InventoryItemList.Find(i => i.data.type == Type.POTION).data.quantity -= 1;
                value = InventoryItemList.Find(i => i.data.type == Type.POTION).data.value;
                playerHealth.RestoreHealth(value);
                break;
                //아래로는 아직 미구현
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
    //[SerializeField] private Image InventoryPanel;//인벤토리 아이템 이미지
    
    private CItem selectItem;//현재 선택중인 아이템
    private int i_SelectNum = 0;//현재 선택된 아이템의 List방 번호
    private int i_SelectNum_Max = 3;//인벤토리 최대 방 갯수
    private PlayerInput playerInput;//인벤토리와 상호작용하는 선택키(F)와 사용키(G)

    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
    }
    private void Update()
    {
        if (playerInput.itemUse)//사용키(G키)
        {
            if (selectItem)//선택된 아이템이 Null이 아니라면
            {
                UseItem();
            }
        }
        if (playerInput.itemSelect)//선택키(F키)
        {
            //아이템 선택키(F키)를 눌러도 인벤토리에 아무것도 없다면 함수를 실행시키지 않음
            if (InventoryItemList.Count <= 0)
            {
                return;
            }
            ChoiceItem();
        }
    }

    //아이템이 들어왔을때 부르는 함수
    public void LootItem(CItem m_item)
    {
        CItem item = InventoryItemList.Find(i => i == m_item);
        //인벤토리에 없는 아이템이라면
        if (item == null)
        {
            //인벤토리가 꽉 찬 경우
            if(InventoryItemList.Count >= i_SelectNum_Max)
            {
                return;
            }
            m_item.NumUp();
            InventoryItemList.Add(m_item);//새로운 아이템을 추가한다.
        }
        else
        {
            item.NumUp();
        }
    }
    //인벤토리 내 아이템을 사용
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

    //아이템을 선택하는 함수. 아이템 선택 키(F키)를 누를 경우 다음 아이템을 selectItem에 저장한다.
    public void ChoiceItem()
    {
        if (InventoryItemList.Count != 0)
        {
            //i_SelectNum++;//다음 아이템을 선택
            i_SelectNum = (int)Mathf.Repeat(++i_SelectNum, i_SelectNum_Max);
            //if (i_SelectNum >= InventoryItemList.Count) i_SelectNum = 0;//선택한 아이템이 인벤토리의 List갯수를 넘어가면 0으로 초기화
            selectItem = InventoryItemList[i_SelectNum];
        }

        UIManager.instance.UpdateInventory(selectItem.InventoryImage, selectItem.Num);
    }
    */
}
