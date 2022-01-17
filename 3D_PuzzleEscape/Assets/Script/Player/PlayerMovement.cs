using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput playerInput;
    private Rigidbody playerRigid;

    [SerializeField] private float f_MoveSpeed;
    [SerializeField] private float f_RotateSpeed;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
        Rotate();
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
            Camera.main.transform.rotation *= Quaternion.Euler(MouseMove, 0f, 0f);
        }
    }
}
