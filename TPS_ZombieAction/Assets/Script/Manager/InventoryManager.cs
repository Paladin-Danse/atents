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
    private List<UseItem> InventoryItemList = new List<UseItem>();//인벤토리
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
        //아이템 데이터를 Inventory Item List에 저장하고 각 아이템 사용 시 코딩된 내용을 처리하도록 이벤트를 삽입한다.
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

        //투척아이템 미리 생성 후 감추기
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
            if (playerInput.itemSelect)//선택키(F키)
            {
                //아이템 선택키(F키)를 눌러도 인벤토리 데이터가 아무것도 없다면 함수를 실행시키지 않음, 플레이어가 아이템을 던지는 도중에 아이템을 변경할 수 없음
                if (InventoryItemList.Count <= 0 || playerInput.throwing)
                {
                    return;
                }

                //인벤토리에 데이터는 있지만 아이템이 전혀 없더라도 함수를 실행시키지 않음
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
                    if (selectItem.isThrowing)//투척아이템이 맞을경우 투척아이템의 궤적을 그린다.
                    {
                        int index = 0;
                        //땅이나 벽, 오브젝트와 부딪혔을 때 사용할 변수들
                        RaycastHit hit;

                        var player = playerAttack.gameObject;
                        var mainCam = Camera.main;

                        parabolaRenderer.enabled = true;
                        parabolaRenderer.positionCount = ((int)(maxTime / timeResolution));

                        Vector3 veloVector3 = mainCam.transform.forward * f_ThrowPower;
                        ThrowVector = veloVector3;//아이템이 던져졌을 때 던지는 방향과 힘을 알기위해 쓰이는 변수. 현재는 궤적만 그리고 있어서 값을 저장만 함.

                        Vector3 currentPosition = player.transform.position;
                        //플레이어의 발이 플레이어 위치값의 기준이 되어있으므로 약간 올림.
                        currentPosition.y += 1.25f;
                        PlayerThrowItemPosition = currentPosition;//아이템이 던져졌을 때 투척 아이템의 생성위치를 알기위해 쓰이는 변수. 위 ThrowVector처럼 궤적만 그리고 있기에 값만 저장.

                        for (float t = 0.0f; t < maxTime; t += timeResolution)//timeResolution은 현재 좌표를 찍고 다음 좌표를 찍기까지의 시간(현재 정해진 값은 0.02f)
                        {
                            //부딪히는 위치까지만 LineRenderer를 그리게 수정하기
                            if (Physics.Raycast(currentPosition, mainCam.transform.forward, out hit, f_ThrowPower * timeResolution))
                            {
                                parabolaRenderer.SetPosition(index, hit.point);
                                parabolaRenderer.positionCount = index + 1;
                                break;
                            }

                            parabolaRenderer.SetPosition(index, currentPosition);//현재 LineRenderer 인덱스에 선을 그릴 좌표입력.

                            currentPosition += veloVector3 * timeResolution;//선을 그릴 다음 좌표값을 넣어줌. 다음 좌표값은 현재 위치에 0.02초 * 힘의 벡터값
                            veloVector3 += Physics.gravity * timeResolution;//벡터값에 0.02초 뒤의 중력값이 누적연산

                            index++;
                        }
                    }

                    else if (playerInput.itemCheck)//투척아이템이 아니라면 아이템사용키(G키)가 한 번 눌리는 순간 아이템이 사용되게 한다.
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
    //투척아이템을 던지는 함수
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

    //Loot에서 불필요하게 중복되는 코드를 줄이는 함수
    private void LootItem(UseItem item)
    {
        item.data.quantity++;//개수증가
        if (selectItem == null)
        {
            selectItem = item;//먹은 아이템을 장비
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
                item = null;//인벤토리에 저장되지 않는 아이템은 함수의 예외값으로 처리하기 위해 null값으로 비워준다.
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
            case ITEM_TYPE.AMMO://탄약
                item = InventoryItemList.Find(i => i.data.type == ITEM_TYPE.AMMO);
                if (item != null)
                {
                    playerAttack.GetAmmo(item.data.value);
                    item = null;
                }
                break;
            case ITEM_TYPE.POTION://회복아이템
                item = InventoryItemList.Find(i => i.data.type == ITEM_TYPE.POTION);
                
                //아이템 사용
                if (item != null && item.data.quantity > 0)//아이템 갯수가 0보다 많아야 사용할 수 있다.
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
                //선택한 아이템이 인벤토리의 List갯수를 넘어가면 0으로 초기화 아니면 1씩 증가
                i_SelectNum = (int)Mathf.Repeat(++i_SelectNum, i_SelectNum_Max);
            } while (InventoryItemList[i_SelectNum].data.type == ITEM_TYPE.AMMO
                || InventoryItemList[i_SelectNum].data.quantity <= 0);
            //총알은 인벤토리에 들어가지 않는 관계로 오류를 막기위해 총알이 들어오면 다음 아이템으로 넘긴다.
            //마찬가지로 인벤토리에 비어있는 아이템도 선택할 수 없게 막는다.

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
