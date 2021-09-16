using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public GameObject mainWeapon;
    public GameObject subWeapon;
    private Gun equipGun;
    public Transform gunPivot;
    public Transform leftHandMount;
    public Transform rightHandMount;

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

        //왼손 오른손 위치 테스트할때만 사용
        /*
        playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandMount.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandMount.rotation);

        playerAnimator.SetIKPosition(AvatarIKGoal.RightHand, rightHandMount.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.RightHand, rightHandMount.rotation);
        */
        
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
