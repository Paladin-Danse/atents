using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedKit : CItem
{
    [SerializeField] private float f_HealPoint = 20f;
    private PlayerHealth playerHealth;
    public override void Loot(GameObject target)
    {
        playerHealth = target.GetComponent<PlayerHealth>();//RestoreHealth함수를 부르기위해 값을 가져옴.
        InventoryManager.instance.LootItem(this);//인벤토리에 아이템 추가
        
        gameObject.SetActive(false);
    }

    public override void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }
    public override void Use()
    {
        playerHealth.RestoreHealth(f_HealPoint);
        UIManager.instance.UpdateplayerHealthBar();
        
        base.Use();
    }
}
