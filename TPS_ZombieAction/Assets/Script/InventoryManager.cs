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
    private LineRenderer parabolaRenderer;

    private Vector3 ThrowVector;
    private Vector3 PlayerThrowItemPosition;
    private float f_ThrowPower = 10.0f;
    private float timeResolution = 0.02f;
    private float maxTime = 10f;

    private float f_ThrowYSpeed = 0.30f;
    private float f_RotateY = 0;

    [SerializeField] private GameObject grenade;
    [SerializeField] private GameObject flashBang;
    [SerializeField] private GameObject incenBomb;
    private List<GameObject> throwItemList = new List<GameObject>();

    private void Awake()
    {
        var player = GameObject.Find("Player");
        playerAttack = player.GetComponent<PlayerAttacks>();
        playerHealth = player.GetComponent<PlayerHealth>();
        playerInput = player.GetComponent<PlayerInput>();
        parabolaRenderer = player.GetComponent<LineRenderer>();
    }

    private void Start()
    {
        //������ �����͸� Inventory Item List�� �����ϰ� �� ������ ��� �� �ڵ��� ������ ó���ϵ��� �̺�Ʈ�� �����Ѵ�.
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

        parabolaRenderer.enabled = false;

        //��ô������ �̸� ���� �� ���߱�
        var throwItem = Instantiate(grenade, transform.position, Quaternion.identity);
        if (throwItem)
        {
            throwItem.SetActive(false);
            throwItemList.Add(throwItem);
        }
    }

    private void Update()
    {
        playerInput = GameManager.instance.playerInput;
        if (playerInput.rotateY != 0)
        {
            f_RotateY = Mathf.Clamp(f_RotateY + (playerInput.rotateY * f_ThrowYSpeed), -10f, 10f);
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

        if (selectItem != null && selectItem.data.quantity > 0)
        {
            if (playerInput.throwing)
            {
                if (selectItem.isThrowing)//��ô�������� ������� ��ô�������� ������ �׸���.
                {
                    int index = 0;
                    //���̳� ��, ������Ʈ�� �ε����� �� ����� ������
                    RaycastHit hit;
                    Vector3 hitposition;

                    var player = playerAttack.gameObject;

                    parabolaRenderer.enabled = true;
                    parabolaRenderer.positionCount = ((int)(maxTime / timeResolution));
                    
                    Vector3 veloVector3 = player.transform.forward * f_ThrowPower;
                    veloVector3.y += f_RotateY;
                    ThrowVector = veloVector3;

                    Vector3 currentPosition = player.transform.position;
                    //�÷��̾��� ���� �÷��̾� ��ġ���� ������ �Ǿ������Ƿ� �ణ �ø�.
                    currentPosition.y += 1.25f;
                    PlayerThrowItemPosition = currentPosition;

                    for (float t = 0.0f; t < maxTime; t += timeResolution)
                    {
                        //�ε����� ��ġ������ ���η������� �׸��� �����ϱ�
                        if (Physics.Raycast(currentPosition, player.transform.forward, out hit, f_ThrowPower * timeResolution))
                        {
                            parabolaRenderer.SetPosition(index, hit.point);
                            parabolaRenderer.positionCount = index + 1;
                            break;
                        }

                        parabolaRenderer.SetPosition(index, currentPosition);//�÷��̾� ��ġ

                        currentPosition += veloVector3 * timeResolution;
                        veloVector3 += Physics.gravity * timeResolution;

                        index++;
                    }
                }
                else if (playerInput.itemCheck)//��ô�������� �ƴ϶�� �����ۻ��Ű(GŰ)�� �� �� ������ ���� �������� ���ǰ� �Ѵ�.
                {
                    selectItem.Use();
                }
                
                if(playerInput.useCancel)
                {
                }

            }
            else if (playerInput.itemUse)
            {
                parabolaRenderer.enabled = false;
                selectItem.Use();
            }
        }

        /*
        if (playerInput.itemUse)//���Ű(GŰ)
        {
            if (selectItem != null)//���õ� �������� Null�� �ƴ϶��
            {
                selectItem.Use();
            }
        }
        */
    }
    //��ô�������� ������ �Լ�
    public void Throwing(GameObject m_throwItem)
    {
        if (m_throwItem)
        {
            m_throwItem.transform.position = PlayerThrowItemPosition;
            m_throwItem.SetActive(true);
        }
        else
        {
            m_throwItem = Instantiate(grenade, PlayerThrowItemPosition, Quaternion.identity);
            throwItemList.Add(m_throwItem);
        }
        
        Rigidbody rigid = m_throwItem.GetComponent<Rigidbody>();
        if (rigid)
        {
            rigid.velocity = ThrowVector;
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

                    GameObject throwItem = throwItemList.Find(i =>
                    {
                        if (!i.activeSelf && i.tag.Equals("Grenade"))
                        {
                            return true;
                        }
                        return false;
                    });
                    Throwing(throwItem);

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
