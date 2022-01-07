using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SHOT_TYPE//총이 자동화기인지, 반자동화기인지 구분
    {
        TYPE_FULLAUTO,
        TYPE_SEMIAUTO
    }
public enum STATE//총의 현재 상태를 구분
{
    STATE_READY,
    STATE_EMPTY,
    STATE_RELOADING
}

public class Gun : MonoBehaviour
{
    public STATE e_State { get; private set; }
    [SerializeField] private SHOT_TYPE e_Type;

    [SerializeField] private Transform fireTransform;//총구 발사위치
    [SerializeField] private Transform leftHandle;//실제 왼손위치값
    public Transform LeftHandle { get { return leftHandle; } }//왼손위치 호출
    [SerializeField] private Transform rightHandle;//실제 오른손위치값
    public Transform RightHandle { get { return rightHandle; } }//오른손위치 호출

    [SerializeField] private ParticleSystem muzzleFlashEffect;//총의 섬광효과
    [SerializeField] private ParticleSystem shellEjectEffect;//탄피배출효과

    [SerializeField] private AudioSource gunAudioPlayer;//총의 AudioSource
    [SerializeField] private Animator playerAnimator;//재장전을 위해 가져온 플레이어 애니메이터
    [SerializeField] private AudioClip shotClip;//격발클립
    [SerializeField] private AudioClip reloadClip;//장전클립
    
    [SerializeField] private float f_Damage;//총의 데미지
    [SerializeField] private float f_SupDamage;//제압 데미지
    [SerializeField] private float f_FireDistance = 50;//총의 사거리
    [SerializeField] private int i_MaxAmmoRemain = 250;//총의 최대탄약
    [SerializeField] private int i_AmmoRemain = 100;//총의 현재탄약
    [SerializeField] private int i_MagCapacity = 25;//총의 탄창크기
    [SerializeField] private int i_MagAmmo;//탄창의 현재탄약
    [SerializeField] private float f_TimeBetFire = 0.12f;//발사속도
    [SerializeField] private float f_ReloadTime = 1.8f;//총의 장전속도
    [SerializeField] private float f_LastFireTime;//총이 마지막으로 격발한 시간
    [SerializeField] private float f_TimeToEffect = 0.03f;//이펙트 발생 시간
    [SerializeField] [Range(0, 100)]private float f_Accuracy;//총의 정확도
    [SerializeField] [Range(0, 100)]private float f_Recoil;//총의 반동
    private bool b_Reload;

    //총의 발사궤적이지만, 노란색으로 표시되는 현재와 달리 어느정도 진행 후 필요가 없어질 무렵에 제거할 예정
    private LineRenderer bulletLineRenderer;

    private void Awake()
    {
        gunAudioPlayer = GetComponent<AudioSource>();
        playerAnimator = GetComponentInParent<Animator>();
        if (!playerAnimator) Debug.Log("playerAnimator is Not Found");
        bulletLineRenderer = GetComponent<LineRenderer>();

        i_MagAmmo = i_MagCapacity;
    }

    private void Start()
    {
        bulletLineRenderer.positionCount = 2;//불렛 라인렌더러에 총구와 총의 사거리만큼 두개의 지점이 필요
        bulletLineRenderer.enabled = false;//라인렌더러를 비활성화
        playerAnimator.SetBool("Reload", false);

        //총의 기본수치 정의(원래 OnEnable이었으나 총을 바꿀 때마다 총이 장전이 되기에 Awake에 표현하고 Scene을 새로 부를 때 문제가 있을경우, 해당 코드를 다시 부른다.)

        e_State = STATE.STATE_READY;
    }

    //일반 업데이트를 사용했을 경우 반동으로 올라간 각도가 다시 되돌아오기까지의 시간이 그때마다 다름.
    private void FixedUpdate()
    {
        if(transform.rotation != transform.parent.parent.rotation) RecoilBackupRotation();
    }

    private void OnEnable()
    {
        f_LastFireTime = 0;
        UIManager.instance.UpdateAmmoText(i_MagAmmo, i_AmmoRemain);//총이 바뀔 때마다 각 총의 탄약으로 UI를 업데이트 함.
        UIManager.instance.SetGunAccuracy(f_Accuracy);//총을 꺼낼 때 십자선 크기를 총의 적중률에 맞춰 조절한다.
    }

    //현재 총이 무슨 타입의 총인지 다른 스크립트에 반환
    public string AutoType()
    {
        if (e_Type == SHOT_TYPE.TYPE_FULLAUTO)
            return "FULLAUTO";
        else
            return "SEMIAUTO";
    }

    //사격했을 때 보여줄 효과를 표현
    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        muzzleFlashEffect.Play();
        shellEjectEffect.Play();
        gunAudioPlayer.PlayOneShot(shotClip);

        //라인렌더러 온오프
        
        bulletLineRenderer.SetPosition(0, fireTransform.position);
        bulletLineRenderer.SetPosition(1, hitPosition);
        bulletLineRenderer.enabled = true;
        
        yield return new WaitForSeconds(f_TimeToEffect);

