using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance;
    public static GameManager instance
    {
        get
        {
            if (!m_instance)
                m_instance = FindObjectOfType<GameManager>();
            return m_instance;
        }
    }

    public PlayerAttacks playerAttack { get; private set; }
    public PlayerHealth playerHealth { get; private set; }
    public PlayerInput playerInput { get; private set; }
    public PlayerItemLooting playerItemLooting { get; private set; }
    public PlayerMovement playerMovement { get; private set; }

    [SerializeField] private GetItem[] dropItem;//드랍되는 아이템 목록
    [SerializeField] [Range(0, 1)] private float f_DropPercent = 0.25f;//드랍확률(0.25 = 25%)

    private List<GetItem> dropItemList = new List<GetItem>();//드랍된 아이템 목록

    private void Awake()
    {
        GameObject player = GameObject.Find("Player");
        if (player == null) Debug.Log("player Null");
        else
        {
            playerAttack = player.GetComponent<PlayerAttacks>();
            playerHealth = player.GetComponent<PlayerHealth>();
            playerInput = player.GetComponent<PlayerInput>();
            if (playerInput == null) Debug.Log("playerInput Null");
            playerItemLooting = player.GetComponent<PlayerItemLooting>();
            playerMovement = player.GetComponent<PlayerMovement>();
        }
        for (int i = 0; i < dropItem.Length; i++)
        {
            MakeItem(dropItem[i]);
        }
    }

    public GetItem MakeItem(GetItem m_Item)
    {
        var item = Instantiate(m_Item);
        if (item) dropItemList.Add(item);//해당 아이템리스트에 방금 생성한 아이템을 Add
        item.gameObject.SetActive(false);//방금 생성된 아이템은 비활성화
        return item;
    }

    public void DropItem(Vector3 pos)
    {
        //랜덤으로 0에서 1까지의 소수를 가져옴
        float Percentage = Random.Range(0f, 1f);

        //만약 랜덤으로 나온 수치가 드랍확률보다 낮다면 드랍확률에 들어온것으로 판정하여 드랍목록에 있는 아이템 중 랜덤으로 하나를 드랍
        if (Percentage < f_DropPercent)
        {
            int itemNum = Random.Range(0, dropItem.Length);//랜덤한 아이템 종류를 선택

            var item = dropItemList.Find(i =>
            {
                if (!i.gameObject.activeSelf && i.type.Equals(dropItem[itemNum].type))
                {
                    return true;
                }
                return false;
            });
            //선택된 아이템의 종류(dropItem[itemNum])를 키값으로 전체 아이템리스트에서 해당 아이템리스트를 가져온 뒤,
            //Find함수로 비활성화된 오브젝트를 가져와 임시변수 item에 넣는다.

            if (item == null)
            {
                item = MakeItem(dropItem[itemNum]);//비활성화된 아이템이 없는경우 MakeItem 함수를 불러와 새로 생성한뒤 넣는다.
            }

            item.gameObject.SetActive(true);//가져온 아이템을 활성화
            item.transform.position = pos;//아이템 위치를 드랍되어야 할 위치로 이동
        }
    }
}