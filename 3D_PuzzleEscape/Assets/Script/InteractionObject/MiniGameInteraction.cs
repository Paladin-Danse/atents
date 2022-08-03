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
    protected AudioSource ClearSoundEffect;
    [SerializeField] AudioClip clear_clip;
    //만약 세이브파일을 불러왔을 때 미니게임의 클리어유무를 저장해야할 경우 사용할 bool변수.
    //public bool b_Clear { get; protected set; }

    protected new void Start()
    {
        base.Start();
        e_ObjectType = OBJ_TYPE.OBJ_MINIGAME;
        InteractionEvent += MiniGameStart;
        MiniGameCancel += DefaultCancel;
        b_OnMiniGame = false;
        Mini_Cam.gameObject.SetActive(false);
        ClearSoundEffect = GetComponent<AudioSource>();
        ClearSoundEffect.clip = clear_clip;
#if UNITY_EDITOR
        if (!ClearSoundEffect.clip) Debug.Log(this.gameObject.name + " Error(MiniGameInteraction) : clear_clip is Not Found");
#endif
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
            GameManager.instance.SetActivePlayer(false);
            GameManager.instance.SetActiveOption(false);
            InventoryManager.instance.LockInventory();
            UIManager.instance.SetUI(false);

            b_OnMiniGame = true;
            Mini_Cam.gameObject.SetActive(true);
        }
    }

    protected void MiniGameClear()
    {
#if UNITY_EDITOR
        Debug.Log("Clear!!");
#endif
        if(ClearSoundEffect.clip)
        {
            ClearSoundEffect.Play();
        }

        GameManager.instance.SetActivePlayer(true);
        GameManager.instance.SetActiveOption(true);
        InventoryManager.instance.UnlockInventory();
        UIManager.instance.SetUI(true);

        b_OnMiniGame = false;
        InteractionEvent -= MiniGameStart;
        gameObject.layer = 2;
        Mini_Cam.gameObject.SetActive(false);
    }
    //ESC버튼을 누르면 미니게임을 도중에 중단하는 함수.
    protected void DefaultCancel()
    {
        GameManager.instance.SetActivePlayer(true);
        InventoryManager.instance.UnlockInventory();
        UIManager.instance.SetUI(true);

        b_OnMiniGame = false;
        Mini_Cam.gameObject.SetActive(false);
    }
}
