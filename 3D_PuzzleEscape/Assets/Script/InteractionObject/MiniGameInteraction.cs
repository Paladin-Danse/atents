using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MiniGameInteraction : InteractionObject
{
    [SerializeField] protected CinemachineVirtualCamera Mini_Cam;
    public bool b_OnMiniGame { get; protected set; }
    protected event Action MiniGameCancel;
    protected PlayerInput playerInput;

    protected new void Start()
    {
        base.Start();
        e_ObjectType = OBJ_TYPE.OBJ_MINIGAME;
        InteractionEvent += MiniGameStart;
        MiniGameCancel += DefaultCancel;
        b_OnMiniGame = false;
        Mini_Cam.gameObject.SetActive(false);
    }
    protected void Update()
    {
        if (playerInput)
        {
            if (b_OnMiniGame && playerInput.ActionCancelKey)
            {
                if (MiniGameCancel != null) MiniGameCancel();
            }
        }
    }

    protected void MiniGameStart()
    {
        if (Mini_Cam && !b_OnMiniGame)
        {
            if (GameManager.instance.playerInput && playerInput == null) playerInput = GameManager.instance.playerInput;
            GameManager.instance.playerInteraction.LockInteraction();
            GameManager.instance.playerMovement.LockMove();
            InventoryManager.instance.LockInventory();
            UIManager.instance.UIDisable();

            b_OnMiniGame = true;
            Mini_Cam.gameObject.SetActive(true);
        }
    }

    protected void MiniGameClear()
    {
        Debug.Log("Clear!!");

        GameManager.instance.playerInteraction.UnlockInteraction();
        GameManager.instance.playerMovement.UnlockMove();
        InventoryManager.instance.UnlockInventory();
        UIManager.instance.UIEnable();

        b_OnMiniGame = false;
        InteractionEvent -= MiniGameStart;
        gameObject.layer = 2;
        Mini_Cam.gameObject.SetActive(false);
    }

    protected void DefaultCancel()
    {
        GameManager.instance.playerInteraction.UnlockInteraction();
        GameManager.instance.playerMovement.UnlockMove();
        InventoryManager.instance.UnlockInventory();
        UIManager.instance.UIEnable();

        b_OnMiniGame = false;
        Mini_Cam.gameObject.SetActive(false);
    }
}
