using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : LivingEntity
{
    

    [SerializeField] private LayerMask whatIsTarget;
    [SerializeField] private LivingEntity targetEntity;
    private NavMeshAgent pathFinder;

    public ParticleSystem hitEffect;
    public AudioClip deathSound;
    public AudioClip hitSound;

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

    //아직 쓰고 있지 않은 변수
    //[SerializeField] private float f_Damage = 20f;
    //[SerializeField] private float f_timeBetAttck = 0.5f;
    private float f_LastAttackTime;

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
        //제압관련 초기화
        f_SupHealth = f_StartingSupHealth;
        ExecutionArea.gameObject.SetActive(false);
        b_Suppressed = false;

        //기초스탯 초기화
        f_StartingHealth = 300f;
        pathFinder.speed = 2.0f;

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
            }
            //타겟을 발견하지 못했다면
            else
            {
                //멈춰서
                pathFinder.isStopped = true;
                //20의 반지름을 가진 구체를 만들고 그 안에 들어온 대상(whatIsTarget)을 탐색함.
                Collider[] colliders = Physics.OverlapSphere(transform.position, 20f, whatIsTarget);
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
        if(!b_Dead)
        {
            var tr = hitEffect.transform;
            tr.position = hitPoint;
            tr.rotation = Quaternion.LookRotation(hitNormal);
            hitEffect.Play();

            enemyAudioPlayer.PlayOneShot(hitSound);
        }
        OnSupDamage(damage);
        base.OnDamage(damage, hitPoint, hitNormal);
    }
    
    public void OnSupDamage(float newSupDamage)
    {
        f_SupHealth -= newSupDamage;
        
        if(f_SupHealth <= 0)
        {
            b_Suppressed = true;
            StartCoroutine(Suppressed());
        }
    }

    public override void Die()
    {
        base.Die();

        Collider[] enemyColliders = GetComponents<Collider>();
        for(int i=0; i < enemyColliders.Length; i++)
        {
            enemyColliders[i].enabled = true;
        }

        pathFinder.isStopped = true;
        pathFinder.enabled = false;
        rigid.isKinematic = true;//죽고나서 다른 물체와 충돌할경우 시체가 움직이는 상황방지

        enemyAnimator.SetTrigger("Die");
        enemyAudioPlayer.PlayOneShot(deathSound);
    }

    private IEnumerator Suppressed()
    {
        var exeArea = ExecutionArea.gameObject;

        pathFinder.isStopped = true;
        exeArea.SetActive(true);

        yield return new WaitForSeconds(f_SupTime);

        //제압된 상태에서 죽을경우 버그가 생기지 않게 코드처리를 막는다.
        if (!b_Dead)
        {
            f_SupHealth = f_StartingSupHealth;
            exeArea.SetActive(false);
            b_Suppressed = false;
            StartCoroutine(UpdatePath());
        }
    }

    public void Execution()
    {
        Debug.Log("Execution On!");
        Die();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Setup();
    }
}
