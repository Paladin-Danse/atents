using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedKit : CItem
{
    [SerializeField] private float f_HealPoint = 20f;
    private PlayerHealth playerHealth;
    public override void Loot(GameObject target)
    {
        //playerHealth = target.GetComponent<PlayerHealth>();//RestoreHealth함수를 부르기위해 값을 가져옴.
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
    public override void Use()
    {
        //PlayerHealth 값을 가져오는데 확실히 괜찮다 싶은 정답을 못 찾았음.
        //처음엔 AmmoPack스크립트에서 썼던것처럼 Loot에서 target의 PlayerHealth스크립트를 받아오고 Use에서 RestoreHealth를 부르는 방법을 썼지만,
        //Use가 불리기전까지 여러개의 MedKit인스턴스가 PlayerHealth값을 Use가 불리기전까지 계속 가지도록 하는게 맞는지 확신이 안 섬.
        //playerHealth.RestoreHealth(f_HealPoint);
        
        //두번째 방법으로 Use를 사용할때 멤버변수로 플레이어를 받아오는 방법이 있었음.
        
        //먼저 아이템 사용키를 눌러 Use를 부르는 함수를 짜야할것으로 보임. 학원에 도착하면 해당문제부터 해결할 것.
        //PlayerHealth playerHealth = ;

        base.Use();
    }
}
