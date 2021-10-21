using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private string verticalMoveKeyName = "Vertical";
    private string horizontalMoveKeyName = "Horizontal";
    private string rotateXKeyName = "Mouse X";
    private string rotateYKeyName = "Mouse Y";
    private string attackKeyName = "Fire1";
    private string aimingKeyName = "Fire2";
    private string reloadKeyName = "Reload";
    private string mainWeaponSwapKeyName = "MainSwap";
    private string subWeaponSwapKeyName = "SubSwap";
    private string weaponSwapKeyName = "Mouse ScrollWheel";
    private string playerInteractionKeyName = "Interaction";
    private string itemUseKeyName = "ItemUse";
    private string itemSelectKeyName = "ItemSelect";
    private string testKeyName = "Test";
    public float verticalMove { get; private set; }
    public float horizontalMove { get; private set; }
    public float rotateX { get; private set; }
    public float rotateY { get; private set; }
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
    public bool itemUsing { get; private set; }
    public bool throwing { get; private set; }
    public bool useCancel { get; private set; }
    public bool itemSelect { get; private set; }
    public bool test { get; private set; }
    
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        verticalMove = Input.GetAxis(verticalMoveKeyName);
        horizontalMove = Input.GetAxis(horizontalMoveKeyName);
        rotateX = Input.GetAxis(rotateXKeyName);
        rotateY = Input.GetAxis(rotateYKeyName);
        attack_ButtonDown = Input.GetButtonDown(attackKeyName);
        attack_Button = Input.GetButton(attackKeyName);
        aiming = Input.GetButton(aimingKeyName);
        Not_aiming = Input.GetButtonUp(aimingKeyName);
        reload = Input.GetButtonDown(reloadKeyName);
        mainWeaponSwap = Input.GetButtonDown(mainWeaponSwapKeyName);
        subWeaponSwap = Input.GetButtonDown(subWeaponSwapKeyName);
        weaponSwap = Input.GetAxisRaw(weaponSwapKeyName);
        playerInteraction = Input.GetButtonDown(playerInteractionKeyName);
        itemUse = Input.GetButtonDown(itemUseKeyName);
        itemUsing = Input.GetButton(itemUseKeyName);
        throwing = Input.GetButtonUp(itemUseKeyName);
        useCancel = Input.GetButtonDown(aimingKeyName);
        itemSelect = Input.GetButtonDown(itemSelectKeyName);
        test = Input.GetButtonDown(testKeyName);
    }
}
