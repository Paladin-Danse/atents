using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Spawn_Unit : MonoBehaviour {
    public int Unit_Cost;
    public GameObject Unit;

    public Image CoolTime_Image;
    public float _Time;
    public float dTime;
    public bool Cool_On = true;

    // Use this for initialization
    void Start () {
        CoolTime_Image = transform.Find("CoolTime_Image").GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
	    if(CoolTime_Image.fillAmount != 0f)
        {
            dTime += Time.deltaTime;
            CoolTime_Image.fillAmount = 1 - (dTime/_Time);
        }
	}


    //다른 곳에서 호출하는 함수들
    public int GetCost()
    {
        return Unit_Cost;
    }

    public float GetTime()
    {
        return _Time;
    }

    public bool Set_Cool(bool Cool)
    {
        return Cool_On = Cool;
    }

    public bool Get_Cool()
    {
        return Cool_On;
    }
    public void CoolTime_Visual()
    {
        CoolTime_Image.fillAmount = 1;
        dTime = 0;
    }
}
