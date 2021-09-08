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

        //����� ���� 2���� ����
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

        //�������� �ѱ� ��
        bulletLineRenderer.SetPosition(0, fireTransform.position);
        //������ �Է����� ���� �浹 ��ġ
        bulletLineRenderer.SetPosition(1, hitPosition);
        bulletLineRenderer.enabled = true;

        yield return new WaitForSeconds(0.03f);

        bulletLineRenderer.enabled = false;
    }

    //��Ƽ踦 ���� �Լ�
    public void Fire()
    {
        if(state == State.READY && Time.time >= lastFireTime + timeBetFire)
        {
            lastFireTime = Time.time;
            //���
            Shot();
        }
    }

    //��������� �ϴ� �Լ�
    private void Shot()
    {
        RaycastHit hit;

        Vector3 hitPosition = Vector3.zero;

        if(Physics.Raycast(fireTransform.position, fireTransform.forward, out hit, fireDistance))
        {
            iDamageable target = hit.collider.GetComponent<iDamageable>();
            if(target != null)
            {
                //�ѿ� ���� ��뿡�� ������ �Լ�����
                target.OnDamage(damage, hit.point, hit.normal);
            }
            hitPosition = hit.point;
        }
        else
        {
            //�浹�� ��ü�� ���� �� �ѱ����� ���� �ִ� ��Ÿ���ŭ�� ��ġ�� �浹��ġ�� ǥ��
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
