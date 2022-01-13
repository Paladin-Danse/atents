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

    //���а��� ����
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

        //���а��� �ʱ�ȭ
        f_SupHealth = f_StartingSupHealth;
        ExecutionArea.gameObject.SetActive(false);
        b_Suppressed = false;
        enemyAnimator.SetBool("Suppressed", false);

        //���ʽ��� �ʱ�ȭ
        f_StartingHealth = 300f;
        pathFinder.speed = f_Speed;
        f_LastAttackTime = Time.time;
        pathFinder.stoppingDistance = f_AttackRange;

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

                if (Time.time >= f_LastAttackTime + f_timeBetAttck)
                {
                    Collider[] colliders = Physics.OverlapSphere(transform.position, f_AttackRange + 0.1f, whatIsTarget);
                    for (int i = 0; i < colliders.Length; i++)
                    {
                        LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();
                        //�߰��� ����� null�� �ƴϰ� ����ִٸ�
                        if (livingEntity != null && !livingEntity.b_Dead)
                        {
                            rigid.rotation = Quaternion.LookRotation(livingEntity.transform.position - transform.position);
                            //livingEntity.OnDamage(f_Damage, transform.position, transform.position - livingEntity.transform.position);//���� ���� ������� �ִ� ��Ŀ��� �����浹ü�� ���� �ε����� �������� �ִ� ������� ����. ���� �ѹ� �ɼ�����.
                            enemyAnimator.SetTrigger("Attack");
                            f_LastAttackTime = Time.time;
                            break;
                        }
                    }
                }
            }
            //Ÿ���� �߰����� ���ߴٸ�
            else
            {
                //���缭
                pathFinder.isStopped = true;
                //20�� �������� ���� ��ü�� ����� �� �ȿ� ���� ���(whatIsTarget)�� Ž����.
                Collider[] colliders = Physics.OverlapSphere(transform.position, f_SearchRange, whatIsTarget);
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
            rigid.isKinematic = true;//�װ��� �ٸ� ��ü�� �浹�Ұ�� ��ü�� �����̴� ��Ȳ����

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

        //���е� ���¿��� ������� ���װ� ������ �ʰ� �ڵ�ó���� ���´�.
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
