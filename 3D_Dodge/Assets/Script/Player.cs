using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector3 position { get { return transform.position; } }

    [SerializeField] private float speed = 8f;
    private Vector3 velocity = Vector3.zero;
    Rigidbody rigid;
    public bool isLive { get { return gameObject.activeSelf; } }

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    public void Init()
    {
        velocity = Vector3.zero;
        gameObject.SetActive(true);
    }

    void FixedUpdate()
    {
        velocity.x = Input.GetAxis("Horizontal") * speed;
        velocity.z = Input.GetAxis("Vertical") * speed;
        rigid.velocity = velocity;
    }

    public void OnDamaged()
    {
        gameObject.SetActive(false);
    }
}
