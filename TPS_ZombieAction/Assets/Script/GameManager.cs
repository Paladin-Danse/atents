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

    [SerializeField] private GetItem[] dropItem;//����Ǵ� ������ ���
    [SerializeField] [Range(0, 1)] private float f_DropPercent = 0.25f;//���Ȯ��(0.25 = 25%)

    private List<GetItem> dropItemList = new List<GetItem>();//����� ������ ���

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
        if (item) dropItemList.Add(item);//�ش� �����۸���Ʈ�� ��� ������ �������� Add
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

            var item = dropItemList.Find(i =>
            {
                if (!i.gameObject.activeSelf && i.type.Equals(dropItem[itemNum].type))
                {
                    return true;
                }
                return false;
            });
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
}