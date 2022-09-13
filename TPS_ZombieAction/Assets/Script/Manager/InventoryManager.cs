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
    private bool b_throwCancel;

    private float f_ThrowYSpeed = 0.30f;
    private float f_RotateY = 0;

    [SerializeField] private ThrowItem grenade;
    [SerializeField] private ThrowItem flashbang;
    [SerializeField] private ThrowItem incenbomb;
    private List<ThrowItem> throwItemList = new List<ThrowItem>();

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
        ThrowItem[] throwItems = { grenade, flashbang, incenbomb };

        for (int i = 0; i < throwItems.Length; i++)
        {
            var throwItem = Instantiate(throwItems[i], transform.position, Quaternion.identity);
            if (throwItem)
            {
                throwItem.gameObject.SetActive(false);
                throwItemList.Add(throwItem);
            }
        }

        b_throwCancel = false;
    }

    private void Update()
    {
        if (!playerHealth.b_Dead)
        {
            if (playerInput.rotateY != 0)
            {
                f_RotateY = Mathf.Clamp(f_RotateY + (playerInput.rotateY * f_ThrowYSpeed), -10f, 10f);
            }
            if (playerInput.itemSelect)//����Ű(FŰ)
            {
                //������ ����Ű(FŰ)�� ������ �κ��丮 �����Ͱ� �ƹ��͵� ���ٸ� �Լ��� �����Ű�� ����, �÷��̾ �������� ������ ���߿� �������� ������ �� ����
                if (InventoryItemList.Count <= 0 || playerInput.throwing)
                {
                    return;
                }

                //�κ��丮�� �����ʹ� ������ �������� ���� ������ �Լ��� �����Ű�� ����
                int quantity = 0;
                for (int i = 0; i < InventoryItemList.Count; i++)
                {
                    quantity += InventoryItemList[i].data.quantity;
                }
                if (quantity <= 0) return;
                ChoiceItem();
            }

            if (selectItem != null && selectItem.data.quantity > 0)
            {
                if (playerInput.throwing && b_throwCancel == false && playerAttack.playerAttackState != PlayerAttacks.ATTACK_STATE.EXECUTE)
                {
                    if (selectItem.isThrowing)//��ô�������� ������� ��ô�������� ������ �׸���.
                    {
                        int index = 0;
                        //���̳� ��, ������Ʈ�� �ε����� �� ����� ������
                        RaycastHit hit;

                        var player = playerAttack.gameObject;
                        var mainCam = Camera.main;

                        parabolaRenderer.enabled = true;
                        parabolaRenderer.positionCount = ((int)(maxTime / timeResolution));

                        Vector3 veloVector3 = mainCam.transform.forward * f_ThrowPower;
                        ThrowVector = veloVector3;//�������� �������� �� ������ ����� ���� �˱����� ���̴� ����. ����� ������ �׸��� �־ ���� ���常 ��.

                        Vector3 currentPosition = player.transform.position;
                        //�÷��̾��� ���� �÷��̾� ��ġ���� ������ �Ǿ������Ƿ� �ణ �ø�.
                        currentPosition.y += 1.25f;
                        PlayerThrowItemPosition = currentPosition;//�������� �������� �� ��ô �������� ������ġ�� �˱����� ���̴� ����. �� ThrowVectoró�� ������ �׸��� �ֱ⿡ ���� ����.

                        for (float t = 0.0f; t < maxTime; t += timeResolution)//timeResolution�� ���� ��ǥ�� ��� ���� ��ǥ�� �������� �ð�(���� ������ ���� 0.02f)
                        {
                            //�ε����� ��ġ������ LineRenderer�� �׸��� �����ϱ�
                            if (Physics.Raycast(currentPosition, mainCam.transform.forward, out hit, f_ThrowPower * timeResolution))
                            {
                                parabolaRenderer.SetPosition(index, hit.point);
                                parabolaRenderer.positionCount = index + 1;
                                break;
                            }

                            parabolaRenderer.SetPosition(index, currentPosition);//���� LineRenderer �ε����� ���� �׸� ��ǥ�Է�.

                            currentPosition += veloVector3 * timeResolution;//���� �׸� ���� ��ǥ���� �־���. ���� ��ǥ���� ���� ��ġ�� 0.02�� * ���� ���Ͱ�
                            veloVector3 += Physics.gravity * timeResolution;//���Ͱ��� 0.02�� ���� �߷°��� ��������

                            index++;
                        }
                    }

                    else if (playerInput.itemCheck)//��ô�������� �ƴ϶�� �����ۻ��Ű(GŰ)�� �� �� ������ ���� �������� ���ǰ� �Ѵ�.
                    {
                        selectItem.Use();
                    }

                    if (playerInput.useCancel)
                    {
                        parabolaRenderer.enabled = false;
                        b_throwCancel = true;
                        return;
                    }
                }
                else if (playerInput.itemUse)
                {
                    b_throwCancel = false;
                    if (parabolaRenderer.enabled == true)
                    {
                        parabolaRenderer.enabled = false;
                        selectItem.Use();
                    }
                }
            }
        }
    }
    //��ô�������� ������ �Լ�
    public void Throwing(ThrowItem m_throwItem)
    {
        m_throwItem.transform.position = PlayerThrowItemPosition;
        m_throwItem.gameObject.SetActive(true);

        Rigidbody rigid = m_throwItem.gameObject.GetComponent<Rigidbody>();
        if (rigid)
        {
            rigid.velocity = ThrowVector;
        }
        m_throwItem.Explosion();
    }

    //Loot���� ���ʿ��ϰ� �ߺ��Ǵ� �ڵ带 ���̴� �Լ�
    private void LootItem(UseItem item)
    {
        item.data.quantity++;//��������
        if (selectItem == null)
        {
            selectItem = item;//���� �������� ���
            i_SelectNum = InventoryItemList.FindIndex(i => i == selectItem);
        }
        if (selectItem.data.type == item.data.type)
        {
            UIManager.instance.UpdateInventory(item.data.iconName, item.data.quantity);
        }
    }

    public void Loot(ITEM_TYPE type)
    {
        UseItem item = new UseItem();

        switch(type)
        {
            case ITEM_TYPE.AMMO:
                item = InventoryItemList.Find(i => i.data.type == ITEM_TYPE.AMMO);
                item.Use();
                item = null;//�κ��丮�� ������� �ʴ� �������� �Լ��� ���ܰ����� ó���ϱ� ���� null������ ����ش�.
                break;
            case ITEM_TYPE.POTION:
                item = InventoryItemList.Find(i => i.data.type == ITEM_TYPE.POTION);
                break;
            case ITEM_TYPE.GRENADE:
                item = InventoryItemList.Find(i => i.data.type == ITEM_TYPE.GRENADE);
                break;
            case ITEM_TYPE.FLASHBANG:
                item = InventoryItemList.Find(i => i.data.type == ITEM_TYPE.FLASHBANG);
                break;
            case ITEM_TYPE.INCENDIARY_BOMB:
                item = InventoryItemList.Find(i => i.data.type == ITEM_TYPE.INCENDIARY_BOMB);
                break;
            case ITEM_TYPE.KEY_ITEM:
                item = InventoryItemList.Find(i => i.data.type == ITEM_TYPE.KEY_ITEM);
                item.Use();
                item = null;
                break;
        }
        if (item != null) LootItem(item);
    }

    public void InventoryItemUse(ITEM_TYPE type)
    {
        UseItem item = new UseItem();

        switch (type)
        {
            case ITEM_TYPE.AMMO://ź��
                item = InventoryItemList.Find(i => i.data.type == ITEM_TYPE.AMMO);
                if (item != null)
                {
                    playerAttack.GetAmmo(item.data.value);
                    item = null;
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
                    item.data.quantity--;

                    ThrowItem throwItem = throwItemList.Find(i =>
                    {
                        if (!i.gameObject.activeSelf && i.gameObject.tag.Equals("Grenade"))
                        {
                            return true;
                        }
                        return false;
                    });

                    if (throwItem)
                    {
                        Throwing(throwItem);
                    }
                    else
                    {
                        throwItem = Instantiate(grenade, PlayerThrowItemPosition, Quaternion.identity);
                        throwItemList.Add(throwItem);
                        Throwing(throwItem);
                    }
                    UIManager.instance.UpdateInventory(selectItem.data.iconName, selectItem.data.quantity);
                }
                break;
            case ITEM_TYPE.FLASHBANG:
                item = InventoryItemList.Find(i => i.data.type == ITEM_TYPE.FLASHBANG);
                if (item != null && item.data.quantity > 0)
                {
                    item.data.quantity--;
                    ThrowItem throwItem = throwItemList.Find(i =>
                    {
                        if (!i.gameObject.activeSelf && i.gameObject.tag.Equals("Flashbang"))
                        {
                            return true;
                        }
                        return false;
                    });

                    if (throwItem)
                    {
                        Throwing(throwItem);
                    }
                    else
                    {
                        throwItem = Instantiate(flashbang, PlayerThrowItemPosition, Quaternion.identity);
                        throwItemList.Add(throwItem);
                        Throwing(throwItem);
                    }

                    UIManager.instance.UpdateInventory(selectItem.data.iconName, selectItem.data.quantity);
                }
                break;
            case ITEM_TYPE.INCENDIARY_BOMB:
                item = InventoryItemList.Find(i => i.data.type == ITEM_TYPE.INCENDIARY_BOMB);
                if (item != null)
                {
                    item.data.quantity--;
                    ThrowItem throwItem = throwItemList.Find(i =>
                    {
                        if (!i.gameObject.activeSelf && i.gameObject.tag.Equals("Incendiarybomb"))
                        {
                            return true;
                        }
                        return false;
                    });

                    if (throwItem)
                    {
                        Throwing(throwItem);
                    }
                    else
                    {
                        throwItem = Instantiate(incenbomb, PlayerThrowItemPosition, Quaternion.identity);
                        throwItemList.Add(throwItem);
                        Throwing(throwItem);
                    }

                    UIManager.instance.UpdateInventory(selectItem.data.iconName, selectItem.data.quantity);
                }
                break;
            case ITEM_TYPE.KEY_ITEM:
                item = InventoryItemList.Find(i => i.data.type == ITEM_TYPE.KEY_ITEM);
                if (item != null)
                {
                    UIManager.instance.UpdateGoalCount(item.data.value);
                    item = null;
                }
                break;
        }
        if(item != null)
        {
            if (item.data.quantity <= 0)
            {
                selectItem = null;
                UIManager.instance.UpdateInventory(null, 0);
            }
        }

    }

    public void ChoiceItem()
    {
        if (InventoryItemList.Count != 0)
        {
            do
            {
                //������ �������� �κ��丮�� List������ �Ѿ�� 0���� �ʱ�ȭ �ƴϸ� 1�� ����
                i_SelectNum = (int)Mathf.Repeat(++i_SelectNum, i_SelectNum_Max);
            } while (InventoryItemList[i_SelectNum].data.type == ITEM_TYPE.AMMO
                || InventoryItemList[i_SelectNum].data.quantity <= 0);
            //�Ѿ��� �κ��丮�� ���� �ʴ� ����� ������ �������� �Ѿ��� ������ ���� ���������� �ѱ��.
            //���������� �κ��丮�� ����ִ� �����۵� ������ �� ���� ���´�.

            selectItem = InventoryItemList[i_SelectNum];
        }
        if (selectItem != null)
        {
            UIManager.instance.UpdateInventory(selectItem.data.iconName, selectItem.data.quantity);
        }
    }

    private IEnumerator ThrowBomb(GameObject m_throwItem)
    {
        yield return new WaitForSeconds(3.0f);

        m_throwItem.SetActive(false);
    }
}
