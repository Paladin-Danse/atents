using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    //플레이어 움직임
    private string verticalMoveKeyName = "Vertical";
    private string horizontalMoveKeyName = "Horizontal";
    private string runMoveKeyName = "Run";
    private string rotateXKeyName = "Mouse X";
    private string rotateYKeyName = "Mouse Y";

    //플레이어 공격 및 조작
    private string attackKeyName = "Fire1";
    private string aimingKeyName = "Fire2";
    private string reloadKeyName = "Reload";
    private string mainWeaponSwapKeyName = "MainSwap";
    private string subWeaponSwapKeyName = "SubSwap";
    private string weaponSwapKeyName = "Mouse ScrollWheel";
    private string playerInteractionKeyName = "Interaction";
    private string itemUseKeyName = "ItemUse";
    private string itemSelectKeyName = "ItemSelect";
    private string dodgeKeyName = "Dodge";

    //디버그
    private string testKeyName = "Test";

    //플레이어 움직임.
    public float verticalMove { get; private set; }
    public float horizontalMove { get; private set; }
    public bool onRun { get; private set; }
    public bool offRun { get; private set; }
    public float rotateX { get; private set; }
    public float rotateY { get; private set; }

    //플레이어 공격 및 조작
    public float weaponSwap { get; private set; }
    public bool attack_ButtonDown { get; private set; }
    public bool attack_Button { get; private set; }
    public bool aiming { get; private set; }
    public bool Not_aiming { get; private set; }
    public bool reload { get; private set; }
    public bool mainWeaponSwap { get; private set; }
    public bool subWeaponSwap { get; private set; }
    public bool playerInteraction { get; private set; }
    public bool itemUse { get; private set; }
    public bool itemCheck { get; private set; }
    public bool throwing { get; private set; }
    public bool useCancel { get; private set; }
    public bool itemSelect { get; private set; }
    public bool dodge { get; private set; }

    //디버그
    public bool test { get; private set; }

    private void Start()
    {
        GameManager.instance.OffCursorVisible();
    }

    private void Update()
    {
        //플레이어 움직임
        verticalMove = Input.GetAxis(verticalMoveKeyName);
        horizontalMove = Input.GetAxis(horizontalMoveKeyName);
        onRun = Input.GetButton(runMoveKeyName);
        offRun = Input.GetButtonUp(runMoveKeyName);
        rotateX = Input.GetAxis(rotateXKeyName);
        rotateY = Input.GetAxis(rotateYKeyName);

        //플레이어 공격 및 조작
        attack_ButtonDown = Input.GetButtonDown(attackKeyName);
        attack_Button = Input.GetButton(attackKeyName);
        aiming = Input.GetButton(aimingKeyName);
        Not_aiming = Input.GetButtonUp(aimingKeyName);
        reload = Input.GetButtonDown(reloadKeyName);
        mainWeaponSwap = Input.GetButtonDown(mainWeaponSwapKeyName);
        subWeaponSwap = Input.GetButtonDown(subWeaponSwapKeyName);
        weaponSwap = Input.GetAxisRaw(weaponSwapKeyName);
        playerInteraction = Input.GetButtonDown(playerInteractionKeyName);
        throwing = Input.GetButton(itemUseKeyName);
        itemCheck = Input.GetButtonDown(itemUseKeyName);
        itemUse = Input.GetButtonUp(itemUseKeyName);
        useCancel = Input.GetButtonDown(aimingKeyName);
        itemSelect = Input.GetButtonDown(itemSelectKeyName);
        dodge = Input.GetButtonDown(dodgeKeyName);

        //디버그
        test = Input.GetButtonDown(testKeyName);
    }
}
