using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInteraction : InteractionObject
{
    // Start is called before the first frame update
    protected Animation MoveAnimation;
    [SerializeField] protected AnimationClip MoveClip;
    protected bool b_OnMove;
    [SerializeField] protected float f_MoveTime;

    private void Awake()
    {
        MoveAnimation = GetComponent<Animation>();
    }

    protected new void Start()
    {
        base.Start();

        if (!MoveAnimation) Debug.Log("MoveAnimation is Not Found!!");
        else SetAnim(MoveClip);

        e_ObjectType = OBJ_TYPE.OBJ_INTERACT;
        InteractionEvent += InteractiontoMove;
        b_OnMove = false;
    }
    protected void InteractiontoMove()
    {
        if (NeedItem) NeedItem = null;

        if (b_OnMove)
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

    public void MovePause()
    {
        if (MoveAnimation[MoveAnimation.clip.name].time >= 1.0f)
        {
            MoveAnimation[MoveAnimation.clip.name].time = 1.0f;
        }
        else if(MoveAnimation[MoveAnimation.clip.name].time <= 0.0f)
        {
            MoveAnimation[MoveAnimation.clip.name].time = 0.0f;
        }
    }
    public void OneTime_Anim()
    {
        InteractionEvent -= InteractiontoMove;
        gameObject.layer = gameObject.layer = LayerMask.NameToLayer("Default");
    }
    
    public void SetAnim(AnimationClip clip)
    {
        MoveAnimation.clip = clip;
    }

    public void AnimationReverse()
    {
        MoveAnimation[MoveAnimation.clip.name].time = 1.0f;
        MoveAnimation[MoveAnimation.clip.name].speed = 1.0f;
        b_OnMove = true;
    }
}
