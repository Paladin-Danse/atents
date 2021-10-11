using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput playerInput;
    private Rigidbody playerRigid;
    private Animator playerAnimator;
    [SerializeField] private float f_moveSpeed = 1.0f;
    [SerializeField] private float f_rotateSpeed = 1.0f;
    [SerializeField] private GameObject gunPivot;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigid = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    public void Move()
    {
        if(playerInput.verticalMove != 0)
        {
            Vector3 moveDistance = playerInput.verticalMove * transform.forward * f_moveSpeed * Time.deltaTime;

            playerRigid.MovePosition(playerRigid.position + moveDistance);
        }
        if (playerInput.horizontalMove != 0)
        {
            Vector3 moveDistance = playerInput.horizontalMove * transform.right * f_moveSpeed * Time.deltaTime;

            playerRigid.MovePosition(playerRigid.position + moveDistance);
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
            float mouseMove = playerInput.rotateY * f_rotateSpeed;

            var rot = gunPivot.transform.rotation;
            rot *= Quaternion.Euler(-mouseMove, 0f, 0f);
            //제한범위가 0~60으로 걸림. 뭐가 문제인지 파악이 필요
            rot.eulerAngles = new Vector3(Mathf.Clamp(rot.eulerAngles.x, -60f, 60f), rot.eulerAngles.y, rot.eulerAngles.z);
            gunPivot.transform.rotation = rot;
        }
    }
}
