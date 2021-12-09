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

    //ȸ��
    private bool b_Dodge;
    private Vector3 DodgeVector;
    [SerializeField] private float f_DodgeDistance = 5f;

    [SerializeField] private float f_walkSpeed = 1.5f;
    //���� �������� ���� ����
    //[SerializeField] private float f_aimMoveSpeed = 1.0f;
    [SerializeField] private float f_runSpeed = 3.0f;
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
        b_Dodge = false;
    }

    private void Start()
    {
        //���� gunPivot�� rotation�� ������ �������� �����ؼ� ���� �ڸ� ���� �ٶ��� �÷��̾� ���� �հ� �ڸ� ���� ���¿��� ���������� ���Ѵ�.
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
            
            //���콺�� �Ѿ��� ���� �ö󰡸� ���� �ݴ������ ���ϰų� �ѹ��� �� �� �ֱ⶧���� �Ѱ踦 �ΰ� �� �̻��� ������ ���ϰ� ������ �д�.
            float eulerAnglesX = ClampAngle(rot.eulerAngles.x, f_highCamRotation, f_lowCamRotation);
            rot = Quaternion.Euler(eulerAnglesX, rot.eulerAngles.y, 0f);

            gunPivot.transform.rotation = rot;
        }
    }

    private void Dodge()
    {
        //ȸ�� ���� �ӵ��� �����ִ� ����
        float perTime = 0.1f;

        if (playerInput.dodge && !b_Dodge
            && GameManager.instance.playerAttack.playerAttackState != PlayerAttacks.ATTACK_STATE.EXECUTE) // ó�����°� �ƴ϶��
        {
            //������ �ε��� �� ���θ����� �Ѵ�.
            int layermask = 1 << LayerMask.NameToLayer("Terrain");

            RaycastHit hit;
            //�����̰� �ִ� �������� ȸ��
            Vector3 DodgeDirection = ((playerInput.verticalMove * transform.forward) + (playerInput.horizontalMove * transform.right)).normalized;
            if (DodgeDirection.magnitude == 0) DodgeDirection = -transform.forward;
            Vector3 Distance;//�ӽ����尪

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
        //ȸ���ϴ� ������ ����
        GameManager.instance.playerHealth.OnInvincibility();
        b_Dodge = true;

        yield return new WaitForSeconds(1f);

        GameManager.instance.playerHealth.OffInvincibility();
        b_Dodge = false;
        DodgeVector = new Vector3();
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

    /*
    private void OnDrawGizmos()
    {
        Vector3 direction = ((playerInput.verticalMove * transform.forward) + (playerInput.horizontalMove * transform.right)).normalized;
        Gizmos.DrawRay(transform.position + (Vector3.up * 0.5f), direction * f_DodgeDistance);
    }
    */
}
