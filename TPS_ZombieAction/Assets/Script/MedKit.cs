using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedKit : CItem
{
    [SerializeField] private float f_HealPoint = 20f;
    private PlayerHealth playerHealth;
    public override void Loot(GameObject target)
    {
        //playerHealth = target.GetComponent<PlayerHealth>();//RestoreHealth�Լ��� �θ������� ���� ������.
        InventoryManager.instance.LootItem(this);//�κ��丮�� ������ �߰�
        UIManager.instance.UpdateInventory(InventoryImage, Num);//UI�� �κ��丮 ������Ʈ ��������

        gameObject.SetActive(false);
    }

    public override CItem NewItem()
    {
        return Instantiate(this);
    }

    public override void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }
    public override void Use()
    {
        //PlayerHealth ���� �������µ� Ȯ���� ������ ���� ������ �� ã����.
        //ó���� AmmoPack��ũ��Ʈ���� �����ó�� Loot���� target�� PlayerHealth��ũ��Ʈ�� �޾ƿ��� Use���� RestoreHealth�� �θ��� ����� ������,
        //Use�� �Ҹ��������� �������� MedKit�ν��Ͻ��� PlayerHealth���� Use�� �Ҹ��������� ��� �������� �ϴ°� �´��� Ȯ���� �� ��.
        //playerHealth.RestoreHealth(f_HealPoint);
        
        //�ι�° ������� Use�� ����Ҷ� ��������� �÷��̾ �޾ƿ��� ����� �־���.
        
        //���� ������ ���Ű�� ���� Use�� �θ��� �Լ��� ¥���Ұ����� ����. �п��� �����ϸ� �ش繮������ �ذ��� ��.
        //PlayerHealth playerHealth = ;

        base.Use();
    }
}
