using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResourceManager : MonoBehaviour {
    
    int Resource = 0;
    public int Upgrade1_Point;
    public int Upgrade2_Point;
    public int Upgrade3_Point;
    int Upgrade_State = 1;

    public Text ResourceText;
    public Text Max_Resource_Text;
    string text;

    Image Upgrade_Button_Image;
    public Sprite Upgrade2_Sprite;
    public Sprite Upgrade3_Sprite;

    public Spawn_Cube SC;
    GameManager GM;

    bool Resource_none = false;

    int Min_Res = 0;
    int Max_Res = 50;

    // Use this for initialization
    void Start () {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        Upgrade_Button_Image = GameObject.Find("Upgrade").GetComponent<Image>();
        Max_Resource_Text.text = Max_Res.ToString();
	}
	
	// Update is called once per frame
	void Update () {
        ResourceText.text = Resource.ToString();
        if (Input.GetKeyDown(KeyCode.A))
        {
            Resource_Get();
        }
    }

    public void Resource_Get()
    {
        if (GM.GetGameState() == "Play")
        {
            Resource += Upgrade_State * 2;//보급물자강화상태만큼 자원획득
            if (Resource > Max_Res)
            {
                Resource = Max_Res;
            }
        }
    }

    public void Resource_Cost()
    {
        if(Resource >= SC.GetCost())
        {
            Resource_none = false;//자원이 있음
        }
        else
        {
            Resource_none = true;//자원이 없음
        }
    }

    public bool GetResource_none()//자원의 상태를 보고함
    {
        return Resource_none;
    }

    public void Resource_Lost()//자원을 잃음
    {
        Resource -= SC.GetCost();
    }

    public void Resource_Upgrade()
    {

        if(Upgrade_State == 1 && Upgrade1_Point <= Resource)//보급물자강화가 아직 1단계이고, 1단계업그레이드 요구수치보다 자원이 많다면
        {
            Upgrade_State++;//강화
            Max_Res = 100;
            Resource -= Upgrade1_Point;//자원에서 요구수치만큼 뺌
            Upgrade_Button_Image.sprite = Upgrade2_Sprite;
            Max_Resource_Text.text = Max_Res.ToString();
        }
        if(Upgrade_State == 2 && Upgrade2_Point <= Resource)//2단계 -> 3단계
        {
            Upgrade_State++;
            Max_Res = 200;
            Resource -= Upgrade2_Point;
            Upgrade_Button_Image.sprite = Upgrade3_Sprite;
            Max_Resource_Text.text = Max_Res.ToString();
        }
        if (Upgrade_State == 3 && Upgrade3_Point <= Resource)//3단계 -> 4단계
        {
            Upgrade_State = 5;//예외로 5단계가 됨. Upgrade_State만큼 자원을 획득하기 때문
            Max_Res = 500;
            Upgrade_Button_Image.gameObject.SetActive(false);
            Resource -= Upgrade3_Point;
            Max_Resource_Text.text = Max_Res.ToString();
        }
    }
}
