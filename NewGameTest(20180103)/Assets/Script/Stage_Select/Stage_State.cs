using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Stage_State : MonoBehaviour {
    enum stage_state
    {
        Disable,
        Open,
        Enable
    }
    enum Star_State
    {
        One,
        Two,
        Three
    }
    stage_state SS;
    StageManager SM;
    Medal_Box MB;

    int CS_Number;
    public int My_Stage_Number;

    public Sprite Disable_Sprite;
    public Sprite Open_Sprite;
    public Sprite Enable_Sprite;

    GameObject Star;
    public Sprite NoneStar;
    public Sprite OneStar;
    public Sprite TwoStar;
    public Sprite ThreeStar;

    // Use this for initialization
    void Start () {
        MB = GameObject.Find("GameManager").GetComponent<Medal_Box>();
        SS = stage_state.Disable;
        SM = GameObject.Find("GameManager").GetComponent<StageManager>();
        CS_Number = SM.GetCurrentStage();

        State_Check();

        Star = gameObject.transform.Find("Star").gameObject;
        Sprite_State();

    }
	
	// Update is called once per frame
	void Update () {
	    
	}







    public void State_Check()
    {
        if (CS_Number < My_Stage_Number)
        {
            SS = stage_state.Disable;
        }

        else if (CS_Number == My_Stage_Number)
        {
            SS = stage_state.Open;
        }

        else
        {
            SS = stage_state.Enable;
        }
    }

    public void Sprite_State()
    {
        if(SS == stage_state.Disable)
        {
            GetComponent<UnityEngine.UI.Image>().sprite = Disable_Sprite;
            GetComponent<UnityEngine.UI.Button>().enabled = false;

            Star.gameObject.SetActive(false);
        }
        else if (SS == stage_state.Open)
        {
            GetComponent<UnityEngine.UI.Image>().sprite = Open_Sprite;
            GetComponent<UnityEngine.UI.Button>().enabled = true;

            Star.gameObject.SetActive(true);
            if(MB.Star_Set(My_Stage_Number) == 1)
            {
                Star.GetComponent<Image>().sprite = OneStar;
            }
            else if(MB.Star_Set(My_Stage_Number) == 2)
            {
                Star.GetComponent<Image>().sprite = TwoStar;
            }
            else if(MB.Star_Set(My_Stage_Number) == 3)
            {
                Star.GetComponent<Image>().sprite = ThreeStar;
            }
            else
            {
                Star.GetComponent<Image>().sprite = NoneStar;
            }
        }
        else
        {
            GetComponent<UnityEngine.UI.Image>().sprite = Enable_Sprite;
            GetComponent<UnityEngine.UI.Button>().enabled = true;
            
            Star.gameObject.SetActive(true);
            if (MB.Star_Set(My_Stage_Number) == 1)
            {
                Star.GetComponent<Image>().sprite = OneStar;
            }
            else if (MB.Star_Set(My_Stage_Number) == 2)
            {
                Star.GetComponent<Image>().sprite = TwoStar;
            }
            else if (MB.Star_Set(My_Stage_Number) == 3)
            {
                Star.GetComponent<Image>().sprite = ThreeStar;
            }
            else
            {
                Star.GetComponent<Image>().sprite = NoneStar;
            }
        }
    }
}
