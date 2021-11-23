using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerAttacks : MonoBehaviour
{
    public enum ATTACK_STATE
    {
        IDLE,
        MELEE,
        EXECUTE,
        AIMING,
        THROW
    }

    [SerializeField] private Gun mainWeapon;
    [SerializeField] private Gun subWeapon;
    [SerializeField] private MeleeWeapon equipMelee;
    [SerializeField] private GameObject grenade;

    public Gun equipGun { get; private set; }
    [SerializeField] private Transform gunPivot;
    [SerializeField] private Transform meleeWeapon;
    [SerializeField] private Transform leftHandMount;
    [SerializeField] private Transform rightHandMount;
    
    private PlayerInput playerInput;
    private Animator playerAnimator;
    private ATTACK_STATE playerAttackState;
    private bool b_OnExecution;
    private Enemy ExecutionTarget;

    private CinemachineVirtualCamera MainWeaponCam;
    private CinemachineVirtualCamera SubWeaponCam;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
        MainWeaponCam = mainWeapon.transform.Find("MainWeapon vcam").GetComponent<CinemachineVirtualCamera>();
        SubWeaponCam = subWeapon.transform.Find("SubWeapon vcam").GetComponent<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        EquipMainWeapon();

        MainWeaponCam.gameObject.SetActive(false);
        SubWeaponCam.gameObject.SetActive(false);
        b_OnExecution = false;
        ExecutionTarget = null;
    }
    private void Update()
    {
        WeaponSwap();
        WeaponAimShot();
        MeleeAttacking();
    }
    public void EquipMainWeapon()
    {
        if (!mainWeapon.gameObject.activeSelf) subWeapon.ReloadCancel();

        mainWeapon.gameObject.SetActive(true);
        subWeapon.gameObject.SetActive(false);

        EquipWeapon(mainWeapon);
    }

    public void EquipSubWeapon()
    {
        if (!subWeapon.gameObject.activeSelf) mainWeapon.ReloadCancel();

        mainWeapon.gameObject.SetActive(false);
        subWeapon.gameObject.SetActive(true);

        EquipWeapon(subWeapon);
    }
    public void WeaponSwap()
    {
        if(playerInput.weaponSwap != 0)
        {
            if (mainWeapon.gameObject.activeSelf)
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
        if (playerInput.aiming && playerAttackState != ATTACK_STATE.MELEE && !playerInput.onRun)
        {
            playerAttackState = ATTACK_STATE.AIMING;

            UIManager.instance.CrosshairEnable();
            
            if (equipGun == mainWeapon) MainWeaponCam.gameObject.SetActive(true);
            else if (equipGun == subWeapon) SubWeaponCam.gameObject.SetActive(true);

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
        
        if((playerInput.Not_aiming && playerAttackState == ATTACK_STATE.AIMING) || playerInput.onRun)
        {
            playerAttackState = ATTACK_STATE.IDLE;

            UIManager.instance.CrosshairDisable();

            if (equipGun == mainWeapon) MainWeaponCam.gameObject.SetActive(false);
            else if (equipGun == subWeapon) SubWeaponCam.gameObject.SetActive(false);
        }

        if(playerInput.reload)
        {
            equipGun.Reload();
        }

        //gunPivot�� ���ڴ�� z���� Ʋ������ ���׸� �ذ��ϱ� ���� ����. rigidbody�� �����ұ ����������, ���������� �����̴� ���� ���� ��� ������.(���ذ�...)
        /*
        if (gunPivot.rotation.z != 0)
            gunPivot.rotation = Quaternion.Euler(gunPivot.rotation.x, gunPivot.rotation.y, 0);
        */
    }

    public void MeleeAttacking()
    {
        if (playerInput.attack_ButtonDown
                && playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Execute") == false
                && b_OnExecution)
        {
            playerAttackState = ATTACK_STATE.EXECUTE;
            if (ExecutionTarget != null)
                ExecutionTarget.Execution();
            else
                Debug.Log("No Target");
            playerAnimator.SetTrigger("Execute");
            UIManager.instance.InteractionExit();
            playerAttackState = ATTACK_STATE.IDLE;
            b_OnExecution = false;
        }
        else if (playerAttackState == ATTACK_STATE.IDLE && playerInput.attack_ButtonDown)
        {
            playerAttackState = ATTACK_STATE.MELEE;

            equipGun.gameObject.SetActive(false);//�������⸦ �ֵθ� ��, �����ϰ� �ִ� ���� ��� ����ִ´�.
            //���� �ִϸ��̼��� �����Ǿ����� �ʾ� ���� ��ġ�� �ٲ㵵 �̻��� ��� �ɰ��̶� ��.
            HandPositioning(equipMelee.LeftHandle, equipMelee.RightHandle);

            playerAnimator.SetTrigger("MeleeAttack");

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
            HandPositioning(equipGun.LeftHandle, equipGun.RightHandle);
        }
    }

    private void HandPositioning(Transform leftHand, Transform rightHand)
    {
        leftHandMount = leftHand;
        rightHandMount = rightHand;
    }
    
    private void OnAnimatorIK(int layerIndex)
    {
        if (playerAttackState == ATTACK_STATE.IDLE || playerAttackState == ATTACK_STATE.AIMING)
        {
            gunPivot.position = playerAnimator.GetIKHintPosition(AvatarIKHint.RightElbow);

            HandAnimatorPosition();
        }
        if(playerAttackState == ATTACK_STATE.MELEE)
        {
            meleeWeapon.position = playerAnimator.GetIKHintPosition(AvatarIKHint.RightElbow);//gunPivot�ȿ� �������⸦ ������ �ȵǴ� ����� ��������ĭ Transform�� �ӽú����� ����غ�.

            HandAnimatorPosition();
        }
    }

    private void HandAnimatorPosition()//��ġ�� ���� �Լ�ȭ
    {
        playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

        playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandMount.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandMount.rotation);

        playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

        playerAnimator.SetIKPosition(AvatarIKGoal.RightHand, rightHandMount.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.RightHand, rightHandMount.rotation);
    }
    //�÷��̾ ���� ó���� ������ ������ ���� ���� ó����ư�� Ȱ��ȭ
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("ExecutionArea"))
        {
            UIManager.instance.InteractionEnter(INTERACTION.EXECUTE);
            ExecutionTarget = other.transform.parent.GetComponent<Enemy>();
        }
    }

    //�÷��̾ ���� ó���� ������ ������ �� ������� ó����ư�� ������ ó���� ����
    private void OnTriggerStay(Collider other)
    {
        var obj = other.gameObject;
        if (obj.name.Equals("ExecutionArea") && obj.activeSelf == true)
        {
            b_OnExecution = true;
        }
    }

    //�÷��̾ ���� ó���� ������ �������� ������ ���� ó����ư�� ��Ȱ��ȭ
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Equals("ExecutionArea"))
        {
            UIManager.instance.InteractionExit();
            playerAttackState = ATTACK_STATE.IDLE;
            ExecutionTarget = null;
            b_OnExecution = false;
        }
    }
    
    private void OnDisable()
    {
        mainWeapon.gameObject.SetActive(false);
        subWeapon.gameObject.SetActive(false);
        equipMelee.gameObject.SetActive(false);
    }

    //���� �ִϸ��̼� �̺�Ʈ
    //�������� �ִϸ��̼��� ���� �� �ִϸ��̼� �̺�Ʈ�� �ҷ��� �Լ�
    public void OnIdle()
    {
        playerAttackState = ATTACK_STATE.IDLE;
        equipGun.gameObject.SetActive(true);
        HandPositioning(equipGun.LeftHandle, equipGun.RightHandle);
    }
}
