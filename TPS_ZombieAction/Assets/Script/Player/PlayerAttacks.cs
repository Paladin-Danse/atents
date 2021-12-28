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
    public ATTACK_STATE playerAttackState { get; private set; }
    private bool b_OnExecution;
    private Enemy ExecutionTarget;

    private CinemachineVirtualCamera MainWeaponCam;
    private CinemachineVirtualCamera SubWeaponCam;

    private void Awake()
    {
        GameManager.instance.Setup();

        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
        MainWeaponCam = mainWeapon.transform.Find("MainWeapon vcam").GetComponent<CinemachineVirtualCamera>();
        SubWeaponCam = subWeapon.transform.Find("SubWeapon vcam").GetComponent<CinemachineVirtualCamera>();
    }

    public void WeaponLoad(GameObject m_MainWeapon, GameObject m_SubWeapon, GameObject m_MeleeWeapon)
    {
        if(!mainWeapon)
        {
            mainWeapon = Instantiate(m_MainWeapon, gunPivot.transform.Find("MainWeapon")).GetComponent<Gun>();
            mainWeapon.gameObject.SetActive(false);//각 무기의 SetActive를 꺼서 OnEnable함수가 불리는 시기를 조절함.
        }
        if(!subWeapon)
        {
            subWeapon = Instantiate(m_SubWeapon, gunPivot.transform.Find("SubWeapon")).GetComponent<Gun>();
            subWeapon.gameObject.SetActive(false);
        }
        if(!equipMelee)
        {
            equipMelee = Instantiate(m_MeleeWeapon, transform.Find("MeleeWeapon")).GetComponent<MeleeWeapon>();
            equipMelee.gameObject.SetActive(false);
            meleeWeapon = equipMelee.gameObject.transform.parent;
        }
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
        if (playerInput.aiming && playerAttackState != ATTACK_STATE.MELEE && playerAttackState != ATTACK_STATE.EXECUTE && !playerInput.onRun)
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
    }

    public IEnumerator ExecuteRoutine(Enemy Target)
    {
        playerAttackState = ATTACK_STATE.EXECUTE;
        equipGun.gameObject.SetActive(false);

        //무적판정 + 움직임봉쇄
        GameManager.instance.playerHealth.OnInvincibility();
        GameManager.instance.playerMovement.OffMove();

        //처형하기전에 처형상대를 바라보기
        Vector3 targetDirection = Target.transform.position - transform.position;
        targetDirection.y = 0f;
        transform.rotation = Quaternion.LookRotation(targetDirection);

        Target.Execution();
        playerAnimator.SetTrigger("Execute");
        UIManager.instance.InteractionExit();

        yield return new WaitUntil(() => (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Execute") == true && playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f));

        playerAttackState = ATTACK_STATE.IDLE;
        equipGun.gameObject.SetActive(true);

        GameManager.instance.playerHealth.OffInvincibility();
        GameManager.instance.playerMovement.OnMove();

        b_OnExecution = false;
    }

    public void MeleeAttacking()
    {
        if (playerInput.attack_ButtonDown
                && playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Execute") == false
                && b_OnExecution)
        {
            //총을 장전중에 처형을 할수도 있기에 재장전 캔슬
            equipGun.ReloadCancel();
            
            if (ExecutionTarget != null)
            {
                StartCoroutine(ExecuteRoutine(ExecutionTarget));
            }
            else
                Debug.Log("No Target");
        }
        else if (playerAttackState == ATTACK_STATE.IDLE && playerInput.attack_ButtonDown)
        {
            //총을 장전중에 근접무기를 들수도 있기에 재장전 캔슬
            equipGun.ReloadCancel();

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
            ExecutionTarget = other.transform.parent.GetComponent<Enemy>();
        }
    }

    //플레이어가 적의 처형이 가능한 범위에 들어서 있을경우 처형버튼을 누르면 처형을 실행
    private void OnTriggerStay(Collider other)
    {
        var obj = other.gameObject;
        if (obj.name.Equals("ExecutionArea") && obj.activeSelf == true && b_OnExecution == false)
        {
            b_OnExecution = true;
            if(ExecutionTarget == null)
            {
                ExecutionTarget = other.transform.parent.GetComponent<Enemy>();
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

    //이하 애니메이션 이벤트
    //근접공격 애니메이션이 끝날 때 애니메이션 이벤트로 불러올 함수
    public void OnIdle()
    {
        playerAttackState = ATTACK_STATE.IDLE;
        equipGun.gameObject.SetActive(true);
        HandPositioning(equipGun.LeftHandle, equipGun.RightHandle);
    }
}
