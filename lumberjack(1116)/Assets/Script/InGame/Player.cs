using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    public float Speed;

    HPGauge HPG;
    Jump jump;

    float HP;
    public float MaxHP;
    public float Damage_Time;

    bool Damaged = false;
    bool On_Shiled = false;
    public GameObject On_Shield_Image;

    public Vector3 Effect_Pos;
    public Vector3 Damaged_Pos;

    public GameObject Effect_Blocked;
    public GameObject Effect_Damaged;
    public AudioClip Sound_Blocked;

    public enum Player_State
    {
        Run,
        Attack,
        Dead
    }
    Player_State PS;

    // Use this for initialization
    void Start () {
        HP = MaxHP;
        PS = Player_State.Run;
        HPG = GetComponent<HPGauge>();
        HPG.SetHPMax(HP);
	}

    void FixedUpdate()
    {
        if (PS != Player_State.Dead)
        {
            transform.Translate(Vector2.right * Speed);
        }

        else
        {
            GameManager GM;
            GM = GameObject.Find("GameManager").GetComponent<GameManager>();
            GM.GameOver();

            Background_Move BM;
            BM = GameObject.Find("BackGround").GetComponent<Background_Move>();
            BM.Background_Stop();
        }
    }

    public void HPLost(float Atk_point)//공격을 입고 HP를 잃는 함수
    {
        if(On_Shiled)
        {
            Blocked_Effect_On(Effect_Blocked);
            StartCoroutine("Damage_Flash");
            GetComponent<AudioSource>().clip = Sound_Blocked;
            GetComponent<AudioSource>().Play();
            On_Shiled = false;
            On_Shield_Image.SetActive(false);
            return;
        }
        if (Damaged == false)
        {
            HP -= Atk_point;
            Damaged_Effect_On(Effect_Damaged);
            //DRF.Red_Flash();//데미지 점멸이펙트
            HPG.GaugeState(HP);
            StartCoroutine("Damage_Flash");

            if (HP <= 0)
            {
                PS = Player_State.Dead;
                //Destroy(gameObject);//파괴
            }
        }
    }

    public void HPGet(float Heal_point)
    {
        HP += Heal_point;
        if(MaxHP < HP)
        {
            HP = MaxHP;
        }
        HPG.GaugeState(HP);
    }
    
    public void Shiled_Get()
    {
        On_Shiled = true;
        On_Shield_Image.SetActive(true);
    }


    IEnumerator Damage_Flash()
    {
        Damaged = true;
        //쉴드가 없을경우에만 데미지를 받을때 점멸을 할경우.
        if (!On_Shiled)
        {

        }
        yield return new WaitForSeconds(Damage_Time);
        Damaged = false;
    }










    //이펙트 함수 모음
    public void HP_Effect_On(GameObject Effect)
    {
        GameObject _effect;
        _effect = Instantiate(Effect, gameObject.transform.position + Effect_Pos, Quaternion.identity);
        _effect.transform.parent = gameObject.transform;
    }

    public void AP_Effect_On(GameObject Effect)
    {
        GameObject _effect;
        _effect = Instantiate(Effect, gameObject.transform.position + Effect_Pos, Quaternion.identity);
        _effect.transform.parent = gameObject.transform;
    }
    public void Shield_Effect_On(GameObject Effect)
    {
        GameObject _effect;
        _effect = Instantiate(Effect, gameObject.transform.position + Effect_Pos, Quaternion.identity);
        _effect.transform.parent = gameObject.transform;
    }
    public void Attack_Effect_On(GameObject Effect)
    {
        GameObject _effect;
        _effect = Instantiate(Effect, gameObject.transform.position + Effect_Pos, Quaternion.identity);
        _effect.transform.parent = gameObject.transform;
    }
    void Blocked_Effect_On(GameObject Effect)
    {
        GameObject _effect;
        _effect = Instantiate(Effect, gameObject.transform.position + Damaged_Pos, Quaternion.identity);
        _effect.transform.parent = gameObject.transform;
    }

    void Damaged_Effect_On(GameObject Effect)
    {
        GameObject _effect;
        _effect = Instantiate(Effect, gameObject.transform.position + Damaged_Pos, Quaternion.identity);
        _effect.transform.parent = gameObject.transform;
    }
    
}
