using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MiniGameInteraction : InteractionObject
{
    [SerializeField] protected CinemachineVirtualCamera Mini_Cam;
    public bool b_OnMiniGame { get; private set; }
    //������� ���� �̴ϰ������ �Լ��� ��������Ʈ�� �� �ʿ�� ����������, ���� �̴ϰ���ó�� �߰����� �Լ�(����UI���� ��)�� �ʿ��� ��츦 ���� ��������Ʈ�� �ۼ���.
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
    //ESC��ư�� ������ �̴ϰ����� ���߿� �ߴ��ϴ� �Լ�.
    //ESC��ư�� ������ ���� ���� ���ļ� �÷��̾��� On_Move������ true�� ���� �Ȱ��� ESC��ư�� �����ϴ� Option�Լ��� ���� �ɼ�â�� Ų��...
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
