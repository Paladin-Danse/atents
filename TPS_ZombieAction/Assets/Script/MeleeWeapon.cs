using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] private float f_Damage = 20f;
    [SerializeField] private Transform leftHandle;//���� �޼���ġ��
    public Transform LeftHandle { get { return leftHandle; } }//�޼���ġ ȣ��
    [SerializeField] private Transform rightHandle;//���� ��������ġ��
    public Transform RightHandle { get { return rightHandle; } }//��������ġ ȣ��
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
