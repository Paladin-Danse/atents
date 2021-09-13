using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerInput playerInput;
    PlayerHealth playerHealth;
    Rigidbody playerRigidbody;
    Animator playerAnimator;
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private float rotateSpeed = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerHealth = GetComponent<PlayerHealth>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        Damaged();
    }
    private void FixedUpdate()
    {
        Rotate();
        Move();

        playerAnimator.SetFloat("Move", playerInput.move);
    }

    private void Move()
    {
        Vector3 moveDistance = playerInput.move * transform.forward * moveSpeed * Time.deltaTime;

        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    }

    private void Rotate()
    {
        float turn = playerInput.rotate * rotateSpeed * Time.deltaTime;

        playerRigidbody.rotation = playerRigidbody.rotation * Quaternion.Euler(0f, turn, 0f);
    }
    //플레이어 디버그용 자해키
    private void Damaged()
    {
        if (playerInput.damaged)
        {
            playerHealth.OnDamage(20f, Vector3.zero, Vector3.forward);
        }
    }
    /*PlayerHealth스크립트에도 똑같은 코드가 있어서 제외
    private void OnTriggerEnter(Collider other)
    {
        IItem item = other.GetComponent<IItem>();

        if(item != null) item.Use(gameObject);
    }
    */
}
