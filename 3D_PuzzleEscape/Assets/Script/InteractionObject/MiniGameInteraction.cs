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
    protected AudioSource ClearSoundEffect;
    [SerializeField] AudioClip clear_clip;
    //���� ���̺������� �ҷ����� �� �̴ϰ����� Ŭ���������� �����ؾ��� ��� ����� bool����.
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
            InventoryManager.instance.SetInventory(false);
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

        SetMiniInterface(true);
        MiniGameDelete();
    }

    protected void SetMiniInterface(bool setbool)
    {
        GameManager.instance.SetActivePlayer(setbool);
        GameManager.instance.SetActiveOption(setbool);
        InventoryManager.instance.SetInventory(setbool);
        UIManager.instance.SetUI(setbool);
    }

    protected void MiniGameDelete()
    {
        b_OnMiniGame = false;
        InteractionEvent -= MiniGameStart;
        gameObject.layer = 2;
        Mini_Cam.gameObject.SetActive(false);
    }

    //ESC��ư�� ������ �̴ϰ����� ���߿� �ߴ��ϴ� �Լ�.
    protected void DefaultCancel()
    {
        GameManager.instance.SetActivePlayer(true);
        InventoryManager.instance.SetInventory(true);
        UIManager.instance.SetUI(true);

        b_OnMiniGame = false;
        Mini_Cam.gameObject.SetActive(false);
    }
}
