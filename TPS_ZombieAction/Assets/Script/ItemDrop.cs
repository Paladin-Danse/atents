using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    /*
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
    [SerializeField] private Dictionary<CItem, List<CItem>> dropItemList = new Dictionary<CItem, List<CItem>>();//��ųʸ�<CItem(����������), List<CItem>�����ۿ�����Ʈ ����Ʈ>

    private void Awake()
    {
        for(int i=0; i<dropItem.Length; i++)
        {
            var itemList = new List<CItem>();//�ӽø���Ʈ ����

            dropItemList.Add(dropItem[i], itemList);//��ü �����۸���Ʈ<������ ����, ������ �����۸���Ʈ>�� ����

            //MakeItem(dropItem[i]);

            var item = Instantiate(dropItem[i]);//������ ���(dropItem)�� �ִ� �������� �������� �ϳ��� ����
            if (item)
            {
                item.gameObject.SetActive(false);//�����Ҷ� ������ ��� �������� ��Ȱ��ȭ �س��´�.
                itemList.Add(item);//������ �������� �ӽø���Ʈ�� �ְ�
            }
        }
    }

    //Awake�Լ����� �ߺ��Ǵ� �κ��� MakeItem�Լ��� ��ü�Ұ�!
    
    public CItem MakeItem(CItem m_item)
    {
        var item = Instantiate(m_item);//�������� ����
        if(item) dropItemList[m_item].Add(item);//�ش� �����۸���Ʈ�� ��� ������ �������� Add
        item.gameObject.SetActive(false);//��� ������ �������� ��Ȱ��ȭ
        return item;
    }
    
    public void DropItem(Vector3 pos)
    {
        //�������� 0���� 1������ �Ҽ��� ������
        float Percentage = Random.Range(0f, 1f);
        
        //���� �������� ���� ��ġ�� ���Ȯ������ ���ٸ� ���Ȯ���� ���°����� �����Ͽ� �����Ͽ� �ִ� ������ �� �������� �ϳ��� ���
        if (Percentage < f_DropPercent)
        {
            int itemNum = Random.Range(0, dropItem.Length);//������ ������ ������ ����
            var DropItem = dropItem[itemNum];

            var item = dropItemList[DropItem].Find(i => !i.gameObject.activeSelf);
            //���õ� �������� ����(dropItem[itemNum])�� Ű������ ��ü �����۸���Ʈ���� �ش� �����۸���Ʈ�� ������ ��,
            //Find�Լ��� ��Ȱ��ȭ�� ������Ʈ�� ������ �ӽú��� item�� �ִ´�.

            if (item == null)
            {
                item = MakeItem(dropItem[itemNum]);//��Ȱ��ȭ�� �������� ���°�� MakeItem �Լ��� �ҷ��� ���� �����ѵ� �ִ´�.
            }

            item.gameObject.SetActive(true);//������ �������� Ȱ��ȭ
            item.transform.position = pos;//������ ��ġ�� ����Ǿ�� �� ��ġ�� �̵�
        }
    }
    */
}
