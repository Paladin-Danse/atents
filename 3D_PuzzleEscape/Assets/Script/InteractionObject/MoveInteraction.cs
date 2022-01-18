using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInteraction : InteractionObject
{
    // Start is called before the first frame update
    [SerializeField] private Transform a_to_b_Transform;
    [SerializeField] private Transform b_to_a_Transform;
    private Transform TargetTransform;
    private Rigidbody rigid;
    private float t;
    [SerializeField] private float f_MoveTime;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();

        e_ObjectType = OBJ_TYPE.OBJ_INTERACT;
        InteractionEvent += InteractiontoMove;
        TargetTransform = b_to_a_Transform;
        t = 0;
    }

    void FixedUpdate()
    {
        if(transform != TargetTransform)
        {
            t += Time.deltaTime / f_MoveTime;
            rigid.MovePosition(Vector3.Lerp(gameObject.transform.position, TargetTransform.position, t));
            rigid.MoveRotation(Quaternion.Lerp(gameObject.transform.rotation, TargetTransform.rotation, t));
        }
    }

    private void InteractiontoMove()
    {
        if (TargetTransform == a_to_b_Transform)
            TargetTransform = b_to_a_Transform;
        else
            TargetTransform = a_to_b_Transform;

        t = 0;
    }
}
