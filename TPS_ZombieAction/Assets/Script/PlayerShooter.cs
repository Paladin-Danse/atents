using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private GameObject mainWeapon;
    [SerializeField] private GameObject subWeapon;
    private Gun equipGun;
    [SerializeField] private Transform gunPivot;
    [SerializeField] private Transform leftHandMount;
    [SerializeField] private Transform rightHandMount;

    private PlayerInput playerInput;
    private Animator playerAnimator;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();

        EquipMainWeapon();
    }
    private void Update()
    {
        WeaponSwap();
        WeaponAimShot();
    }
    public void EquipMainWeapon()
    {
        mainWeapon.SetActive(true);
        subWeapon.SetActive(false);

        EquipWeapon(mainWeapon);
    }

    public void EquipSubWeapon()
    {
        mainWeapon.SetActive(false);
        subWeapon.SetActive(true);

        EquipWeapon(subWeapon);
    }
    public void WeaponSwap()
    {
        if(playerInput.weaponSwap != 0)
        {
            if(mainWeapon.activeSelf)
            {
                EquipSubWeapon();
            }
            else
            {
                EquipMainWeapon();
            }
        }
        if (playerInput.mainWeaponSwap) EquipMainWeapon();
        if (playerInput.subWeaponSwap) EquipSubWeapon();
    }

    public void WeaponAimShot()
    {
        if(playerInput.aiming)
        {
            Debug.Log("I'm Aiming!");
            if(equipGun.AutoType() == "SEMIAUTO" && playerInput.attack_ButtonDown)
            {
                
                equipGun.Fire();
                Debug.Log("Semi Shot!");
            }

            if(equipGun.AutoType() == "FULLAUTO" && playerInput.attack_Button)
            {
                
                equipGun.Fire();
                Debug.Log("Auto Shot!");
            }
        }
    }

    public void EquipWeapon(GameObject Weapon)
    {
        equipGun = Weapon.transform.Find("Gun").GetComponent<Gun>();

        if (equipGun)
        {
            leftHandMount = equipGun.transform.Find("Left Handle").transform;
            rightHandMount = equipGun.transform.Find("Right Handle").transform;
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        gunPivot.position = playerAnimator.GetIKHintPosition(AvatarIKHint.RightElbow);

        playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

        playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandMount.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandMount.rotation);

        playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

        playerAnimator.SetIKPosition(AvatarIKGoal.RightHand, rightHandMount.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.RightHand, rightHandMount.rotation);
    }

    private void OnDisable()
    {
        mainWeapon.SetActive(false);
        subWeapon.SetActive(false);
    }
}
