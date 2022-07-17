using UnityEngine;
using System.Collections;

public class Spawn_Cube : MonoBehaviour {
    
    public ResourceManager RM;
    public Object[] UnitList;
    public Spawn_Unit SU;
    GameManager GM;
    
    public float R_Spawn_Y;

    
    bool Button_Down = false;
    bool Resource_none = false;

    int List_Num = 0;
    

    Vector3 RandPosition;

	// Use this for initialization
	void Start () {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
	    if(Button_Down && SU.Get_Cool() && RM.GetResource_none() == false)//버튼을 눌렀고 & 쿨타임이 돌아왔으며 & 자원없음이 false일때
        {
            if(GM.GetGameState() == "Play")
            {
                StartCoroutine("Cube_Spawn", SU);//유닛소환 코루틴시작
                Button_Down = false;
            }
        }
        else
        {
            Button_Down = false;
        }
	}

    IEnumerator Cube_Spawn(Spawn_Unit SU)
    {
        float RandomY = Random.Range(-R_Spawn_Y, R_Spawn_Y);
        RandPosition = new Vector3(transform.position.x, transform.position.y + RandomY, transform.position.z + RandomY);
        GameObject Unit = Instantiate(SU.Unit, RandPosition, Quaternion.identity);//유닛 소환하고 리스트에 저장
        RM.Resource_Lost();
        SU.CoolTime_Visual();

        if (Unit.GetComponent<Collider2D>())
        {
            ProgressTo_By_Unit(Unit.GetComponent<Collider2D>());
        }
        else
        {
            ProgressTo_By_Unit(Unit.GetComponentInChildren<Collider2D>());
        }
        SU.Set_Cool(false);//현재 유닛쿨타임이 돌고 있음
        
        yield return new WaitForSeconds(SU.GetTime());//쿨타임 도는중...
        SU.Set_Cool(true);//쿨타임이 끝남
    }

    public void OnButton(Spawn_Unit spawn_unit)
    {
        SU = spawn_unit;
        if (Button_Down == false && SU.Get_Cool())
        {
            RM.Resource_Cost();
            Button_Down = true;
        }
    } 

    public int GetCost()
    {
        return SU.GetCost(); 
    }

    public void GetList(Object[] List)
    {
        List = UnitList;
    }

    void ProgressTo_By_Unit(Collider2D Unit)
    {
        ProgressTo progress;
        progress = GameObject.Find("GameManager").GetComponent<ProgressTo>();
        progress.GetUnitList(Unit);
    }
}
