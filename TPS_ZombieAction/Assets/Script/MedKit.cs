using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedKit : CItem
{
    [SerializeField] private float f_HealPoint = 20f;


    public override void Loot(GameObject target)
    {
        InventoryManager.instance.LootItem(this);//인벤토리에 아이템 추가
        UIManager.instance.UpdateInventory(InventoryImage, Num);//UI에 인벤토리 업데이트 정보갱신

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
