using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput playerInput;
    private Animator playerAnim;
    private Rigidbody playerRigid;

    [SerializeField] private CinemachineVirtualCamera Vcam;
    [SerializeField] private float f_MoveSpeed;
    [SerializeField] private float f_RotateSpeed;

    private bool b_OnCrouch;
    private bool b_OnMove;
    private bool b_OnOption;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnim = GetComponent<Animator>();
        playerRigid = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        b_OnMove = true;
        b_OnCrouch = false;
        b_OnOption = true;
    }

    private void Update()
    {
        if (b_OnMove)
        {
            Move();
            Rotate();
            Crouch();
            if (playerInput.ActionCancelKey)
                Option();
        }
    }

    public void Move()
    {
        Vector3 MoveDistance = ((playerInput.transform.right * playerInput.HorizontalMoveKey) + (playerInput.transform.forward * playerInput.VerticalMoveKey)).normalized;
        if(MoveDistance.magnitude > 0)
        {
            playerRigid.MovePosition(playerRigid.position + MoveDistance * f_MoveSpeed * Time.deltaTime);
        }
    }

    public void Rotate()
    {
        if(playerInput.RotateXKey != 0)
        {
            float MouseMove = playerInput.RotateXKey * f_RotateSpeed * Time.deltaTime;
            playerRigid.rotation *= Quaternion.Euler(0f, MouseMove, 0f);
        }
        if(playerInput.RotateYKey != 0)
        {
            float MouseMove = -playerInput.RotateYKey * f_RotateSpeed * Time.deltaTime;

            var rot = Vcam.transform.rotation;
            rot *= Quaternion.Euler(MouseMove, 0f, 0f);

            float ClampAngleX = ClampAngle(rot.eulerAngles.x, -80f, 80f);
            rot = Quaternion.Euler(ClampAngleX, rot.eulerAngles.y, 0f);

            Vcam.transform.rotation = rot;
        }
    }
    //앉기 함수(구현 중...)
    public void Crouch()
    {
        if(playerInput.CrouchKey)
        {
            if (!b_OnCrouch)
            {
                transform.localScale = new Vector3(1f, 2f, 1f);
                b_OnCrouch = true;
            }
            else
            {
                transform.localScale = new Vector3(1f, 3f, 1f);
                b_OnCrouch = false;
            }
            //애니메이션 처리
            /*
            b_OnCrouch = !b_OnCrouch;
            playerAnim.SetBool("b_Crouch", b_OnCrouch);
            */
        }
    }
    public void Option()
    {
        if (!GameManager.instance.playerMovement) return;

        if (GameManager.instance.GetBoolOption() == false)
        {
            GameManager.instance.SetActiveOption(true);
            return;
        }

        //옵션bool이 true일 때, 옵션창을 키고 옵션bool을 false로 바꾼다.
        //옵션bool이 false일 때, 옵션창을 끄고 옵션bool을 true로 바꾼다.
        UIManager.instance.SetGameOptionUI(b_OnOption);

        //이곳에 SetActiveOption함수를 쓰지 말 것. Option창을 끌 때, 게임매니저에서 b_Option을 false로 바꾸고, 다시 이 함수로 돌아오지 못하는 문제가 발생한다.
        if (b_OnOption)
        {
            GameManager.instance.SetActivePlayer(false);
            GameManager.instance.OnCursorVisible();
        }
        else
        {
            GameManager.instance.SetActivePlayer(true);
            GameManager.instance.OffCursorVisible();
        }

        b_OnOption = !b_OnOption;
    }

    //eulerAnles가 360을 넘어가는 수치를 0으로 되돌리고 0아래로 넘어가는 수치를 360으로 되돌리는 문제가 있기에 일반적인 Mathf.Clamp를 사용할 수가 없어 해당 문제를 해결할 새 함수를 만듦.
    private float ClampAngle(float angle, float from, float to)
    {
        if (angle < 0f) angle += 360;
        if (angle > 180f) return Mathf.Max(angle, 360 + from);
        return Mathf.Min(angle, to);
    }

    public void LockMove(bool setbool)
    {
        b_OnMove = setbool;
    }
/*
    public void LockMove()
    {
        b_OnMove = false;
    }
    public void UnlockMove()
    {
        b_OnMove = true;
    }
*/
}