        bulletLineRenderer.enabled = false;
    }

    //반동으로 올라간 각도를 RotateTowards를 사용해 최대한 자연스럽게 원래 각도로 되돌린다.
    private void RecoilBackupRotation()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, transform.parent.parent.rotation, 0.8f);
    }

    //사격입력을 받았을때 들어옴.
    public void Fire()
    {
        //총이 준비상태이고, 마지막 발사시간으로부터 발사속도만큼의 시간이 지났다면
        if (e_State == STATE.STATE_READY && Time.time >= f_LastFireTime + f_TimeBetFire && gameObject.activeSelf)
        {
            f_LastFireTime = Time.time;
            Shot();

            //반동
            UIManager.instance.CrosshairRecoil(f_Recoil);
            transform.rotation *= Quaternion.Euler(f_Recoil * -0.1f, 0, 0);
        }
        //탄창이 비어있는 상태이면 자동으로 재장전을 거침.
        else if(e_State == STATE.STATE_EMPTY)
        {
            Reload();
        }
    }
    //사격
    public void Shot()
    {
        RaycastHit hit;//충돌체
        Vector3 hitPosition = Vector3.zero;//충돌위치
        Vector3 aimCenter;//에임이 조준하고 있는 위치
        int PierceObject = 1 << LayerMask.NameToLayer("Penetrable");
        PierceObject = ~PierceObject;

        Vector3 randAim = Random.insideUnitCircle * ((UIManager.instance.CrosshairReturnSize() - 60f) * 0.5f);

        //Debug.Log(string.Format("Random.insideUnitCircle : {0}, CrosshairSize : {1}", Random.insideUnitCircle, UIManager.instance.CrosshairReturnSize() - 60f));

        Vector3 finalAimVector = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, f_FireDistance) + randAim;
        //finalAimVector.z = f_FireDistance;

        //화면 정중앙엔 에임이 위치하고 있고 정중앙에서 선을 쏘았을 때, 부딪히는 물체가 있다면 해당 물체를 쏘게 해야한다.(정확히는 에임에 들어온 물체)
        if (Physics.Raycast(Camera.main.ScreenPointToRay(finalAimVector), out hit, Mathf.Infinity, PierceObject))
        {
            aimCenter = hit.point;
        }
        else//만약 부딪힌 물체가 없다면 화면 정중앙에서 해당총의 사거리가 끝나는 위치를 에임의 위치로 삼는다.
        {
            aimCenter = Camera.main.ScreenToWorldPoint(finalAimVector);
        }

        //선을 그려서 충돌하는 물체가 있다면
        if (Physics.Raycast(fireTransform.position, (aimCenter - fireTransform.position), out hit, f_FireDistance, PierceObject))
        {
            I_Damageable target = hit.collider.GetComponent<I_Damageable>();//대미지를 입는 오브젝트인 경우만
            if(target != null)
            {
                target.OnDamage(f_Damage, hit.point, fireTransform.position - hit.point);
            }
            hitPosition = hit.point;
        }
        else
        {
            hitPosition = fireTransform.position + (aimCenter - fireTransform.position) * f_FireDistance;//부딪히는 물체가 없는 경우 충돌위치를 총구+총의 사거리로 지정
        }
        StartCoroutine(ShotEffect(hitPosition));//사격효과 코루틴

        i_MagAmmo--;
        UIManager.instance.UpdateAmmoText(i_MagAmmo, i_AmmoRemain);

        //탄약이 바닥났을 경우
        if(i_MagAmmo <= 0)
        {
            e_State = STATE.STATE_EMPTY;
        }
    }

    public bool Reload()
    {
        //현재상태가 재장전중이거나,
        //현재 총의 탄약이 바닥 났거나,
        //탄창의 탄약이 가득한경우에는 재장전을 실행하지 않는다.
        if (e_State == STATE.STATE_RELOADING || i_AmmoRemain <= 0 || i_MagAmmo >= i_MagCapacity) return false;

        StartCoroutine(ReloadRoutine());
        return true;
    }

    //재장전을 할 때 총을 바꿀경우, 재장전이 취소된 총이 고장났음. 이외에도 고장이 나는 경우를 방지하기 위해 해당함수를 호출할 것.
    public void ReloadCancel()
    {
        if(e_State == STATE.STATE_RELOADING)
        {
            if (i_MagAmmo <= 0)//총알이 비어있는 상태에서 총을 바꿀 땐, 반드시 EMPTY로 바꿔야한다. 아니면 재장전이 취소된 총은 남은 탄약이 0발이라도 반드시 한발은 쏘게 됨.
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

    //들고있을 수 있는 탄약이 최대치보다 많을 경우
    public bool Ammo_Limit()
    {
        if (i_AmmoRemain >= i_MaxAmmoRemain) return true;
        else return false;
    }

    //탄약 획득
    public void GetAmmo(int m_i_newAmmo)
    {
        i_AmmoRemain += m_i_newAmmo;
        if (i_AmmoRemain > i_MaxAmmoRemain) i_AmmoRemain = i_MaxAmmoRemain;
        if(gameObject.activeSelf) UIManager.instance.UpdateAmmoText(i_MagAmmo, i_AmmoRemain);//현재 활성화가 되어있는 총만 UI업데이트를 한다.
    }

    private IEnumerator ReloadRoutine()
    {
        e_State = STATE.STATE_RELOADING;

        gunAudioPlayer.PlayOneShot(reloadClip);
        playerAnimator.SetBool("Reload", true);

        yield return new WaitForSeconds(f_ReloadTime);

        //장전할때 들어올 탄약은 탄창크기에서 현재탄약만큼 제하고, 현재 들고있는 탄약이 장전해야되는 탄약보다 적으면 거기서 다시한번 빼고 장전한다.
        int AmmoToFill = i_MagCapacity - i_MagAmmo;

        if (i_AmmoRemain < AmmoToFill)
        {
            AmmoToFill = i_AmmoRemain;
        }
        i_MagAmmo += AmmoToFill;
        i_AmmoRemain -= AmmoToFill;

        UIManager.instance.UpdateAmmoText(i_MagAmmo, i_AmmoRemain);
        
        playerAnimator.SetBool("Reload", false);

        e_State = STATE.STATE_READY;
    }
}
