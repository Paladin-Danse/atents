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
    private PlayerShooter playerShooter;

    //테스트를 위해 가져오는 입력키값 실제로는 필요없음.
    private PlayerInput playerInput;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerAudioPlayer = GetComponent<AudioSource>();
        playerMovement = GameManager.instance.playerMovement;
        playerShooter = GameManager.instance.playerShooter;

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
    }
    public override void RestoreHealth(float newHealth)
    {
        base.RestoreHealth(newHealth);

        UIManager.instance.UpdateplayerHealthBar();
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if(!b_Dead)
        {
            playerAudioPlayer.PlayOneShot(hitSound);
        }

        base.OnDamage(damage, hitPoint, hitNormal);

        UIManager.instance.UpdateplayerHealthBar();
    }

    public override void Die()
    {
        base.Die();

        playerAnimator.SetTrigger("Die");

        playerMovement.enabled = false;
        playerShooter.enabled = false;
    }
}
