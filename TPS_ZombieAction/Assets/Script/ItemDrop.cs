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
    [SerializeField] private CItem[] dropItem;//����Ǵ� ������ ���
    [SerializeField] [Range(0, 1)] private float f_DropPercent = 0.25f;//���Ȯ��(0.25 = 25%)
    //private List<CItem> dropItemList;//����� ������ ���
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
        //�������� 0���� 1������ �Ҽ��� ������
        float Percentage = Random.Range(0f, 1f);
        
        //���� �������� ���� ��ġ�� ���Ȯ������ ���ٸ� ���Ȯ���� ���°����� �����Ͽ� �����Ͽ� �ִ� ������ �� �������� �ϳ��� ���
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
