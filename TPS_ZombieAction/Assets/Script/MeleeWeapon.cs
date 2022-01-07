using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] private float f_Damage = 20f;
    //���� �������� ���� ����
    //[SerializeField] private float f_SupDamage = 50f;
    [SerializeField] private Transform leftHandle;//���� �޼���ġ��
    public Transform LeftHandle { get { return leftHandle; } }//�޼���ġ ȣ��
    [SerializeField] private Transform rightHandle;//���� ��������ġ��
    public Transform RightHandle { get { return rightHandle; } }//��������ġ ȣ��
    
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
        //GetCurrentAnimatorStateInfo(0 = "Base Movement", 1 = "Upper Body") <= �ڵ尡 �ƴϴ� ������ ����!!
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
