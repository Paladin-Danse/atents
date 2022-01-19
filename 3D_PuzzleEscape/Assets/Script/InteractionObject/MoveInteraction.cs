using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInteraction : InteractionObject
{
    // Start is called before the first frame update
    private Animation MoveAnimation;
    [SerializeField] private AnimationClip MoveClip;
    private bool b_OnMove;
    [SerializeField] private float f_MoveTime;

    void Start()
    {
        MoveAnimation = GetComponent<Animation>();
        if (!MoveAnimation) Debug.Log("MoveAnimation is Not Found!!");
        else MoveAnimation.clip = MoveClip;

        e_ObjectType = OBJ_TYPE.OBJ_INTERACT;
        InteractionEvent += InteractiontoMove;
        b_OnMove = false;
    }

    private void InteractiontoMove()
    {
        if(b_OnMove)
        {
            MoveAnimation[MoveAnimation.clip.name].speed = -1.0f;
            b_OnMove = false;
        }
        else
        {
            MoveAnimation[MoveAnimation.clip.name].speed = 1.0f;
            b_OnMove = true;
        }

        MoveAnimation.Play();
    }
}
