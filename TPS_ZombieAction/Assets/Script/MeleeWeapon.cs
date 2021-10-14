using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] private float f_Damage = 20f;
    [SerializeField] private Transform leftHandle;//실제 왼손위치값
    public Transform LeftHandle { get { return leftHandle; } }//왼손위치 호출
    [SerializeField] private Transform rightHandle;//실제 오른손위치값
    public Transform RightHandle { get { return rightHandle; } }//오른손위치 호출

    [SerializeField] private float f_AttackTime;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Attack()
    {
        gameObject.SetActive(true);
        StartCoroutine(AttackRoutin());
    }

    private IEnumerator AttackRoutin()
    {
        yield return new WaitForSeconds(f_AttackTime);

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            LivingEntity entity = other.gameObject.GetComponent<LivingEntity>();
            if (entity)
            {
                entity.OnDamage(f_Damage, other.transform.position, other.transform.forward);
            }
        }
    }
}
