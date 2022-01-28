using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameInteraction : InteractionObject
{
    [SerializeField] protected GameObject MiniGameUI;
    public bool b_OnMiniGame { get; protected set; }

    void Start()
    {
        e_ObjectType = OBJ_TYPE.OBJ_MINIGAME;
        InteractionEvent += MiniGameStart;
        b_OnMiniGame = false;
    }
    private void Update()
    {
        if (b_OnMiniGame)
        {
            if (GameManager.instance.playerInput.ActionCancelKey)
            {
                GameManager.instance.playerInteraction.UnlockInteraction();
                GameManager.instance.playerMovement.UnlockMove();

                b_OnMiniGame = false;
                MiniGameUI.SetActive(false);
            }
        }
    }

    protected void MiniGameStart()
    {
        if (MiniGameUI && !b_OnMiniGame)
        {
            b_OnMiniGame = true;
            GameManager.instance.playerInteraction.LockInteraction();
            GameManager.instance.playerMovement.LockMove();

            MiniGameUI.SetActive(true);
        }
    }
}
