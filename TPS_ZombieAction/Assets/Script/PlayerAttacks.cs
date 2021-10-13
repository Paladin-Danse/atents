using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ATTACK_STATE
{
    IDLE,
    MELEE,
    AIMING
}

public class PlayerAttacks : MonoBehaviour
{
    [SerializeField] private Gun mainWeapon;
    [SerializeField] private Gun subWeapon;
    public Gun equipGun { get; private set; }
    public MeleeWeapon equipMelee {get; private set; }
    [SerializeField] private Transform gunPivot;
    [SerializeField] private Transform leftHandMount;
    [SerializeField] private Transform rightHandMount;

    private PlayerInput playerInput;
    private Animator playerAnimator;
    private ATTACK_STATE playerAttackState;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        EquipMainWeapon();
    }
    private void Update()
    {
        WeaponSwap();
        WeaponAimShot();
        MeleeAttacking();
    }
    public void EquipMainWeapon()
    {
        mainWeapon.gameObject.SetActive(true);
        subWeapon.gameObject.SetActive(false);

        EquipWeapon(mainWeapon);
    }

    public void EquipSubWeapon()
    {
        mainWeapon.gameObject.SetActive(false);
        subWeapon.gameObject.SetActive(true);

        EquipWeapon(subWeapon);
    }
    public void WeaponSwap()
    {
        if(playerInput.weaponSwap != 0)
        {
            if(mainWeapon.gameObject.activeSelf)
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
        if(playerInput.aiming && playerAttackState != ATTACK_STATE.MELEE)
        {
            playerAttackState = ATTACK_STATE.AIMING;
            if (equipGun.AutoType() == "SEMIAUTO"
                && playerInput.attack_ButtonDown
                && playerAttackState == ATTACK_STATE.AIMING)
            {
                equipGun.Fire();
            }

            if(equipGun.AutoType() == "FULLAUTO"
                && playerInput.attack_Button
                && playerAttackState == ATTACK_STATE.AIMING)
            {
                equipGun.Fire();
            }
        }
        
        if(playerInput.Not_aiming && playerAttackState == ATTACK_STATE.AIMING)
        {
            playerAttackState = ATTACK_STATE.IDLE;
        }

        if(playerInput.reload)
        {
            equipGun.Reload();
        }
    }
    public void MeleeAttacking()
    {
        if (playerAttackState == ATTACK_STATE.IDLE && playerInput.attack_ButtonDown)
        {
            //playerAttackState = ATTACK_STATE.MELEE;
            equipMelee.Attack();
        }
    }

    public bool GetAmmo(int newAmmo)
    {
        if (mainWeapon.Ammo_Limit() && subWeapon.Ammo_Limit()) return false;

        mainWeapon.GetAmmo(newAmmo);
        subWeapon.GetAmmo(newAmmo);

        return true;
    }

    public void EquipWeapon(Gun Weapon)
    {
        equipGun = Weapon;

        if (equipGun)
        {
            leftHandMount = equipGun.LeftHandle;
            rightHandMount = equipGun.RightHandle;
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
        mainWeapon.gameObject.SetActive(false);
        subWeapon.gameObject.SetActive(false);
    }
}
