using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corpse_drawer_Event : MoveInteraction
{
    [SerializeField] private GameObject DropObj;
    [SerializeField] private AnimationClip EventAnimClip;
    private BoxCollider col;
    private ItemInteraction itemInteraction;

    private bool b_OnEvent;
    protected new void Start()
    {
        if (gameObject.layer != LayerMask.NameToLayer("Interactable")) gameObject.layer = LayerMask.NameToLayer("Interactable");

        MoveAnimation = GetComponent<Animation>();
        if (!MoveAnimation) Debug.Log("MoveAnimation is Not Found!!");
        else MoveAnimation.clip = EventAnimClip;

        e_ObjectType = OBJ_TYPE.OBJ_INTERACT;
        InteractionEvent += InteractiontoMove;
        b_OnMove = false;

        col = DropObj.GetComponent<BoxCollider>();
        itemInteraction = DropObj.GetComponent<ItemInteraction>();
        b_OnEvent = false;
}

    protected new void InteractiontoMove()
    {
        if(!b_OnEvent)
        {
            if (NeedItem) NeedItem = null;

            MoveAnimation.Play();
            base.Start();
            b_OnEvent = true;
        }
        else
        {
            base.InteractiontoMove();
        }

    }
    public new void MovePause()
    {
        if (!b_OnEvent)
        {
            if (MoveAnimation[MoveAnimation.clip.name].time >= 1.0f)
            {
                MoveAnimation[MoveAnimation.clip.name].time = 1.0f;

                col.enabled = true;
                itemInteraction.enabled = true;
            }
        }
        else
        {
            base.MovePause();
        }
    }
}
