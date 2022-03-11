using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LookInteraction : InteractionObject
{
    [SerializeField] protected CinemachineVirtualCamera LookCam;
    protected PlayerInput playerInput;
    protected bool b_Lookobj;

    protected new void Start()
    {
        base.Start();
        LookCam.gameObject.SetActive(false);
        InteractionEvent += LookObject;
        b_Lookobj = false;
    }

    protected void LookObject()
    {
        playerInput = GameManager.instance.playerInput;

        LookCam.gameObject.SetActive(true);
        b_Lookobj = true;
        UIManager.instance.SetUI(false);
        GameManager.instance.playerInteraction.LockInteraction();
        GameManager.instance.playerMovement.LockMove();

    }

    protected void Update()
    {
        if(playerInput)
        {
            if(b_Lookobj && playerInput.ActionCancelKey)
            {
                UIManager.instance.SetUI(true);
                GameManager.instance.playerInteraction.UnlockInteraction();
                GameManager.instance.playerMovement.UnlockMove();

                b_Lookobj = false;
                LookCam.gameObject.SetActive(false);
            }
        }
    }
}
