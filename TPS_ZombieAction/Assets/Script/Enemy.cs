using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : LivingEntity
{
    [SerializeField] private LayerMask whatIsTarget;
    [SerializeField] private float f_SearchRange = 2f;
    [SerializeField] private float f_AttackRange = 0.5f;
    [SerializeField] private LivingEntity targetEntity;
    [SerializeField] private float f_Damage = 20f;
    [SerializeField] private float f_Speed = 1.0f;
    [SerializeField] private float f_timeBetAttck = 0.5f;
    private float f_LastAttackTime;
    private NavMeshAgent pathFinder;

    public ParticleSystem hitEffect;
    public AudioClip deathSound;
    public AudioClip hitSound;
    [SerializeField] private Collider Leftarm_AttackBox;
    [SerializeField] private Collider Rightarm_AttackBox;

    private Animator enemyAnimator;
    private AudioSource enemyAudioPlayer;
    private Renderer enemyRenderer;
    private Rigidbody rigid;

    //제압관련 변수
    private bool b_Suppressed;
    [SerializeField] private float f_SupTime = 3f;
    [SerializeField] private float f_StartingSupHealth = 100f;
    [SerializeField] private Collider ExecutionArea;
    private float f_SupHealth;

    private bool hasTarget
    {
        get
        {
            if(targetEntity != null && !targetEntity.b_Dead)
            {
                return true;
            }
            return false;
        }
    }

    private void Awake()
    {
        pathFinder = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        enemyAudioPlayer = GetComponent<AudioSource>();
        enemyRenderer = GetComponentInChildren<Renderer>();
        rigid = GetComponent<Rigidbody>();
    }

    public void Setup()
    {
        pathFinder.enabled = true;
        Leftarm_AttackBox.GetComponent<DamageBox>().Setup(f_Damage * 0.5f);
        Rightarm_AttackBox.GetComponent<DamageBox>().Setup(f_Damage * 0.5f);

        Leftarm_AttackBox.enabled = false;
        Rightarm_AttackBox.enabled = false;

        //제압관련 초기화
        f_SupHealth = f_StartingSupHealth;
        ExecutionArea.gameObject.SetActive(false);
        b_Suppressed = false;
        enemyAnimator.SetBool("Suppressed", false);

        //기초스탯 초기화
        f_StartingHealth = 300f;
        pathFinder.speed = f_Speed;
        f_LastAttackTime = Time.time;
        pathFinder.stoppingDistance = f_AttackRange;

        rigid.isKinematic = false;

        //죽을 때 아이템을 드랍하는 함수를 OnDeath에 삽입
        OnDeath += () => GameManager.instance.DropItem(transform.position);
    }

    private void Start()
    {
        StartCoroutine(UpdatePath());
    }

    private void Update()
    {
        enemyAnimator.SetBool("HasTarget", hasTarget);
    }

    

    private IEnumerator UpdatePath()
    {
        //살아있는 동안
        while(!b_Dead && !b_Suppressed)
        {
            //타겟을 발견했다면
            if(hasTarget)
            {
                //타겟을 향해 계속 이동
                pathFinder.isStopped = false;
                pathFinder.SetDestination(targetEntity.transform.position);

                if (Time.time >= f_LastAttackTime + f_timeBetAttck)
                {
                    Collider[] colliders = Physics.OverlapSphere(transform.position, f_AttackRange + 0.1f, whatIsTarget);
                    for (int i = 0; i < colliders.Length; i++)
                    {
                        LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();
                        //발견한 대상이 null이 아니고 살아있다면
                        if (livingEntity != null && !livingEntity.b_Dead)
                        {
                            rigid.rotation = Quaternion.LookRotation(livingEntity.transform.position - transform.position);
                            //livingEntity.OnDamage(f_Damage, transform.position, transform.position - livingEntity.transform.position);//원래 직접 대미지를 주던 방식에서 공격충돌체를 만들어서 부딪히면 데미지를 주는 방식으로 변경. 차후 롤백 될수있음.
                            enemyAnimator.SetTrigger("Attack");
                            f_LastAttackTime = Time.time;
                            break;
                        }
                    }
                }
            }
            //타겟을 발견하지 못했다면
            else
            {
                //멈춰서
                pathFinder.isStopped = true;
                //20의 반지름을 가진 구체를 만들고 그 안에 들어온 대상(whatIsTarget)을 탐색함.
                Collider[] colliders = Physics.OverlapSphere(transform.position, f_SearchRange, whatIsTarget);
                for(int i=0; i<colliders.Length; i++)
                {
                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();
                    //발견한 대상이 null이 아니고 살아있다면
                    if(livingEntity != null && !livingEntity.b_Dead)
                    {
                        targetEntity = livingEntity;
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(0.25f);
        }
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!b_Dead)
        {
            var tr = hitEffect.transform;
            tr.position = hitPoint;
            tr.rotation = Quaternion.LookRotation(hitNormal);
            hitEffect.Play();

            enemyAudioPlayer.PlayOneShot(hitSound);

            OnSupDamage(damage);

            if (!hasTarget)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, f_SearchRange * 5f, whatIsTarget);
                
                for (int i = 0; i < colliders.Length; i++)
                {
                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();
                    if (livingEntity != null && !livingEntity.b_Dead)
                    {
                        targetEntity = livingEntity;
                        break;
                    }
                }
            }
            base.OnDamage(damage, hitPoint, hitNormal);
        }
    }
    
    public void OnSupDamage(float newSupDamage)
    {
        if (!b_Dead)
        {
            f_SupHealth -= newSupDamage;

            if (f_SupHealth <= 0)
            {
                StartCoroutine(Suppressed());
            }
        }
    }

    public override void Die()
    {
        if (!b_Dead)
        {
            base.Die();

            Collider[] enemyColliders = GetComponents<Collider>();
            for (int i = 0; i < enemyColliders.Length; i++)
            {
                enemyColliders[i].enabled = false;
            }
            rigid.useGravity = false;
            ExecutionArea.gameObject.SetActive(false);

            pathFinder.enabled = true;
            pathFinder.isStopped = true;
            pathFinder.enabled = false;
            rigid.isKinematic = true;//죽고나서 다른 물체와 충돌할경우 시체가 움직이는 상황방지

            enemyAnimator.SetBool("Suppressed", false);
            enemyAnimator.SetTrigger("Die");
            enemyAudioPlayer.PlayOneShot(deathSound);
        }
    }

    private IEnumerator Suppressed()
    {
        if(f_Health < 0)
        {
            Die();
            yield break;
        }
        var exeArea = ExecutionArea.gameObject;

        b_Suppressed = true;
        pathFinder.ResetPath();
        exeArea.SetActive(true);

        enemyAnimator.SetBool("Suppressed", true);

        yield return new WaitForSeconds(f_SupTime);

        //제압된 상태에서 죽을경우 버그가 생기지 않게 코드처리를 막는다.
        if (!b_Dead)
        {
            f_SupHealth = f_StartingSupHealth;
            exeArea.SetActive(false);
            b_Suppressed = false;
            enemyAnimator.SetBool("Suppressed", false);

            StartCoroutine(UpdatePath());
        }
    }

    public void Execution()
    {
        Die();
    }

    protected override void OnEnable()
    {
        Setup();
        base.OnEnable();
    }

    public IEnumerator LeftArmAttack()
    {
        Leftarm_AttackBox.enabled = true;

        yield return new WaitForSeconds(0.05f);

        Leftarm_AttackBox.enabled = false;
    }

    public IEnumerator RightArmAttack()
    {
        Rightarm_AttackBox.enabled = true;

        yield return new WaitForSeconds(0.08f);

        Rightarm_AttackBox.enabled = false;
    }
}
