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
        if (playerInput.itemSelect)//선택키(F키)
        {
            //아이템 선택키(F키)를 눌러도 인벤토리에 아무것도 없다면 함수를 실행시키지 않음
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
                if (selectItem.isThrowing)//투척아이템이 맞을경우 투척아이템의 궤적을 그린다.
                {
                    int index = 0;
                    //땅이나 벽, 오브젝트와 부딪혔을 때 사용할 변수들
                    RaycastHit hit;
                    Vector3 hitposition;

                    var player = playerAttack.gameObject;

                    parabolaRenderer.enabled = true;
                    parabolaRenderer.positionCount = ((int)(maxTime / timeResolution));
                    
                    Vector3 veloVector3 = player.transform.forward * f_ThrowPower;
                    veloVector3.y += f_RotateY;
                    ThrowVector = veloVector3;

                    Vector3 currentPosition = player.transform.position;
                    //플레이어의 발이 플레이어 위치값의 기준이 되어있으므로 약간 올림.
                    currentPosition.y += 1.25f;
                    PlayerThrowItemPosition = currentPosition;

                    for (float t = 0.0f; t < maxTime; t += timeResolution)
                    {
                        //부딪히는 위치까지만 라인렌더러를 그리게 수정하기
                        if (Physics.Raycast(currentPosition, player.transform.forward, out hit, f_ThrowPower * timeResolution))
                        {
                            parabolaRenderer.SetPosition(index, hit.point);
                            parabolaRenderer.positionCount = index + 1;
                            break;
                        }

                        parabolaRenderer.SetPosition(index, currentPosition);//플레이어 위치

                        currentPosition += veloVector3 * timeResolution;
                        veloVector3 += Physics.gravity * timeResolution;

                        index++;
                    }
                }
                else if (playerInput.itemCheck)//투척아이템이 아니라면 아이템사용키(G키)가 한 번 눌리는 순간 아이템이 사용되게 한다.
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
        if (playerInput.itemUse)//사용키(G키)
        {
            if (selectItem != null)//선택된 아이템이 Null이 아니라면
            {
                selectItem.Use();
            }
        }
        */
    }
    //투척아이템을 던지는 함수
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
                    item.data.quantity++;//개수증가
                    if (selectItem == null)
                    {
                        selectItem = InventoryItemList.Find(i => i.data.type == ITEM_TYPE.POTION);//먹은 아이템을 장비
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
                        selectItem = InventoryItemList.Find(i => i.data.type == ITEM_TYPE.GRENADE);//먹은 아이템을 장비
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
            case ITEM_TYPE.AMMO://탄약
                item = InventoryItemList.Find(i => i.data.type == ITEM_TYPE.AMMO);
                if (item != null)
                {
                    playerAttack.GetAmmo(item.data.value);
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
                    //매니저에서 조건에 맞춰 플레이어 보고 함수를 실행시키는 방법. 하지만 단 한번 불리는 함수이기에 한번 불리는 동안 던질것인지 취소할것인지 결과값을 낼 수 없었음.
                    /*
                    if (playerInput.itemUsing)//아이템사용키(G키)를 누르는 중
                    {
                        playerAttack.ThrowAiming(type);
                        if(playerInput.useCancel)
                        {
                            playerAttack.ThrowCancel(type);
                            break;
                        }
                    }
                    else if (playerInput.throwing)//아이템사용키(G키)를 뗐을 때
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
            //아래로는 아직 미구현 아이템
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
            i_SelectNum = (int)Mathf.Repeat(++i_SelectNum, i_SelectNum_Max);//선택한 아이템이 인벤토리의 List갯수를 넘어가면 0으로 초기화 아니면 1씩 증가
            if (InventoryItemList[i_SelectNum].data.type == ITEM_TYPE.AMMO)//총알은 인벤토리에 들어가지 않는 관계로 오류를 막기위해 총알이 들어오면 다음 아이템으로 넘긴다.
                i_SelectNum = (int)Mathf.Repeat(++i_SelectNum, i_SelectNum_Max);
            
            selectItem = InventoryItemList[i_SelectNum];
        }
        if (selectItem != null)
        {
            UIManager.instance.UpdateInventory(selectItem.data.iconName, selectItem.data.quantity);
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
