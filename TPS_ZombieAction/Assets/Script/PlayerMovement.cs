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
    //���� �������� ���� ����
    //[SerializeField] private float f_aimMoveSpeed = 1.0f;
    //[SerializeField] private float f_runSpeed = 3.0f;
    [SerializeField] private float f_rotateSpeed = 1.0f;
    [SerializeField] private GameObject gunPivot;
    [SerializeField] private float f_highCamRotation = -60f;//ī�޶� ������ ����
    [SerializeField] private float f_lowCamRotation = 60f;//ī�޶� �Ʒ����� ����
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
            
            //���콺�� �Ѿ��� ���� �ö󰡸� ���� �ݴ������ ���ϰų� �ѹ��� �� �� �ֱ⶧���� �Ѱ踦 �ΰ� �� �̻��� ������ ���ϰ� ������ �д�.
            float eulerAnglesX = ClampAngle(rot.eulerAngles.x, f_highCamRotation, f_lowCamRotation);
            rot = Quaternion.Euler(eulerAnglesX, rot.eulerAngles.y, rot.eulerAngles.z);
            gunPivot.transform.rotation = rot;
        }
    }

    //eulerAnles�� 360�� �Ѿ�� ��ġ�� 0���� �ǵ����� 0�Ʒ��� �Ѿ�� ��ġ�� 360���� �ǵ����� ������ �ֱ⿡ �Ϲ����� Mathf.Clamp�� ����� ���� ���� �ش� ������ �ذ��� �� �Լ��� ����.
    private float ClampAngle(float angle, float from, float to)
    {
        if (angle < 0f) angle += 360;
        if (angle > 180f) return Mathf.Max(angle, 360 + from);
        return Mathf.Min(angle, to);
    }

    //���� �ִϸ��̼� �̺�Ʈ
    //�ϳ� �����ϴ� �����ʿ�
    public void OnMove()
    {
        b_move = true;
    }

    public void OffMove()
    {
        b_move = false;
    }
}
