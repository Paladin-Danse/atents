using UnityEngine;
using System.Collections;

public class A_to_B : MonoBehaviour {
    public enum UnitState
    {
        Idle,
        Attack,
        Move,
        Dead,
        Heal
    }
    public UnitState US;
    AudioSource AS;
    //public GameObject Destination;//목적지
    public Animator this_Animator;

    public Collider2D EnemyTarget;
    public Collider2D CPTarget;
    public Collider2D Target;
    GameObject Range;
    GameObject HPG_gameobject;
    GameManager GM;

    public AudioClip AttackSound;
    public AudioClip DeadSound;

    Damage_Red_Flash DRF;
    HPGauge HPG;

    public float speed = 5;
    public float Attack_CoolTime;
    float HP;
    public float MaxHP;
    public float ATK_Point = 50;

    bool Attack_Cool = false;
    public bool EnemyContact = false;
    public bool CPContact = false;
    public bool HealContact = false;
    bool Enemy_CP;

    public float Death_Time = 2.0f;
    public float _time = 0.0f;
    public float Damage_Time;

    public bool I_am_TANK = false;
	// Use this for initialization
	void Start () {
        this_Animator = GetComponent<Animator>();

        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        AS = GetComponent<AudioSource>();
        DRF = GetComponent<Damage_Red_Flash>();
        HPG = GetComponent<HPGauge>();
        HPG.SetHPMax(MaxHP);
        HP = MaxHP;

        Range = gameObject.transform.Find("Range").gameObject;
        HPG_gameobject = gameObject.transform.Find("HPGauge").gameObject;
        
        //Destination = GameObject.Find("Sphere");
    }

    // Update is called once per frame
    void FixedUpdate() {

        if (GM.GetGameState() == "Play" || GM.GetGameState() == "Win")
        {
            //타겟에 순서를 정함
            if (EnemyTarget != null)
            {
                Target = EnemyTarget;
            }
            else
            {
                Target = CPTarget;
            }

            //공격을 할지 이동을 할지를 결정
            //공격을 할 경우
            if (this_Animator)
            {
                /*
                switch(US)
                {
                    case UnitState.Move:
                        break;
                    case UnitState.Idle:
                        break;
                    case UnitState.Attack:
                        break;
                    case UnitState.Heal:
                        break;
                    default:
                        break;
                }
                */
                if (EnemyContact && US != UnitState.Dead && !HealContact)//적과 조우하였을떄 공격
                {
                    US = UnitState.Attack;

                    if (Attack_Cool == false)
                    {
                        Attack_Animation(true);
                        StartCoroutine("EnemyAttack");
                    }
                }
                else if (CPContact && US != UnitState.Dead && !HealContact)
                {
                    US = UnitState.Attack;

                    if (EnemyContact && Attack_Cool == false)
                    {
                        Attack_Animation(true);
                        StartCoroutine("EnemyAttack");
                    }
                    else if (Attack_Cool == false)
                    {
                        Attack_Animation(true);
                        StartCoroutine("CPAttack");
                    }
                }
                //이동을 할 경우
                else if (US != UnitState.Dead && US != UnitState.Heal && !HealContact)
                {
                        US = UnitState.Move;

                        Attack_Animation(false);
                        this_Animator.SetTrigger("InMove");
                        transform.Translate(Vector2.right * speed);
                }

                else if (US != UnitState.Dead && US == UnitState.Heal)
                {
                    if(GetComponent<MED_Heal>().GetCoolTime())
                    {
                        //OnIdle_Anim();
                    }
                }

            }
        }

        //적이 제거되었을 시, EnemyContact를 꺼줌
        if (Target == null)
        {
            EnemyContact = false;
        }
        if (CPTarget == null)
        {
            CPContact = false;
        }

        //transform.position = Vector2.MoveTowards(gameObject.transform.position, Destination.transform.position, _Time);
    }

