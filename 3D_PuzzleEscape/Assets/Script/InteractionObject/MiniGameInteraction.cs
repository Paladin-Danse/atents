using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MiniGameInteraction : InteractionObject
{
    [SerializeField] protected CinemachineVirtualCamera Mini_Cam;
    public bool b_OnMiniGame { get; protected set; }

    void Start()
    {
        e_ObjectType = OBJ_TYPE.OBJ_MINIGAME;
        InteractionEvent += MiniGameStart;
        b_OnMiniGame = false;
        Mini_Cam.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (b_OnMiniGame)
        {
            MiniGameCancel();
        }
    }

    protected void MiniGameStart()
    {
        if (Mini_Cam && !b_OnMiniGame)
        {
            b_OnMiniGame = true;
            GameManager.instance.playerInteraction.LockInteraction();
            GameManager.instance.playerMovement.LockMove();

            Mini_Cam.gameObject.SetActive(true);
        }
    }

    protected void MiniGameClear()
    {

    }

    protected void MiniGameCancel()
    {
        if (GameManager.instance.playerInput.ActionCancelKey)
        {
            GameManager.instance.playerInteraction.UnlockInteraction();
            GameManager.instance.playerMovement.UnlockMove();

            b_OnMiniGame = false;
            Mini_Cam.gameObject.SetActive(false);
        }
    }
}
