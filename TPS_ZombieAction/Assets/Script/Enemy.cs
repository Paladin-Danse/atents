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

    [SerializeField] private float f_Damage = 20f;
    [SerializeField] private float f_timeBetAttck = 0.5f;
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
    }

    public void Setup()
    {
        pathFinder.speed = 2.0f;
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
        while(!b_Dead)
        {
            if(hasTarget)
            {
                pathFinder.isStopped = false;
                pathFinder.SetDestination(targetEntity.transform.position);
            }
            else
            {
                pathFinder.isStopped = true;

                Collider[] colliders = Physics.OverlapSphere(transform.position, 20f, whatIsTarget);
                for(int i=0; i<colliders.Length; i++)
                {

                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();

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
            hitEffect.transform.position = hitPoint;
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
            hitEffect.Play();

            enemyAudioPlayer.PlayOneShot(hitSound);
        }

        base.OnDamage(damage, hitPoint, hitNormal);
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

        enemyAnimator.SetTrigger("Die");
        enemyAudioPlayer.PlayOneShot(deathSound);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Setup();
    }
}
