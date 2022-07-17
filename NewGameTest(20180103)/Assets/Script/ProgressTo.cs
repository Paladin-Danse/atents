using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class ProgressTo : MonoBehaviour {
    public List<Collider2D> UnitList;
    public enum mode
    {
        Defense,
        MilitaryOccupation
    };
    
    public mode Mod;

    float _time;
    public float FrontPositionX = -9999;
    public Collider2D FrontUnit;
    float Max = 9999;
    float Min = -9999;

    Transform StartPosition;
    Transform EndPosition;

    public float MapSize;

    public Slider ProgressBar;

    public float DefensTime;
    // Use this for initialization
    void Start () {

        StartPosition = GameObject.Find("Unit_Spawn").transform;//시작지점위치
        EndPosition = GameObject.Find("Enemy_Spawn").transform;//끝지점위치
        MapSize = Mathf.Abs(StartPosition.position.x) + Mathf.Abs(EndPosition.position.x);//맵크기 = 시작지점 X의 절대값 + 끝지점 X의 절대값
        
	}
	
	// Update is called once per frame
	void Update () {
        if (GetComponent<GameManager>().GetGameState() == "Play")
        {
            /*
            if(Mod == mode.Defense && UnitList != null)//방어전이고 필드에 유닛이 있을때
            {
                if (FrontUnit == null)//맨 앞의 유닛이 죽으면 바로 뒤에 있는 유닛으로 대체
                {
                    FrontUnit = NextFront();
                }
                FrontPositionX = FrontUnit.transform.position.x;
                ProgressBar.value = 1 - ((FrontPositionX + MapSize/2) / MapSize);//진행상황막대 표시 수치
            }

            else if(Mod == mode.Defense && UnitList == null)
            {
                FrontUnit = null;
                FrontPositionX = Max;
            }
            */
            
            if (Mod == mode.MilitaryOccupation && UnitList.Count == 0)
            {
                FrontUnit = null;
                FrontPositionX = Min;
                ProgressBar.value = 0;
            }
            else if (Mod == mode.MilitaryOccupation && UnitList.Count != 0)
            {
                if (FrontUnit == null)//맨 앞의 유닛이 죽으면 바로 뒤에 있는 유닛으로 대체
                {
                    UnitList.Remove(FrontUnit);
                    if (NextFront() != null)
                    {
                        FrontUnit = NextFront();
                    }
                }
                else
                {
                    FrontPositionX = FrontUnit.transform.position.x;
                    ProgressBar.value = ((FrontPositionX + MapSize / 2) / MapSize);//진행상황막대 표시 수치
                }
            }
            else
            {
                _time += Time.deltaTime;
                ProgressBar.value = (_time / DefensTime);
            }
        }
    }
    public void GetUnitList(Collider2D Unit)
    {
        UnitList.Add(Unit);
        if(Mod == mode.MilitaryOccupation && Unit.transform.position.x > FrontPositionX)
        {
            FrontUnit = Unit;
            FrontPositionX = FrontUnit.transform.position.x;
        }
    }

    Collider2D NextFront()
    {
        Collider2D NextUnit = null;
        float NextFrontPosition = -9999;
        if(UnitList != null)
        {
            foreach (Collider2D Unit in UnitList)
            {
                if (Unit == null)
                {
                    UnitList.Remove(Unit);
                }
                if (NextFrontPosition < Unit.transform.position.x)
                {
                    NextFrontPosition = Unit.transform.position.x;
                    NextUnit = Unit;
                }
            }
        }
        return NextUnit;
    }
}