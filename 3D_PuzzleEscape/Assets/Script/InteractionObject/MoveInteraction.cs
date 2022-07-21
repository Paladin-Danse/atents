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

    [SerializeField] protected bool Audio_Add_Clip_Chk;
    [SerializeField] protected AudioClip primary_Clip;
    [SerializeField] protected AudioClip additional_Clip;
    AudioSource Interaction_Sound;


    private void Awake()
    {
        MoveAnimation = GetComponent<Animation>();
        Interaction_Sound = GetComponent<AudioSource>();
    }

    protected new void Start()
    {
        base.Start();

        if (!MoveAnimation) Debug.Log("MoveAnimation is Not Found!!");
        else SetAnim(MoveClip);

        if (!primary_Clip) Debug.LogError(gameObject.ToString() + "Not Found primary_Clip!");
        if (Audio_Add_Clip_Chk && !additional_Clip) Debug.LogError(gameObject.ToString() + "Not Found additional_Clip : please Uncheck 'Audio_Add_Source_Chk' or enter the Clip.");

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
            if(Audio_Add_Clip_Chk)Interaction_Sound.clip = additional_Clip;
            else Interaction_Sound.clip = primary_Clip;
            b_OnMove = false;
        }
        else
        {
            MoveAnimation[MoveAnimation.clip.name].speed = 1.0f;
            Interaction_Sound.clip = primary_Clip;
            b_OnMove = true;
        }
        MoveAnimation.Play();
        if (Interaction_Sound.clip) Interaction_Sound.Play();
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
        gameObject.layer = gameObject.layer = LayerMask.NameToLayer("Default");
        InteractionEvent -= InteractiontoMove;
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