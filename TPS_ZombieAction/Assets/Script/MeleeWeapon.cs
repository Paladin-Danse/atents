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
    [SerializeField] private Collider AttackingArea;

    [SerializeField] private float f_AttackTime;

    private void Awake()
    {
        AttackingArea.enabled = false;
    }

    public void Attack()
    {
        StartCoroutine(AttackRoutin());
    }

    IEnumerator AttackRoutin()
    {
        AttackingArea.enabled = true;

        yield return new WaitForSeconds(f_AttackTime);

        AttackingArea.enabled = false;
    }
}
