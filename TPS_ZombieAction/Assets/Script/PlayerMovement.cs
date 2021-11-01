using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MOVESTATE
{
    NONE = 0,
    WALKMOVE,
    AIMMOVE,
    RUNMOVE
}

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput playerInput;
    private Rigidbody playerRigid;
    private Animator playerAnimator;
    private float f_moveSpeed;
    private bool b_move;
    [SerializeField] private float f_walkSpeed = 1.5f;
    //아직 쓰고있지 않은 변수
    //[SerializeField] private float f_aimMoveSpeed = 1.0f;
    //[SerializeField] private float f_runSpeed = 3.0f;
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
    }

    private void Update()
    {
        if (b_move) Move();
    }

    private void FixedUpdate()
    {
        //if (b_move) Move();
        Rotate();
    }

    public void Move()
    {
        if(playerInput.verticalMove != 0)
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
    }
    public void Rotate()
    {
        if(playerInput.rotateX != 0)
        {
            float mouseMove = playerInput.rotateX * f_rotateSpeed;

            playerRigid.rotation *= Quaternion.Euler(0f, mouseMove, 0f);
        }
        if(playerInput.rotateY != 0)
        {
            float mouseMove = -(playerInput.rotateY) * f_rotateSpeed;
        
            var rot = gunPivot.transform.rotation;
            rot *= Quaternion.Euler(mouseMove, 0f, 0f);
            
            //마우스가 한없이 위로 올라가면 총이 반대방향을 향하거나 한바퀴 돌 수 있기때문에 한계를 두고 그 이상은 나가지 못하게 제한을 둔다.
            float eulerAnglesX = ClampAngle(rot.eulerAngles.x, f_highCamRotation, f_lowCamRotation);
            rot = Quaternion.Euler(eulerAnglesX, rot.eulerAngles.y, rot.eulerAngles.z);
            gunPivot.transform.rotation = rot;
        }
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
}
