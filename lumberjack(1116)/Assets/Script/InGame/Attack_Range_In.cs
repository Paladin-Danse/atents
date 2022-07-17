using UnityEngine;
using System.Collections;

public class Attack_Range_In : MonoBehaviour {
    Axe_Attack AA;
    AudioSource AS;

    public AudioClip[] Hit;

    public GameObject Effect_Attack;
	// Use this for initialization
	void Start () {
        AS = GetComponent<AudioSource>();
        AA = GameObject.FindGameObjectWithTag("Player").GetComponent<Axe_Attack>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == ("Seedling"))
        {
            Hit_Sound();
            Attack_Effect_On(Effect_Attack);
            Destroy(other.gameObject.GetComponent<Collider2D>());
            other.GetComponent<Animator>().SetTrigger("InDead");
        }
        if (other.tag == ("Tree"))
        {
            Hit_Sound();
            Attack_Effect_On(Effect_Attack);
            Destroy(other.gameObject);
        }
        if(other.tag == "Projectile")
        {
            Hit_Sound();
            Attack_Effect_On(Effect_Attack);
            other.GetComponent<Projectile>().Destroy_Object();
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
