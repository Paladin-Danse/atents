using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SHOT_TYPE//���� �ڵ�ȭ������, ���ڵ�ȭ������ ����
    {
        TYPE_FULLAUTO,
        TYPE_SEMIAUTO
    }

public class Gun : MonoBehaviour
{
    public enum STATE//���� ���� ���¸� ����
    {
        STATE_READY,
        STATE_EMPTY,
        STATE_RELOADING
    }
    [SerializeField] private STATE e_State;    
    [SerializeField] private SHOT_TYPE e_Type;

    [SerializeField] private Transform fireTransform;//�ѱ� �߻���ġ
    [SerializeField] private Transform leftHandle;//���� �޼���ġ��
    public Transform LeftHandle { get { return leftHandle; } }//�޼���ġ ȣ��
    [SerializeField] private Transform rightHandle;//���� ��������ġ��
    public Transform RightHandle { get { return rightHandle; } }//��������ġ ȣ��

    [SerializeField] private ParticleSystem muzzleFlashEffect;//���� ����ȿ��
    [SerializeField] private ParticleSystem shellEjectEffect;//ź�ǹ���ȿ��

    [SerializeField] private AudioSource gunAudioPlayer;//���� AudioSource
    [SerializeField] private AudioClip shotClip;//�ݹ�Ŭ��
    [SerializeField] private AudioClip reloadClip;//����Ŭ��
    
    [SerializeField] private float f_Damage;//���� �����
    [SerializeField] private float f_SupDamage;
    [SerializeField] private float f_FireDistance = 50;//���� ��Ÿ�

    [SerializeField] private int i_MaxAmmoRemain = 250;//���� �ִ�ź��
    [SerializeField] private int i_AmmoRemain = 100;//���� ����ź��
    [SerializeField] private int i_MagCapacity = 25;//���� źâũ��
    [SerializeField] private int i_MagAmmo;//źâ�� ����ź��
    [SerializeField] private float f_TimeBetFire = 0.12f;//�߻�ӵ�
    [SerializeField] private float f_ReloadTime = 1.8f;//���� �����ӵ�
    [SerializeField] private float f_LastFireTime;//���� ���������� �ݹ��� �ð�
    [SerializeField] private float f_TimeToEffect = 0.03f;//����Ʈ �߻� �ð�

    //���� �߻����������, ��������� ǥ�õǴ� ����� �޸� ������� ���� �� �ʿ䰡 ������ ���ƿ� ������ ����
    private LineRenderer bulletLineRenderer;

    private void Awake()
    {
        gunAudioPlayer = GetComponent<AudioSource>();
        bulletLineRenderer = GetComponent<LineRenderer>();

        bulletLineRenderer.positionCount = 2;//�ҷ� ���η������� �ѱ��� ���� ��Ÿ���ŭ �ΰ��� ������ �ʿ�
        bulletLineRenderer.enabled = true;//���η������� Ȱ��ȭ

        //���� �⺻��ġ ����(���� OnEnable�̾����� ���� �ٲ� ������ ���� ������ �Ǳ⿡ Awake�� ǥ���ϰ� Scene�� ���� �θ� �� ������ �������, �ش� �ڵ带 �ٽ� �θ���.)
        i_MagAmmo = i_MagCapacity;
        e_State = STATE.STATE_READY;
    }

    private void OnEnable()
    {
        f_LastFireTime = 0;
        UIManager.instance.UpdateAmmoText(i_MagAmmo, i_AmmoRemain);//���� �ٲ� ������ �� ���� ź������ UI�� ������Ʈ ��.
    }

    //���� ���� ���� Ÿ���� ������ �ٸ� ��ũ��Ʈ�� ��ȯ
    public string AutoType()
    {
        if (e_Type == SHOT_TYPE.TYPE_FULLAUTO)
            return "FULLAUTO";
        else
            return "SEMIAUTO";
    }

    //������� �� ������ ȿ���� ǥ��
    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        muzzleFlashEffect.Play();
        shellEjectEffect.Play();
        gunAudioPlayer.PlayOneShot(shotClip);

        //���η����� �¿���
        bulletLineRenderer.SetPosition(0, fireTransform.position);
        bulletLineRenderer.SetPosition(1, hitPosition);
        bulletLineRenderer.enabled = true;

