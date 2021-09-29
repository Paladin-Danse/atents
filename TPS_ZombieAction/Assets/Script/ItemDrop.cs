using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    private static ItemDrop m_instance;
    public static ItemDrop instance
    {
        get
        {
            if(m_instance == null)
            {
                m_instance = FindObjectOfType<ItemDrop>();
            }
            return m_instance;
        }
    }
    [SerializeField] private CItem[] dropItem;//드랍되는 아이템 목록
    [SerializeField] [Range(0, 1)] private float f_DropPercent = 0.25f;//드랍확률(0.25 = 25%)
    //private List<CItem> dropItemList;//드랍된 아이템 목록
    [SerializeField] private Dictionary<CItem, List<CItem>> dropItemList = new Dictionary<CItem, List<CItem>>();//딕셔너리<CItem(아이템종류), List<CItem>아이템오브젝트 리스트>

    private void Awake()
    {
        for(int i=0; i<dropItem.Length; i++)
        {
            var itemList = new List<CItem>();//임시리스트 선언
            
            var item = Instantiate(dropItem[i]);//아이템 목록(dropItem)에 있는 아이템을 종류별로 하나씩 생성
            if (item)
            {
                item.gameObject.SetActive(false);//시작할때 생성한 모든 아이템은 비활성화 해놓는다.
                itemList.Add(item);//생성한 아이템을 임시리스트에 넣고
            }
            
            dropItemList.Add(dropItem[i], itemList);//전체 아이템리스트<아이템 종류, 종류별 아이템리스트>에 보관
        }
    }

    //Awake함수에서 중복되는 부분을 MakeItem함수로 대체할것!
    public CItem MakeItem(CItem m_item)
    {
        var item = Instantiate(m_item);
        if(item) dropItemList[m_item].Add(item);
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
            var DropItem = dropItem[itemNum];

            //Debug.Log(dropItem[itemNum].name);
            //Debug.Log(dropItemList[dropItem[itemNum]].Find(i => !i.gameObject.activeSelf));
            var item = dropItemList[DropItem].Find(i => !i.gameObject.activeSelf);
            //선택된 아이템의 종류(dropItem[itemNum])를 키값으로 전체 아이템리스트에서 해당 아이템리스트를 가져온 뒤,
            //Find함수로 비활성화된 오브젝트를 가져와 임시변수 item에 넣는다.

            if (item == null)
            {
                item = MakeItem(dropItem[itemNum]);//비활성화된 아이템이 없는경우 MakeItem 함수를 불러와 새로 생성한뒤 넣는다.
            }

            item.gameObject.SetActive(true);
            item.transform.position = pos;
            
            //Instantiate(dropItem[Random.Range(0, dropItemList.Length)], pos, Quaternion.identity);
        }
    }
}
