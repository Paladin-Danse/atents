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
    Dictionary<int, List<CItem>> dropItemList = new Dictionary<int, List<CItem>>();

    private void Awake()
    {
        for(int i=0; i<dropItem.Length; i++)
        {
            var itemList = new List<CItem>();
            for(int l = 0; l < 1; l++)
            {
                var item = Instantiate(dropItem[i]);
                if (item) itemList.Add(item);
            }

            dropItemList.Add(i, itemList);
        }
    }

    public void MakeItem(CItem m_item)
    {
        //dropItemList[i].Add();
        var item = Instantiate(m_item);
        if (item) dropItemList.Add(item);
    }

    public void DropItem(Vector3 pos)
    {
        //랜덤으로 0에서 1까지의 소수를 가져옴
        float Percentage = Random.Range(0f, 1f);
        
        //만약 랜덤으로 나온 수치가 드랍확률보다 낮다면 드랍확률에 들어온것으로 판정하여 드랍목록에 있는 아이템 중 랜덤으로 하나를 드랍
        if (Percentage < f_DropPercent)
        {
            int itemNum = Random.Range(0, dropItem.Length);

            var tempList = dropItemList[itemNum];
            tempList.Find();


            dropItemList.Find()
            dropItem[itemNum]
            dropItem.SetActive(true);
            dropItem.transform.position = pos;

            //Instantiate(dropItem[Random.Range(0, dropItemList.Length)], pos, Quaternion.identity);
        }
    }
}
