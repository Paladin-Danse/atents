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

    //���а��� ����
    private bool b_Suppressed;
    [SerializeField] private float f_SupTime = 3f;
    [SerializeField] private float f_StartingSupHealth = 100f;
    [SerializeField] private Collider ExecutionArea;
    private float f_SupHealth;

    //���� ���� ���� ���� ����
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
        //���а��� �ʱ�ȭ
        f_SupHealth = f_StartingSupHealth;
        ExecutionArea.gameObject.SetActive(false);
        b_Suppressed = false;

        //���ʽ��� �ʱ�ȭ
        f_StartingHealth = 300f;
        pathFinder.speed = 2.0f;

        rigid.isKinematic = false;
        
        //���� �� �������� ����ϴ� �Լ��� OnDeath�� ����
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
        //����ִ� ����
        while(!b_Dead && !b_Suppressed)
        {
            //Ÿ���� �߰��ߴٸ�
            if(hasTarget)
            {
                //Ÿ���� ���� ��� �̵�
                pathFinder.isStopped = false;
                pathFinder.SetDestination(targetEntity.transform.position);
            }
            //Ÿ���� �߰����� ���ߴٸ�
            else
            {
                //���缭
                pathFinder.isStopped = true;
                //20�� �������� ���� ��ü�� ����� �� �ȿ� ���� ���(whatIsTarget)�� Ž����.
                Collider[] colliders = Physics.OverlapSphere(transform.position, 20f, whatIsTarget);
                for(int i=0; i<colliders.Length; i++)
                {
                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();
                    //�߰��� ����� null�� �ƴϰ� ����ִٸ�
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
        rigid.isKinematic = true;//�װ��� �ٸ� ��ü�� �浹�Ұ�� ��ü�� �����̴� ��Ȳ����

        enemyAnimator.SetTrigger("Die");
        enemyAudioPlayer.PlayOneShot(deathSound);
    }

    private IEnumerator Suppressed()
    {
        var exeArea = ExecutionArea.gameObject;

        pathFinder.isStopped = true;
        exeArea.SetActive(true);

        yield return new WaitForSeconds(f_SupTime);

        //���е� ���¿��� ������� ���װ� ������ �ʰ� �ڵ�ó���� ���´�.
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
