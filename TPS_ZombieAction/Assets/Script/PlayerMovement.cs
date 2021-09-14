using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput playerInput;
    private Rigidbody rigid;
    [SerializeField] float moveSpeed = 1.0f;
    [SerializeField] float rotateSpeed = 1.0f;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rigid = GetComponent<Rigidbody>();
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
            Vector3 moveDistance = playerInput.verticalMove * transform.forward * moveSpeed * Time.deltaTime;

            rigid.MovePosition(rigid.position + moveDistance);
        }
        if (playerInput.horizontalMove != 0)
        {
            Vector3 moveDistance = playerInput.horizontalMove * transform.right * moveSpeed * Time.deltaTime;

            rigid.MovePosition(rigid.position + moveDistance);
        }
    }
    public void Rotate()
    {
        if(playerInput.rotateX != 0)
        {
            float mouseMove = playerInput.rotateX * rotateSpeed;

            rigid.rotation *= Quaternion.Euler(0f, mouseMove, 0f);
        }
    }
}
