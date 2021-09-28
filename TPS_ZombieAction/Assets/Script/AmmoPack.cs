using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPack : CItem
{
    [SerializeField] private int i_Ammo = 30;
    public override void Loot(GameObject target)
    {
        PlayerShooter playerShooter = target.GetComponent<PlayerShooter>();

        if(playerShooter != null)
        {
            if (playerShooter.GetAmmo(i_Ammo))
            {
                gameObject.SetActive(false);

                //instantiate�Լ��� �����ϸ鼭 ���� ����.
                //Destroy(gameObject);
            }
        }
    }

    public override void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }
}
