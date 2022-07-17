using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Attack_UIButton : MonoBehaviour {
    Axe_Attack axe_attack;
    public Slider Charge_Gauge;

    public float ChargeTime;
    float isTime;

    bool OnMouseDown = false;
    bool Type_Throw = false;
    // Use this for initialization
    void Start () {
        axe_attack = GameObject.Find("Player").GetComponent<Axe_Attack>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
	    if(OnMouseDown)
        {
            isTime += Time.deltaTime;

            if (Charge_Gauge)
            {
                Charge_Gauge.value = (isTime / ChargeTime);
            }

            if (isTime >= ChargeTime)
            {
                Type_Throw = true;
            }
        }
        
        if(!OnMouseDown)
        {
            Charge_Gauge.value = 0;
            isTime = 0;
        }
    }
    

    public void Down()
    {
        isTime = 0;
        if(Charge_Gauge)
        {
            Charge_Gauge.value = 0;
        }
        OnMouseDown = true;
    }

    public void Up()
    {
        OnMouseDown = false;

        if(Charge_Gauge)
        {
            Charge_Gauge.value = 0;
        }

        if (Type_Throw)
        {
            axe_attack.AxeThrow();
            Type_Throw = false;
        }
        else
        {
            axe_attack.AxeAttack();
        }
    }
}
