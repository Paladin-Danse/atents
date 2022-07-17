using UnityEngine;
using System.Collections;

public class Throwing_Axe : MonoBehaviour {
    public float Throw_Power;
    Axe_Attack AA;

    AudioSource AS;
    public AudioClip[] Hit;
    public GameObject Effect_Attack;
    // Use this for initialization
    void Start () {
        AS = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
        AA = GameObject.FindGameObjectWithTag("Player").GetComponent<Axe_Attack>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.Translate(Vector2.right * Throw_Power);
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == ("Seedling"))
        {
            Hit_Sound();
            Attack_Effect_On(Effect_Attack);
            Destroy(other.gameObject.GetComponent<Collider2D>());
            other.GetComponent<Animator>().SetTrigger("InDead");
            Destroy(gameObject);
        }
        if(other.tag == ("Stone"))
        {
            Hit_Sound();
            Attack_Effect_On(Effect_Attack);
            Destroy(gameObject);
        }
        if (other.tag == ("Tree"))
        {
            Hit_Sound();
            Attack_Effect_On(Effect_Attack);
            Destroy(gameObject);
        }
        if(other.tag == "Monster")
        {
            Hit_Sound();
            Attack_Effect_On(Effect_Attack);
            other.GetComponent<HP_Enable>().HPLost(AA.Throw_Damage);
            Destroy(gameObject);
        }
        if(other.tag == "Projectile")
        {
            Hit_Sound();
            Attack_Effect_On(Effect_Attack);
            other.GetComponent<Projectile>().Destroy_Object();
            Destroy(gameObject);
        }
    }

    void Hit_Sound()
    {
        int R_Num = Random.Range(0, Hit.Length);
        AS.clip = Hit[R_Num];
        AS.Play();
    }
    void Attack_Effect_On(GameObject Effect)
    {
        Instantiate(Effect, gameObject.transform.position, Quaternion.identity);
    }
}
