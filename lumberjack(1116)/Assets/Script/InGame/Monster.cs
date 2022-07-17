using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {
    enum Monster_State
    {
        Idle = 0,
        Attack,
        Awake,
        Dead
    }
    Monster_State MS;
    Animator Monster_Anim;
    public Transform Shoot_Pos;
    public GameObject Projectile;

    public float speed;
    float HP;

    float _time = 0;
    public float Attack_Time;
	// Use this for initialization
	void Start () {
        Monster_Anim = GetComponent<Animator>();
        HP = GetComponent<HP_Enable>().HP;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        switch(MS)
        {
            case Monster_State.Idle:
                GetComponent<HP_Enable>().HP = HP;
                break;
            case Monster_State.Attack:
                transform.Translate(Vector2.right * speed);
                Monster_Anim.SetTrigger("InAttack");
                On_Awake();
                break;
            case Monster_State.Awake:
                transform.Translate(Vector2.right * speed);
                _time += Time.deltaTime;
                if(_time >= Attack_Time)
                {
                    Debug.Log("InTime");
                    On_Attack();
                    _time = 0;
                }
                break;
            case Monster_State.Dead:
                break;
        }
	}

    public void On_Destroy()
    {
        MS = Monster_State.Dead;
        GameObject.Find("GameManager").GetComponent<GameManager>().GamePlay();
        GameObject.Find("GameManager").GetComponent<GameManager>().On_PlaySound();
        GameObject.Find("GameManager").GetComponent<Plan_Spawner>().Count_Reset();
        Destroy(gameObject);
    }

    public void Instantiate_Projectile()
    {
        Instantiate(Projectile, Shoot_Pos);
    }

    public void Load_Idle_Anim()
    {
        Monster_Anim.SetTrigger("InIdle");
    }

    void On_Attack()
    {
        MS = Monster_State.Attack;
    }

    public void On_Awake()
    {
        MS = Monster_State.Awake; 
    }
    
}
