using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Enemy_Spawn : MonoBehaviour {
    public GameObject Enemy;
    GameManager GM;
    public float Spawn_Time;
    public float R_Spawn_Y;

    bool Cool_On = true;

    Vector3 RandPosition;

    // Use this for initialization
    void Start () {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Cool_On && StartCool())
        {
            if (GM.GetGameState() == "Play")
            {
                StartCoroutine("enemy_Spawn");
            }
        }
    }

    IEnumerator enemy_Spawn()
    {
        float RandomY = Random.Range(-R_Spawn_Y, R_Spawn_Y);
        RandPosition = new Vector3(transform.position.x, transform.position.y + RandomY, transform.position.z + RandomY);
        GameObject enemy = Instantiate(Enemy, RandPosition, Quaternion.identity) as GameObject;//적 유닛 소환

        //ProgressTo_By_enemy(enemy);//소환한 적유닛을 진행상황 스크립트로 보냄
        Cool_On = false;//현재 유닛쿨타임이 돌고 있음

        yield return new WaitForSeconds(Spawn_Time);//쿨타임 도는중...
        Cool_On = true;//쿨타임이 끝남
    }
    /*
    public void ProgressTo_By_enemy(GameObject enemy)
    {
        ProgressTo progress;
        progress = GameObject.Find("GameManager").GetComponent<ProgressTo>();
        progress.GetUnitList(enemy);
    }
    */
    bool StartCool()
    {
        return Time.time >= 5 ? true : false;
    }

}
