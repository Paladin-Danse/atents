using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput playerInput;
    private Rigidbody playerRigid;
    private Animator playerAnimator;
    private float f_moveSpeed;
    private bool b_move;

    //회피
    private bool b_Dodge;
    private Vector3 DodgeVector;
    [SerializeField] private float f_DodgeDistance = 5f;

    [SerializeField] private float f_walkSpeed = 1.5f;
    //아직 쓰고있지 않은 변수
    //[SerializeField] private float f_aimMoveSpeed = 1.0f;
    [SerializeField] private float f_runSpeed = 3.0f;
    [SerializeField] private float f_rotateSpeed = 1.0f;
    [SerializeField] private GameObject gunPivot;
    [SerializeField] private float f_highCamRotation = -60f;//카메라 윗방향 제한
    [SerializeField] private float f_lowCamRotation = 60f;//카메라 아랫방향 제한

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigid = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        f_moveSpeed = f_walkSpeed;
        b_move = true;
        b_Dodge = false;
    }

    private void Start()
    {
        //가끔 gunPivot의 rotation이 엉뚱한 각도에서 시작해서 총이 뒤를 보는 바람에 플레이어 몸을 뚫고 뒤를 향한 상태에서 빠져나가질 못한다.
        gunPivot.transform.rotation = Quaternion.LookRotation(transform.forward);
        DodgeVector = new Vector3();
        f_rotateSpeed = 100f;
    }

    private void Update()
    {
        if (b_move && !b_Dodge) Move();
        Rotate();
        Dodge();
    }

    public void Move()
    {
        if(playerInput.verticalMove > 0)
        {
            if(playerInput.onRun)
            {
                f_moveSpeed = f_runSpeed;
                playerAnimator.speed = 1.5f;
            }
        }

        if(playerInput.offRun || playerInput.verticalMove <= 0)
        {
            f_moveSpeed = f_walkSpeed;
            playerAnimator.speed = 1.0f;
        }


        Vector3 moveDistance = ((playerInput.verticalMove * transform.forward) + (playerInput.horizontalMove * transform.right)).normalized;
        if(moveDistance.magnitude > 0)
        {
            playerRigid.MovePosition(playerRigid.position + moveDistance * f_moveSpeed * Time.deltaTime);
        }
        playerAnimator.SetFloat("Move", moveDistance.magnitude);
        /*
        if (playerInput.verticalMove != 0)
        {
            Vector3 moveDistance = playerInput.verticalMove * transform.forward * f_moveSpeed * Time.deltaTime;

            playerRigid.MovePosition(playerRigid.position + moveDistance);

            playerAnimator.SetFloat("Move", playerInput.verticalMove);
        }

        if (playerInput.horizontalMove != 0)
        {
            Vector3 moveDistance = playerInput.horizontalMove * transform.right * f_moveSpeed * Time.deltaTime;

            playerRigid.MovePosition(playerRigid.position + moveDistance);

            playerAnimator.SetFloat("Move", playerInput.horizontalMove);
        }
        */
    }
    public void Rotate()
    {
        if(playerInput.rotateX != 0)
        {
            float mouseMove = playerInput.rotateX * f_rotateSpeed * Time.deltaTime;

            playerRigid.rotation *= Quaternion.Euler(0f, mouseMove, 0f);
        }
        if(playerInput.rotateY != 0)
        {
            float mouseMove = -(playerInput.rotateY) * f_rotateSpeed * Time.deltaTime;
        
            var rot = gunPivot.transform.rotation;
            rot *= Quaternion.Euler(mouseMove, 0f, 0f);
            
            //마우스가 한없이 위로 올라가면 총이 반대방향을 향하거나 한바퀴 돌 수 있기때문에 한계를 두고 그 이상은 나가지 못하게 제한을 둔다.
            float eulerAnglesX = ClampAngle(rot.eulerAngles.x, f_highCamRotation, f_lowCamRotation);
            rot = Quaternion.Euler(eulerAnglesX, rot.eulerAngles.y, 0f);

            gunPivot.transform.rotation = rot;
        }
    }

    private void Dodge()
    {
        //회피 동작 속도를 정해주는 변수
        float perTime = 0.1f;

        if (playerInput.dodge && !b_Dodge
            && GameManager.instance.playerAttack.playerAttackState != PlayerAttacks.ATTACK_STATE.EXECUTE) // 처형상태가 아니라면
        {
            //지형에 부딪힐 땐 가로막혀야 한다.
            int layermask = 1 << LayerMask.NameToLayer("Terrain");

            RaycastHit hit;
            //움직이고 있는 방향으로 회피
            Vector3 DodgeDirection = ((playerInput.verticalMove * transform.forward) + (playerInput.horizontalMove * transform.right)).normalized;
            if (DodgeDirection.magnitude == 0) DodgeDirection = -transform.forward;
            Vector3 Distance;//임시저장값

            if (Physics.Raycast(transform.position + (Vector3.up * 0.5f), DodgeDirection, out hit, f_DodgeDistance, layermask))
            {
                perTime = (f_DodgeDistance / hit.distance) * 0.1f;
                Distance = DodgeDirection * hit.distance;
            }
            else
            {
                Distance = DodgeDirection * f_DodgeDistance;
            }
            DodgeVector = transform.position + Distance;

            StartCoroutine(DodgeRoutine(DodgeVector));
        }

        if(b_Dodge)
        {
            playerRigid.position = Vector3.Lerp(transform.position, DodgeVector, perTime);
        }
    }

    private IEnumerator DodgeRoutine(Vector3 DodgeVector)
    {
        //회피하는 동안은 무적
        GameManager.instance.playerHealth.OnInvincibility();
        b_Dodge = true;

        yield return new WaitForSeconds(1f);

        GameManager.instance.playerHealth.OffInvincibility();
        b_Dodge = false;
        DodgeVector = new Vector3();
    }

    //eulerAnles가 360을 넘어가는 수치를 0으로 되돌리고 0아래로 넘어가는 수치를 360으로 되돌리는 문제가 있기에 일반적인 Mathf.Clamp를 사용할 수가 없어 해당 문제를 해결할 새 함수를 만듦.
    private float ClampAngle(float angle, float from, float to)
    {
        if (angle < 0f) angle += 360;
        if (angle > 180f) return Mathf.Max(angle, 360 + from);
        return Mathf.Min(angle, to);
    }

    //이하 애니메이션 이벤트
    //꽤나 조잡하다 수정필요
    public void OnMove()
    {
        b_move = true;
    }

    public void OffMove()
    {
        b_move = false;
    }

    /*
    private void OnDrawGizmos()
    {
        Vector3 direction = ((playerInput.verticalMove * transform.forward) + (playerInput.horizontalMove * transform.right)).normalized;
        Gizmos.DrawRay(transform.position + (Vector3.up * 0.5f), direction * f_DodgeDistance);
    }
    */
}
