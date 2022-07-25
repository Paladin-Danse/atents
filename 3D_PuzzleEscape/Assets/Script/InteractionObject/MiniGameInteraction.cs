using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MiniGameInteraction : InteractionObject
{
    [SerializeField] protected CinemachineVirtualCamera Mini_Cam;
    public bool b_OnMiniGame { get; private set; }
    //원래라면 굳이 미니게임취소 함수를 델리게이트로 쓸 필요는 없을테지만, 물약 미니게임처럼 추가적인 함수(물약UI끄기 등)가 필요한 경우를 위해 델리게이트로 작성함.
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
            GameManager.instance.SetBoolOption(true);
            GameManager.instance.SetActivePlayer(false);
            InventoryManager.instance.LockInventory();
            UIManager.instance.SetUI(false);

            b_OnMiniGame = true;
            Mini_Cam.gameObject.SetActive(true);
        }
    }

    protected void MiniGameClear()
    {
        Debug.Log("Clear!!");

        GameManager.instance.SetActivePlayer(true);
        GameManager.instance.SetBoolOption(false);
        InventoryManager.instance.UnlockInventory();
        UIManager.instance.SetUI(true);

        b_OnMiniGame = false;
        InteractionEvent -= MiniGameStart;
        gameObject.layer = 2;
        Mini_Cam.gameObject.SetActive(false);
    }
    //ESC버튼을 누르면 미니게임을 도중에 중단하는 함수.
    //ESC버튼을 누르면 여길 먼저 거쳐서 플레이어의 On_Move변수를 true로 만들어서 똑같은 ESC버튼에 반응하는 Option함수를 거쳐 옵션창을 킨다...
    protected void DefaultCancel()
    {
        GameManager.instance.SetActivePlayer(true);
        GameManager.instance.SetBoolOption(false);
        InventoryManager.instance.UnlockInventory();
        UIManager.instance.SetUI(true);

        b_OnMiniGame = false;
        Mini_Cam.gameObject.SetActive(false);
    }
}
