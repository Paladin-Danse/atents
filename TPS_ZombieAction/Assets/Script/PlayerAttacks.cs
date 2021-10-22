using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    //public MeleeWeapon equipMelee {get; private set; }
    [SerializeField] private Transform gunPivot;
    [SerializeField] private Transform meleeWeapon;
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
        //���� �������� ������ ������ ��� Throwing�Լ��� ȣ��. ������ �� ��� ���� �������� �������� Ÿ�԰��� �� �� ������.
        //if (playerAttackState == ATTACK_STATE.THROW) Throwing();
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

    //�������� �ִϸ��̼��� ���� �� �ִϸ��̼� �̺�Ʈ�� �ҷ��� �Լ�
    public void OnIdle()
    {
        playerAttackState = ATTACK_STATE.IDLE;
        equipGun.gameObject.SetActive(true);
        HandPositioning(equipGun.LeftHandle, equipGun.RightHandle);
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
        }
    }

    //�÷��̾ ���� ó���� ������ ������ �� ������� ó����ư�� ������ ó���� ����
    private void OnTriggerStay(Collider other)
    {
        var obj = other.gameObject;
        if (obj.name.Equals("ExecutionArea") && obj.activeSelf == true)
        {
            playerAttackState = ATTACK_STATE.EXECUTE;

            if(playerInput.attack_ButtonDown
                && playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Execute") == false)
            {
                obj.GetComponentInParent<Enemy>().Execution();
                playerAnimator.SetTrigger("Execute");
                UIManager.instance.InteractionExit();
                playerAttackState = ATTACK_STATE.IDLE;
            }
        }
    }

    //�÷��̾ ���� ó���� ������ �������� ������ ���� ó����ư�� ��Ȱ��ȭ
    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.name.Equals("ExecutionArea"))
        {
            UIManager.instance.InteractionExit();
            playerAttackState = ATTACK_STATE.IDLE;
        }
    }
    
    private void OnDisable()
    {
        mainWeapon.gameObject.SetActive(false);
        subWeapon.gameObject.SetActive(false);
    }
}
