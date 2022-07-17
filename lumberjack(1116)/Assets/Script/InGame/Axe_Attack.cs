using UnityEngine;
using System.Collections;

public class Axe_Attack : MonoBehaviour {
    public GameObject Axe;
    public GameObject GoldAxe;
    public GameObject Effect_Attack;
    GameObject Attack_Range;
    AudioSource AS;

    public float Attack_Time;
    bool Attack_Cool;
    public float Damage;
    public float Throw_Damage;
    public float Throw_CoolTime;
    public float Attack_CoolTime;
    
    public GameObject On_Rampage_Image;

    public AudioClip[] Throw_Sound;

    bool Throw_Cool = false;

    bool On_Rampage;
    float Rampage_Time = 10.0f;
    float _time;

	// Use this for initialization
	void Start () {
        AS = GetComponent<AudioSource>();
        Attack_Range = GameObject.Find("Attack_Range");
        Attack_Range.SetActive(false);
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if(Input.GetKey(KeyCode.DownArrow))
        {
            OnAttack();
        }

	    if(!Attack_Cool)
        {
            Attack_Range.SetActive(false);
        }
        else
        {
            _time += Time.deltaTime;
            if (_time >= Attack_CoolTime)
            {
                Attack_Cool = false;
            }
        }

	}

    public void AxeThrow()
    {
        if(!Throw_Cool)
        {
            StartCoroutine("OnThrow");
        }
    }

    public void AxeAttack()
    {
        if (!Attack_Cool)
        {
            StartCoroutine("OnAttack");
        }
    }

    public void AP_Up(float AP)
    {
        if(!On_Rampage)
        {
            StartCoroutine("AP_Up_Item", AP);
        }
    }

    void OnThrow_Sound()
    {
        int R_Num = Random.Range(0, Throw_Sound.Length);
        AS.clip = Throw_Sound[R_Num];
        AS.Play();
    }

    IEnumerator OnAttack()
    {
        GetComponent<Animator>().SetTrigger("InAttack");
        if (On_Rampage)
        {
            GetComponent<Player>().Attack_Effect_On(Effect_Attack);
        }
        Attack_Range.SetActive(true);
        Attack_Cool = true;
        _time = 0;
        yield return new WaitForSeconds(Attack_Time);
        
        Attack_Range.SetActive(false);
        
    }

    IEnumerator OnThrow()
    {
        GetComponent<Animator>().SetTrigger("InThrow");
        OnThrow_Sound();

        Throw_Cool = true;
        yield return new WaitForSeconds(Throw_CoolTime);
        Throw_Cool = false;
    }

    IEnumerator AP_Up_Item(float AP)
    {
        On_Rampage = true;
        Damage += AP;
        Throw_Damage += AP;
        On_Rampage_Image.SetActive(true);

        yield return new WaitForSeconds(Rampage_Time);

        On_Rampage = false;
        Damage -= AP;
        Throw_Damage -= AP;
        On_Rampage_Image.SetActive(false);
    }

    public void Instantiate_Axe()
    {
        if (!On_Rampage)
        {
            Instantiate(Axe, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(GoldAxe, transform.position, Quaternion.identity);
        }
    }
}
