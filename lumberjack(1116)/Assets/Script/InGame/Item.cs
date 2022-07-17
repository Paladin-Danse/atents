using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {
    public float Heal_point;
    public float AP_Up_Point;

    public bool On_HP_Small_Up;
    public bool On_HP_Big_Up;
    public bool On_AP_Up;
    public bool On_Shield;

    public GameObject Effect_Moru_Small;
    public GameObject Effect_Moru_Big;
    public GameObject Effect_AP_Up;
    public GameObject Effect_Shield;

    AudioSource AS;
    public AudioClip Sound_Moru_Small;
    public AudioClip Sound_Moru_Big;
    public AudioClip Sound_AP_Up;
    public AudioClip Sound_Shield;

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            AS = other.GetComponent<AudioSource>();
            if (On_HP_Small_Up)
            {
                other.GetComponent<Player>().HPGet(Heal_point);
                if (Effect_Moru_Small)
                {
                    other.GetComponent<Player>().HP_Effect_On(Effect_Moru_Small);
                }
                if(Sound_Moru_Small)
                {
                    AS.clip = Sound_Moru_Small;
                    AS.Play();
                }
                Destroy(gameObject);
            }
            if (On_HP_Big_Up)
            {
                other.GetComponent<Player>().HPGet(Heal_point);
                if (Effect_Moru_Big)
                {
                    other.GetComponent<Player>().HP_Effect_On(Effect_Moru_Big);
                }
                if (Sound_Moru_Big)
                {
                    AS.clip = Sound_Moru_Big;
                    AS.Play();
                }
                Destroy(gameObject);
            }
            if (On_AP_Up)
            {
                other.GetComponent<Axe_Attack>().AP_Up(AP_Up_Point);
                if (Effect_AP_Up)
                {
                    other.GetComponent<Player>().AP_Effect_On(Effect_AP_Up);
                }
                if (Sound_AP_Up)
                {
                    AS.clip = Sound_AP_Up;
                    AS.Play();
                }
                Destroy(gameObject);
            }
            if(On_Shield)
            {
                other.GetComponent<Player>().Shiled_Get();
                if (Effect_Shield)
                {
                    other.GetComponent<Player>().Shield_Effect_On(Effect_Shield);
                }
                if (Sound_Shield)
                {
                    AS.clip = Sound_Shield;
                    AS.Play();
                }
                Destroy(gameObject);
            }
        }
        
    }
}
