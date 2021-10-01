using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedKit : CItem
{
    [SerializeField] private float f_HealPoint = 20f;


    public override void Loot(GameObject target)
    {
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
}
