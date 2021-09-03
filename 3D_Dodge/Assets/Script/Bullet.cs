using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public event Action EventHadleOnCollisionPlayer;
    Rigidbody rigid;

    private void Start()
    {
        gameObject.SetActive(false);
        if (!rigid) rigid = GetComponent<Rigidbody>();
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void OnFire(Vector3 dir, float force)
    {
        gameObject.SetActive(true);

        rigid.velocity = Vector3.zero;
        rigid.AddForce(dir.normalized * force);
    }

    private void OnTriggerEnter(Collider other)
    {
        var tag = other.tag;
        if (tag.Equals("Player"))
        {
            if (null != EventHadleOnCollisionPlayer) EventHadleOnCollisionPlayer();
        }
        else if (tag.Equals("Respawn") || other.name.Equals(name)) return;

        gameObject.SetActive(false);
    }
}
