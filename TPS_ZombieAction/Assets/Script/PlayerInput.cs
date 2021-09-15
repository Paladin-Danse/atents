using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string verticalMoveKeyName = "Vertical";
    public string horizontalMoveKeyName = "Horizontal";
    public string rotateXKeyName = "Mouse X";
    public string rotateYKeyName = "Mouse Y";
    public string attackKeyName = "Fire1";
    public string aimingKeyName = "Fire2";
    public string mainWeaponSwapKeyName = "MainSwap";
    public string subWeaponSwapKeyName = "SubSwap";
    public string weaponSwapKeyName = "Mouse ScrollWheel";
    public float verticalMove { get; private set; }
    public float horizontalMove { get; private set; }
    public float rotateX { get; private set; }
    public float rotateY { get; private set; }
    public float weaponSwap { get; private set; }

    public bool attack { get; private set; }
    public bool aiming { get; private set; }
    public bool mainWeaponSwap { get; private set; }
    public bool subWeaponSwap { get; private set; }
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
        attack = Input.GetButtonDown(attackKeyName);
        aiming = Input.GetButton(aimingKeyName);
        mainWeaponSwap = Input.GetButtonDown(mainWeaponSwapKeyName);
        subWeaponSwap = Input.GetButtonDown(subWeaponSwapKeyName);
        weaponSwap = Input.GetAxisRaw(weaponSwapKeyName);
    }
}
