using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : LivingEntity
{
    public AudioClip deathSound;
    public AudioClip hitSound;
    public AudioClip itemPickupSound;

    private Animator playerAnimator;
    private AudioSource playerAudioPlayer;

    private PlayerMovement playerMovement;
    private PlayerAttacks playerShooter;
    private PlayerItemLooting playerItemLooting;

    private bool b_Invincibility;

    //테스트를 위해 가져오는 입력키값 실제로는 필요없음.
    private PlayerInput playerInput;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerAudioPlayer = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooter = GetComponent<PlayerAttacks>();
        playerItemLooting = GetComponent<PlayerItemLooting>();
        b_Invincibility = false;
        //테스트를 위해 가져오는 입력키값 실제로는 필요없음.
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        //테스트를 위한 데미지입기
        if(playerInput.test)
        {
            OnDamage(20f, Vector3.zero, Vector3.zero);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        playerMovement.enabled = true;
        playerShooter.enabled = true;
        OnDeath += GameManager.instance.OnCursorVisible;
        OnDeath += UIManager.instance.OnGameOverUI;
    }
    public override void RestoreHealth(float newHealth)
    {
        base.RestoreHealth(newHealth);

        UIManager.instance.UpdateplayerHealthBar();
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!b_Dead && !b_Invincibility)
        {
            playerAudioPlayer.PlayOneShot(hitSound);
            base.OnDamage(damage, hitPoint, hitNormal);
            UIManager.instance.UpdateplayerHealthBar();
        }
    }

    public override void OnDotDamage(float m_damage, float m_dotTime)
    {
        if (Time.time >= f_LastDotDamageTime + m_dotTime)
        {
            f_Health -= (m_damage / 3);
            if (f_Health <= 0 && !b_Dead) Die();
            f_LastDotDamageTime = Time.time;
        }

        UIManager.instance.UpdateplayerHealthBar();
    }

    public override void Die()
    {
        base.Die();

        playerAnimator.SetTrigger("Die");

        playerMovement.enabled = false;
        playerShooter.enabled = false;
        playerItemLooting.enabled = false;
    }

    public void OnInvincibility()
    {
        b_Invincibility = true;
    }
    public void OffInvincibility()
    {
        b_Invincibility = false;
    }
}
