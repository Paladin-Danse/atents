using UnityEngine;
using System.Collections;

public class Plan_Spawner : MonoBehaviour {
    public GameObject[] Plan;
    GameObject Next_Plan;
    public GameObject[] Monster_Plan;

    Vector2 Plan_P;
    Vector2 Plan_S;

    //public float Spawn_time;
    float _time;

    int R_Num = 1;

    public int Monster_Spawn_Count;
    int Count;
    int Monster_Count = 0;
    float Next_Plan_ScaleX;
    Vector2 Next_Plan_Position;
    
    // Use this for initialization
    void Start () {
        _time = 0;

        Plan_P = Plan[0].transform.position;
        Plan_S = Plan[0].transform.localScale;
        Plan_Data_Change(Plan[1]);

        Plan_Change();
    }
	
	// Update is called once per frame
	void Update () {
        _time += Time.deltaTime;
        /*
        if(Time.time >= _time + Spawn_time)
        {
            Plan_Change();

            _time = Time.time;
        }
        */
	}

    void Plan_Change()
    {
        Next_Plan = Instantiate(Plan[R_Num], Next_Plan_Position, Quaternion.identity) as GameObject;

        R_Num = Random.Range(1, Plan.Length);

        Count++;
        
        if (Count >= Monster_Spawn_Count)
        {
            R_Num = 0;
        }
        Plan_P = Next_Plan.transform.position;
        Plan_S = Next_Plan.transform.localScale;

        if (Count == Monster_Spawn_Count)
        {
            Plan_Data_Change(Monster_Plan[Monster_Count]);
            //GameObject.Find("GameManager").GetComponent<GameManager>().On_MonsterBattle();
            Next_Plan = Instantiate(Monster_Plan[Monster_Count], Next_Plan_Position, Quaternion.identity);

            Plan_P = Next_Plan.transform.position;
            Plan_S = Next_Plan.transform.localScale;
            
            Monster_Count++;
        }
        Plan_Data_Change(Plan[R_Num]);
    }

    void Plan_Data_Change(GameObject Plan)
    {
        Next_Plan_ScaleX = Plan.transform.localScale.x;
        Next_Plan_Position = new Vector2(Plan_P.x + (Next_Plan_ScaleX * (float)3.14 / 2) + (Plan_S.x * (float)3.14 / 2), Plan_P.y);//크기 오류시 수정
    }

    public void Spawn_Trigger()
    {
        if (_time > 5.0f)
        {
            Plan_Change();

            _time = 0;
        }
    }

    public void Count_Reset()
    {
        Count = 0;
        R_Num = Random.Range(1, Plan.Length);
    }
}
