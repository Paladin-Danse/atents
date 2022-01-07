using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] private float f_Damage = 20f;
    //아직 쓰고있지 않은 변수
    //[SerializeField] private float f_SupDamage = 50f;
    [SerializeField] private Transform leftHandle;//실제 왼손위치값
    public Transform LeftHandle { get { return leftHandle; } }//왼손위치 호출
    [SerializeField] private Transform rightHandle;//실제 오른손위치값
    public Transform RightHandle { get { return rightHandle; } }//오른손위치 호출
    
    //[SerializeField] private float f_AttackTime;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Attack(Animator Anim, Action AnimEvent)
    {
        gameObject.SetActive(true);

        StartCoroutine(AttackRoutin(Anim, AnimEvent));
    }

    private IEnumerator AttackRoutin(Animator Anim, Action AnimEvent)
    {
        //GetCurrentAnimatorStateInfo(0 = "Base Movement", 1 = "Upper Body") <= 코드가 아니니 지우지 말것!!
        yield return new WaitUntil(() => (Anim.GetCurrentAnimatorStateInfo(1).IsName("MeleeAttack") == true && Anim.GetCurrentAnimatorStateInfo(1).normalizedTime >= 0.85f));

        AnimEvent();
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            LivingEntity entity = other.gameObject.GetComponent<LivingEntity>();
            if (entity)
            {
                entity.OnDamage(f_Damage, other.transform.position, transform.forward);
            }
        }
    }
}
