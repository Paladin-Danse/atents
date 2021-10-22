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
        //현재 아이템을 던지는 상태일 경우 Throwing함수를 호출. 하지만 이 경우 무슨 아이템을 던지는지 타입값을 줄 수 없었음.
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

            equipGun.gameObject.SetActive(false);//근접무기를 휘두를 때, 장착하고 있는 총은 잠시 집어넣는다.
            //현재 애니메이션이 구현되어있지 않아 손의 위치를 바꿔도 이상한 포즈가 될것이라 함.
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

    //근접공격 애니메이션이 끝날 때 애니메이션 이벤트로 불러올 함수
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
            meleeWeapon.position = playerAnimator.GetIKHintPosition(AvatarIKHint.RightElbow);//gunPivot안에 근접무기를 넣으면 안되는 관계로 근접무기칸 Transform을 임시변수로 사용해봄.

            HandAnimatorPosition();
        }
    }

    private void HandAnimatorPosition()//겹치는 구간 함수화
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
    //플레이어가 적의 처형이 가능한 범위에 들어서는 순간 처형버튼이 활성화
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("ExecutionArea"))
        {
            UIManager.instance.InteractionEnter(INTERACTION.EXECUTE);
        }
    }

    //플레이어가 적의 처형이 가능한 범위에 들어서 있을경우 처형버튼을 누르면 처형을 실행
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

    //플레이어가 적의 처형이 가능한 범위에서 나가는 순간 처형버튼이 비활성화
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
