using UnityEngine;
using System.Collections;

public class HP_Enable : MonoBehaviour {
    bool Damaged;

    public float HP;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void HPLost(float Atk_point)//공격을 입고 HP를 잃는 함수
    {
        if (Damaged == false)
        {
            HP -= Atk_point;
            //DRF.Red_Flash();//데미지 점멸이펙트
            //HPG.GaugeState(HP);
            //StartCoroutine("Damage_Flash");

            if (HP <= 0)
            {
                if (gameObject.tag == "Monster" || gameObject.tag == "Seedling")
                {
                    Destroy(gameObject.GetComponent<Collider2D>());
                    GetComponent<Animator>().SetTrigger("InDead");
                }
                else
                {
                    Destroy(gameObject);//파괴
                }
            }
        }
    }
}
