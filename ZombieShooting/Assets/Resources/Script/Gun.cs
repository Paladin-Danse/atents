using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum State
    {
        READY,
        EMPTY,
        RELOADING
    }

    public State state { get; private set; }

    public Transform fireTransform;

    public ParticleSystem muzzleFlashEffect;
    public ParticleSystem shellEjectEffect;

    private LineRenderer bulletLineRenderer;

    private AudioSource gunAudioPlayer;
    public AudioClip shotClip;
    public AudioClip reloadClip;
    [SerializeField]private float damage = 25f;
    [SerializeField]private float fireDistance = 50f;

    public int ammoRemain = 100;
    [SerializeField] private int magCapacity = 25;
    public int magAmmo;
    [SerializeField] private float timeBetFire = 0.12f;
    [SerializeField] private float reloadTime = 1.8f;
    [SerializeField] private float lastFireTime;

    private void Awake()
    {
        gunAudioPlayer = GetComponent<AudioSource>();
        bulletLineRenderer = GetComponent<LineRenderer>();

        //사용할 점을 2개로 지정
        bulletLineRenderer.positionCount = 2;

        bulletLineRenderer.enabled = false;
    }

    private void OnEnable()
    {
        magAmmo = magCapacity;
        state = State.READY;
        lastFireTime = 0;
    }

    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        muzzleFlashEffect.Play();
        shellEjectEffect.Play();

        gunAudioPlayer.PlayOneShot(shotClip);

        //시작점은 총구 끝
        bulletLineRenderer.SetPosition(0, fireTransform.position);
        //끝점은 입력으로 들어온 충돌 위치
        bulletLineRenderer.SetPosition(1, hitPosition);
        bulletLineRenderer.enabled = true;

        yield return new WaitForSeconds(0.03f);

        bulletLineRenderer.enabled = false;
    }

    //방아쇠를 당기는 함수
    public void Fire()
    {
        if(state == State.READY && Time.time >= lastFireTime + timeBetFire)
        {
            lastFireTime = Time.time;
            //사격
            Shot();
        }
    }

    //실제사격을 하는 함수
    private void Shot()
    {
        RaycastHit hit;

        Vector3 hitPosition = Vector3.zero;

        if(Physics.Raycast(fireTransform.position, fireTransform.forward, out hit, fireDistance))
        {
            iDamageable target = hit.collider.GetComponent<iDamageable>();
            if(target != null)
            {
                //총에 맞은 상대에게 데미지 함수적용
                target.OnDamage(damage, hit.point, hit.normal);
            }
            hitPosition = hit.point;
        }
        else
        {
            //충돌한 물체가 없을 시 총구에서 총의 최대 사거리만큼의 위치를 충돌위치로 표현
            hitPosition = fireTransform.position + fireTransform.forward * fireDistance;
        }
    }

    public bool Reload()
    {
        if(state == State.RELOADING || ammoRemain <= 0 || magAmmo >= magCapacity)
        {
            return false;
        }
        StartCoroutine(ReloadRoutine());
        return true;
    }

    private IEnumerator ReloadRoutine()
    {
        state = State.RELOADING;

        gunAudioPlayer.PlayOneShot(reloadClip);

        yield return new WaitForSeconds(reloadTime);

        int ammoToFill = magCapacity - magAmmo;

        if(ammoRemain < ammoToFill)
        {
            ammoToFill = ammoRemain;
        }

        magAmmo += ammoToFill;
        ammoRemain -= ammoToFill;

        state = State.READY;
    }


}
