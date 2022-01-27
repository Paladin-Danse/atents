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

            var rot = Camera.main.transform.rotation;
            rot *= Quaternion.Euler(MouseMove, 0f, 0f);

            float ClampAngleX = ClampAngle(rot.eulerAngles.x, -60f, 60f);
            rot = Quaternion.Euler(ClampAngleX, rot.eulerAngles.y, 0f);

            Camera.main.transform.rotation = rot;
            
        }
    }

    //eulerAnles가 360을 넘어가는 수치를 0으로 되돌리고 0아래로 넘어가는 수치를 360으로 되돌리는 문제가 있기에 일반적인 Mathf.Clamp를 사용할 수가 없어 해당 문제를 해결할 새 함수를 만듦.
    private float ClampAngle(float angle, float from, float to)
    {
        if (angle < 0f) angle += 360;
        if (angle > 180f) return Mathf.Max(angle, 360 + from);
        return Mathf.Min(angle, to);
    }
}
