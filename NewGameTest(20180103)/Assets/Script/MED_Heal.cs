using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MED_Heal : MonoBehaviour {
    public float Heal_Point;
    public float Heal_Time = 1.0f;
    bool Heal_CoolTime = false;

    Heal_Search HS;
    GameManager GM;

    AudioSource AS;
    public AudioClip Heal_Sound;
    // Use this for initialization
    void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        AS = GetComponent<AudioSource>();
        HS = gameObject.transform.Find("Heal_Range").GetComponent<Heal_Search>();
        
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"), true);
    }

    // Update is called once per frame
    void Update()
    {
        if (HS.UnitList.Count != 0 && GM.GetGameState() == "Play")
        {
            foreach (Collider2D Unit in HS.UnitList)
            {
                if (Unit == null)
                {
                    HS.UnitList.Remove(Unit);
                }

                A_to_B AB = Unit.GetComponent<A_to_B>();
                
                if (AB.GetHPMax() > AB.GetHP())
                {
                    GetComponent<A_to_B>().HealContact = true;
                    if (!Heal_CoolTime)
                    {
                        OnHealSound();
                        StartCoroutine("Healing", AB);
                    }
                }
                else
                {
                    GetComponent<A_to_B>().HealContact = false;
                }
            }
        }
        else
        {
            GetComponent<A_to_B>().HealContact = false;
        }
    }

    

    public bool GetCoolTime()
    {
        return Heal_CoolTime;
    }

    IEnumerator Healing(A_to_B AB)
    {
        GetComponent<A_to_B>().US = A_to_B.UnitState.Heal;
        AB.HP_Heal(Heal_Point);
        GetComponent<A_to_B>().Heal_Animation();
        GetComponent<AudioSource>().clip = Heal_Sound;
        GetComponent<AudioSource>().Play();
        Heal_CoolTime = true;

        yield return new WaitForSeconds(Heal_Time);
        GetComponent<A_to_B>().US = A_to_B.UnitState.Idle;
        Heal_CoolTime = false;
    }
    
    void OnHealSound()
    {
        if (Heal_Sound)
        {
            AS.clip = Heal_Sound;
            AS.volume = 1f;
            AS.Play();
        }
        else
        {
            Debug.Log("Not Sound");
        }
    }
}