        yield return new WaitForSeconds(f_TimeToEffect);

        bulletLineRenderer.enabled = false;
    }

    //����Է��� �޾����� ����.
    public void Fire(Vector3 aimPoint)
    {
        //���� �غ�����̰�, ������ �߻�ð����κ��� �߻�ӵ���ŭ�� �ð��� �����ٸ�
        if (e_State == STATE.STATE_READY && Time.time >= f_LastFireTime + f_TimeBetFire)
        {
            f_LastFireTime = Time.time;
            Shot(aimPoint);
        }
        //źâ�� ����ִ� �����̸� �ڵ����� �������� ��ħ.
        else if(e_State == STATE.STATE_EMPTY)
        {
            Reload();
        }
    }
    //���
    public void Shot(Vector3 aimPoint)
    {
        RaycastHit hit;//�浹ü
        Vector3 hitPosition = Vector3.zero;//�浹��ġ

        //���� �׷��� �浹�ϴ� ��ü�� �ִٸ�
        if(Physics.Raycast(fireTransform.position, fireTransform.forward, out hit, f_FireDistance))
        {
            I_Damageable target = hit.collider.GetComponent<I_Damageable>();//������� �Դ� ������Ʈ�� ��츸
            if(target != null)
            {
                target.OnDamage(f_Damage, hit.point, hit.normal);
            }
            hitPosition = hit.point;
        }
        else
        {
            hitPosition = fireTransform.position + fireTransform.forward * f_FireDistance;//�ε����� ��ü�� ���� ��� �浹��ġ�� �ѱ�+���� ��Ÿ��� ����
        }
        StartCoroutine(ShotEffect(hitPosition));//���ȿ�� �ڷ�ƾ

        i_MagAmmo--;
        UIManager.instance.UpdateAmmoText(i_MagAmmo, i_AmmoRemain);

        //ź���� �ٴڳ��� ���
        if(i_MagAmmo <= 0)
        {
            e_State = STATE.STATE_EMPTY;
        }
    }

    public bool Reload()
    {
        //������°� ���������̰ų�,
        //���� ���� ź���� �ٴ� ���ų�,
        //źâ�� ź���� �����Ѱ�쿡�� �������� �������� �ʴ´�.
        if (e_State == STATE.STATE_RELOADING || i_AmmoRemain <= 0 || i_MagAmmo >= i_MagCapacity) return false;

        StartCoroutine(ReloadRoutine());
        return true;
    }

    //������� �� �ִ� ź���� �ִ�ġ���� ���� ���
    public bool Ammo_Limit()
    {
        if (i_AmmoRemain >= i_MaxAmmoRemain) return true;
        else return false;
    }

    //ź�� ȹ��
    public void GetAmmo(int m_i_newAmmo)
    {
        i_AmmoRemain += m_i_newAmmo;
        if (i_AmmoRemain > i_MaxAmmoRemain) i_AmmoRemain = i_MaxAmmoRemain;
        if(gameObject.activeSelf) UIManager.instance.UpdateAmmoText(i_MagAmmo, i_AmmoRemain);//���� Ȱ��ȭ�� �Ǿ��ִ� �Ѹ� UI������Ʈ�� �Ѵ�.
    }

    private IEnumerator ReloadRoutine()
    {
        e_State = STATE.STATE_RELOADING;

        gunAudioPlayer.PlayOneShot(reloadClip);

        yield return new WaitForSeconds(f_ReloadTime);

        //�����Ҷ� ���� ź���� źâũ�⿡�� ����ź�ุŭ ���ϰ�, ���� ����ִ� ź���� �����ؾߵǴ� ź�ຸ�� ������ �ű⼭ �ٽ��ѹ� ���� �����Ѵ�.
        int AmmoToFill = i_MagCapacity - i_MagAmmo;

        if (i_AmmoRemain < AmmoToFill)
        {
            AmmoToFill = i_AmmoRemain;
        }
        i_MagAmmo += AmmoToFill;
        i_AmmoRemain -= AmmoToFill;

        UIManager.instance.UpdateAmmoText(i_MagAmmo, i_AmmoRemain);
        e_State = STATE.STATE_READY;
    }
}