    IEnumerator EnemyAttack()//공격하는 코루틴
    {
        if(Target != null && US != UnitState.Dead)
        {
            Attack_Cool = true;
            
            if (!I_am_TANK)
            {
                Target.gameObject.SendMessage("HPLost", ATK_Point);//상대의 HP를 깎는 함수를 상대의 스크립트에서 호출
            }
            else
            {
                GetComponent<Tank_Attack>().Attack_Boom(Target);
            }
            On_Attack_Sound();
            US = UnitState.Idle;

            yield return new WaitForSeconds(Attack_CoolTime);
            
            OffIdle_Anim();
            
            Attack_Cool = false;
            US = UnitState.Attack;
        }
    }

    IEnumerator CPAttack()//막사 공격하는 코루틴
    {
        if (CPTarget != null && US != UnitState.Dead)
        {
            Attack_Cool = true;
            if (!I_am_TANK)
            {
                CPTarget.gameObject.GetComponent<CPManager>().SendMessage("HPLost", ATK_Point);
            }
            else
            {
                GetComponent<Tank_Attack>().Attack_Boom(CPTarget);
            }
            On_Attack_Sound();
            US = UnitState.Idle;

            yield return new WaitForSeconds(Attack_CoolTime);

            OffIdle_Anim();

            Attack_Cool = false;
            US = UnitState.Attack;
        }
    }
    public void HPLost(float Atk_point)//공격을 입고 HP를 잃는 함수
    {
        HP -= Atk_point;
        DRF.Red_Flash();//데미지 점멸이펙트
        HPG.GaugeState(HP);
        if (HP <= 0)
        {
            US = UnitState.Dead;
            StopAllCoroutines();
            StartCoroutine("Dead");
        }
    }
    public void HP_Heal(float Heal_point)//회복하는 함수
    {
        if (HP > 0 && US != UnitState.Dead)
        {
            HP += Heal_point;

            HPG.GaugeState(HP);
        }
    }


    IEnumerator Stay(float StayTime)
    {
        yield return new WaitForSeconds(StayTime);
    }

    IEnumerator Dead()
    {
        US = UnitState.Dead;

        Destroy_UI_Collider();

        On_Dead_Sound();
        Dead_Animation();
        yield return new WaitForSeconds(Death_Time);
        Destroy(gameObject);
    }

    void Destroy_UI_Collider()
    {
        Destroy(gameObject.GetComponent<Collider2D>());
        Destroy(Range);
        Destroy(HPG_gameobject);
    }

    void Destroy_SpriteImage()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }


    public void SetEnemyTarget(Collider2D other)//단일 적 한명을 겨냥하는 함수
    {
        EnemyTarget = other;
    }
    public void SetCPTarget(Collider2D other)
    {
        CPTarget = other;
    }

    public void SetEnemyContact(bool Contact)//적과의 조우여부를 다른 함수에서 넘겨받는 함수
    {
        EnemyContact = Contact;
    }
    public void SetCPContact(bool Contact)//막사와의 조우여부를 다른 함수에서 넘겨받는 함수
    {
        CPContact = Contact;
    }

   public float GetHPMax()
    {
        return MaxHP;
    }
    public float GetHP()
    {
        return HP;
    }
    public void HealState()
    {
        US = UnitState.Heal;
    }

    



    //사운드 관련 함수
    void On_Attack_Sound()
    {
        if (AttackSound)
        {
            AS.clip = AttackSound;
            AS.volume = 0.5f;
            AS.Play();
        }
        else
        {
            Debug.Log("Not Sound");
        }
    }

    void On_Dead_Sound()
    {
        if (DeadSound)
        {
            AS.clip = DeadSound;
            AS.volume = 1f;
            AS.Play();
        }
        else
        {
            Debug.Log("Not Sound");
        }
    }







    //각종 애니메이션 함수
    public void OffIdle_Anim()
    {
        this_Animator.SetBool("Attack_Cool", false);
    }
    public void OnIdle_Anim()
    {
        this_Animator.SetBool("Attack_Cool", true);
    }

    public void Attack_Animation(bool OnAttack)
    {
        this_Animator.SetBool("InAttack", OnAttack);
    }
    
    void Move_Animation()
    {
        this_Animator.SetTrigger("InMove");
    }

    public void Dead_Animation()
    {
        this_Animator.SetTrigger("InDead");
    }
    public void Heal_Animation()
    {
        this_Animator.SetTrigger("InHealing");
    }
}
