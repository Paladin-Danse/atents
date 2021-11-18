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
    
    
    [SerializeField] private float f_Damage;//���� ������
    [SerializeField] private float f_SupDamage;//���� ������
    [SerializeField] private float f_FireDistance = 50;//���� ��Ÿ�
    [SerializeField] private int i_MaxAmmoRemain = 250;//���� �ִ�ź��
    [SerializeField] private int i_AmmoRemain = 100;//���� ����ź��
    [SerializeField] private int i_MagCapacity = 25;//���� źâũ��
    [SerializeField] private int i_MagAmmo;//źâ�� ����ź��
    [SerializeField] private float f_TimeBetFire = 0.12f;//�߻�ӵ�
    [SerializeField] private float f_ReloadTime = 1.8f;//���� �����ӵ�
    [SerializeField] private float f_LastFireTime;//���� ���������� �ݹ��� �ð�
    [SerializeField] private float f_TimeToEffect = 0.03f;//����Ʈ �߻� �ð�
    [SerializeField] private float f_Accuracy;//���� ��Ȯ��
    [SerializeField] private float f_Recoil;//���� �ݵ�

    //���� �߻����������, ��������� ǥ�õǴ� ����� �޸� ������� ���� �� �ʿ䰡 ������ ���ƿ� ������ ����
    private LineRenderer bulletLineRenderer;

    private void Awake()
    {
        gunAudioPlayer = GetComponent<AudioSource>();
        //bulletLineRenderer = GetComponent<LineRenderer>();

        i_MagAmmo = i_MagCapacity;
    }

    private void Start()
    {
        /*
        bulletLineRenderer.positionCount = 2;//�ҷ� ���η������� �ѱ��� ���� ��Ÿ���ŭ �ΰ��� ������ �ʿ�
        bulletLineRenderer.enabled = true;//���η������� Ȱ��ȭ
        */

        //���� �⺻��ġ ����(���� OnEnable�̾����� ���� �ٲ� ������ ���� ������ �Ǳ⿡ Awake�� ǥ���ϰ� Scene�� ���� �θ� �� ������ �������, �ش� �ڵ带 �ٽ� �θ���.)

        e_State = STATE.STATE_READY;
    }

    private void Update()
    {
        if(transform.rotation != transform.parent.parent.rotation) RecoilBackupRotation();
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
        /*
        bulletLineRenderer.SetPosition(0, fireTransform.position);
        bulletLineRenderer.SetPosition(1, hitPosition);
        bulletLineRenderer.enabled = true;
        */
        yield return new WaitForSeconds(f_TimeToEffect);

        //bulletLineRenderer.enabled = false;
    }

    private void RecoilBackupRotation()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, transform.parent.parent.rotation, 0.2f);
    }

    //����Է��� �޾����� ����.
    public void Fire()
    {
        //���� �غ�����̰�, ������ �߻�ð����κ��� �߻�ӵ���ŭ�� �ð��� �����ٸ�
        if (e_State == STATE.STATE_READY && Time.time >= f_LastFireTime + f_TimeBetFire)
        {
            f_LastFireTime = Time.time;
            Shot();

            //�ݵ�
            UIManager.instance.CrosshairRecoil(f_Recoil);
            transform.rotation *= Quaternion.Euler(f_Recoil * -0.1f, 0, 0);
        }
        //źâ�� ����ִ� �����̸� �ڵ����� �������� ��ħ.
        else if(e_State == STATE.STATE_EMPTY)
        {
            Reload();
        }
    }
    //���
    public void Shot()
    {
        RaycastHit hit;//�浹ü
        Vector3 hitPosition = Vector3.zero;//�浹��ġ
        Vector3 aimCenter;//������ �����ϰ� �ִ� ��ġ

        //���� ������ ����
        //������ ��������ŭ�� �Ѿ��� Ƣ��������� Ȯ���� ������ ���� �Ѿ��� Ƣ�� ������ ��Ȯ�ϰ� ������ ���� ����.
        //������ �����غ��̴� ������ ������ �׳��� ���Ӿȿ��� ��µ��� ����ó�� ���δ�.

        //Random.insideUnitCircle�� ������� ���� �ڵ�. �簢�� ������ �Ǵ°� ���� ����.
        //Debug.Log(string.Format("Before - aimCenter.x : {0}, aimCenter.y : {1}, aimCenter.z : {2}", aimCenter.x, aimCenter.y, aimCenter.z));
        /*
        //���ڼ� ũ�⸦ �������� �����ϰ� ���� ������.(ex : size = 90�̸� �⺻ũ�� 60��ŭ ���� -15���� 15�ȿ��� �������� ������.)
        var randAim = new Vector3(Random.Range(-(UIManager.instance.CrosshairReturnSize() - 60f) * 0.5f, (UIManager.instance.CrosshairReturnSize() - 60f) * 0.5f),
                                  Random.Range(-(UIManager.instance.CrosshairReturnSize() - 60f) * 0.5f, (UIManager.instance.CrosshairReturnSize() - 60f) * 0.5f),
                                  0);
        */
        //Debug.Log(UIManager.instance.CrosshairReturnSize());

        //Vector3 finalAimVector = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, f_FireDistance) + randAim;

        //Random.insideUnitCircle�� ����� �ڵ�. ���� ������ �׷������� �߸��� ����ΰ��� ������ ������ ��.
        Vector3 randAim = Random.insideUnitCircle * ((UIManager.instance.CrosshairReturnSize() - 60f) * 0.5f);

        //Debug.Log(string.Format("Random.insideUnitCircle : {0}, CrosshairSize : {1}", Random.insideUnitCircle, UIManager.instance.CrosshairReturnSize() - 60f));

        Vector3 finalAimVector = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, f_FireDistance) + randAim;
        //finalAimVector.z = f_FireDistance;

        Debug.Log(string.Format("x : {0}, y : {1}, z : {2}", finalAimVector.x, finalAimVector.y, finalAimVector.z));

        //ȭ�� ���߾ӿ� ������ ��ġ�ϰ� �ְ� ���߾ӿ��� ���� ����� ��, �ε����� ��ü�� �ִٸ� �ش� ��ü�� ��� �ؾ��Ѵ�.(��Ȯ���� ���ӿ� ���� ��ü)
        if (Physics.Raycast(Camera.main.ScreenPointToRay(finalAimVector), out hit))
        {
            aimCenter = hit.point;
        }
        else//���� �ε��� ��ü�� ���ٸ� ȭ�� ���߾ӿ��� �ش����� ��Ÿ��� ������ ��ġ�� ������ ��ġ�� ��´�.
        {
            aimCenter = Camera.main.ScreenToWorldPoint(finalAimVector);
        }

        //���� �׷��� �浹�ϴ� ��ü�� �ִٸ�
        if (Physics.Raycast(fireTransform.position, (aimCenter - fireTransform.position), out hit, f_FireDistance))
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
            hitPosition = fireTransform.position + (aimCenter - fireTransform.position) * f_FireDistance;//�ε����� ��ü�� ���� ��� �浹��ġ�� �ѱ�+���� ��Ÿ��� ����
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

    public void ReloadCancel()
    {
        if(e_State == STATE.STATE_RELOADING)
        {
            if (i_MagAmmo <= 0)
            {
                e_State = STATE.STATE_EMPTY;
            }
            else
            {
                e_State = STATE.STATE_READY;
            }
            StopCoroutine(ReloadRoutine());
        }
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
