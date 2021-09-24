using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum STATE
    {
        STATE_READY,
        STATE_EMPTY,
        STATE_RELOADING
    }
    [SerializeField] private STATE e_State;

    public enum TYPE
    {
        TYPE_FULLAUTO,
        TYPE_SEMIAUTO
    }
    [SerializeField] private TYPE e_Type;

    [SerializeField] private Transform fireTransform;
    [SerializeField] private Transform leftHandle;
    public Transform LeftHandle { get { return leftHandle; } }
    [SerializeField] private Transform rightHandle;
    public Transform RightHandle { get { return rightHandle; } }

    [SerializeField] private ParticleSystem muzzleFlashEffect;
    [SerializeField] private ParticleSystem shellEjectEffect;

    [SerializeField] private AudioSource gunAudioPlayer;
    [SerializeField] private AudioClip shotClip;
    [SerializeField] private AudioClip reloadClip;
    [SerializeField] private float f_Damage;
    [SerializeField] private float f_FireDistance = 50;

    [SerializeField] private int i_MaxAmmoRemain = 250;
    [SerializeField] private int i_AmmoRemain = 100;
    [SerializeField] private int i_MagCapacity = 25;
    [SerializeField] private int i_MagAmmo;
    [SerializeField] private float f_TimeBetFire = 0.12f;
    [SerializeField] private float f_ReloadTime = 1.8f;
    [SerializeField] private float f_LastFireTime;

    private LineRenderer bulletLineRenderer;

    private void Awake()
    {
        gunAudioPlayer = GetComponent<AudioSource>();
        bulletLineRenderer = GetComponent<LineRenderer>();

        bulletLineRenderer.positionCount = 2;
        bulletLineRenderer.enabled = true;
    }

    private void OnEnable()
    {
        i_MagAmmo = i_MagCapacity;
        e_State = STATE.STATE_READY;
        f_LastFireTime = 0;
    }

    public string AutoType()
    {
        if (e_Type == TYPE.TYPE_FULLAUTO)
            return "FULLAUTO";
        else
            return "SEMIAUTO";
    }

    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        muzzleFlashEffect.Play();
        shellEjectEffect.Play();
        gunAudioPlayer.PlayOneShot(shotClip);
        //라인렌더러 온오프
        bulletLineRenderer.SetPosition(0, fireTransform.position);
        bulletLineRenderer.SetPosition(1, hitPosition);
        bulletLineRenderer.enabled = true;

        yield return new WaitForSeconds(0.03f);

        bulletLineRenderer.enabled = false;
    }

    public void Fire()
    {
        if (e_State == STATE.STATE_READY && Time.time >= f_LastFireTime + f_TimeBetFire)
        {
            f_LastFireTime = Time.time;
            Shot();
        }
        else if(e_State == STATE.STATE_EMPTY)
        {
            Reload();
        }
    }

    public void Shot()
    {
        RaycastHit hit;
        Vector3 hitPosition = Vector3.zero;

        if(Physics.Raycast(fireTransform.position, fireTransform.forward, out hit, f_FireDistance))
        {
            I_Damageable target = hit.collider.GetComponent<I_Damageable>();
            if(target != null)
            {
                target.OnDamage(f_Damage, hit.point, hit.normal);
            }
            hitPosition = hit.point;
        }
        else
        {
            hitPosition = fireTransform.position + fireTransform.forward * f_FireDistance;
        }
        StartCoroutine(ShotEffect(hitPosition));

        i_MagAmmo--;

        if(i_MagAmmo <= 0)
        {
            e_State = STATE.STATE_EMPTY;
        }
    }

    public bool Reload()
    {
        if (e_State == STATE.STATE_RELOADING || i_AmmoRemain <= 0 || i_MagAmmo >= i_MagCapacity) return false;

        StartCoroutine(ReloadRoutine());
        return true;
    }

    public bool Ammo_Limit()
    {
        if (i_AmmoRemain >= i_MaxAmmoRemain) return true;
        else return false;
    }
    public void GetAmmo(int m_i_newAmmo)
    {
        i_AmmoRemain += m_i_newAmmo;
        if (i_AmmoRemain > i_MaxAmmoRemain) i_AmmoRemain = i_MaxAmmoRemain;
    }

    private IEnumerator ReloadRoutine()
    {
        e_State = STATE.STATE_RELOADING;

        gunAudioPlayer.PlayOneShot(reloadClip);

        yield return new WaitForSeconds(f_ReloadTime);

        int AmmoToFill = i_MagCapacity - i_MagAmmo;

        if (i_AmmoRemain < AmmoToFill)
        {
            AmmoToFill = i_AmmoRemain;
        }
        i_MagAmmo += AmmoToFill;
        i_AmmoRemain -= AmmoToFill;

        e_State = STATE.STATE_READY;
    }
}
