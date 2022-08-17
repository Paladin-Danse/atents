using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corpse_drawer_Event : InteractionObject
{
    private Animation MoveAnimation;
    private MoveInteraction moveInteraction;
    [SerializeField] private GameObject DropObj;
    [SerializeField] private AnimationClip EventAnimClip;
    private BoxCollider col;
    private ItemInteraction itemInteraction;

    private bool b_OnEvent;

    private void Awake()
    {
        moveInteraction = GetComponent<MoveInteraction>();
        col = DropObj.GetComponent<BoxCollider>();
        itemInteraction = DropObj.GetComponent<ItemInteraction>();

        MoveAnimation = GetComponent<Animation>();

        if (GameManager.instance.mySavedata != null)
        {
            if (GameManager.instance.mySavedata.Mini_3_Clear)
            {
                DropObj.SetActive(false);
                moveInteraction.enabled = true;
                this.enabled = false;
            }
        }
    }
    private new void Start()
    {
        base.Start();

        if (!MoveAnimation) Debug.Log("MoveAnimation is Not Found!!");
        else SetAnim(EventAnimClip);

        SetAnim(EventAnimClip);
        e_ObjectType = OBJ_TYPE.OBJ_INTERACT;
        InteractionEvent += InteractiontoMove;
        b_OnEvent = false;
    }

    private void InteractiontoMove()
    {
        if (NeedItem) NeedItem = null;

        if (!b_OnEvent)
        {
            b_OnEvent = true;
            MoveAnimation.Play();
        }
    }

    private void OnEventEnter()
    {
        col.enabled = true;
        itemInteraction.enabled = true;
        moveInteraction.enabled = true;

        moveInteraction.AnimationReverse();//이미 시체보관함 문이 열린상태라 애니메이션을 뒤에서부터 시작하게 변경.

        this.enabled = false;
    }

    public void SetAnim(AnimationClip clip)
    {
        MoveAnimation.clip = clip;
    }
}
