using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Safe : MiniGameInteraction
{
    [SerializeField] private GameObject SafeDial;
    [SerializeField] private Animation SafeAnim;
    [SerializeField] private Animation SafeLatchAnim;
    [SerializeField] private int[] DialNum;
    
    private int[] DialCurNum;
    private float f_DialNum_Default_Rotation = 30f;
    private Quaternion Dial_Default_Rotation;
    private bool b_OnDialMove;
    private int Dial_Iter;
    private Vector3 Direction = Vector3.zero;
    private int Dial_MoveCnt = 0;

    [SerializeField] protected float f_MoveSpeed;
    
    private new void Start()
    {
        base.Start();
        b_OnDialMove = false;
        DialCurNum = new int[DialNum.Length];
        Dial_Iter = 0;
        Dial_Default_Rotation = SafeDial.transform.rotation;

        MiniGameCancel += DialWrong;
    }
    private new void Update()
    {
        base.Update();
        if(b_OnMiniGame && !b_OnDialMove)//미니게임 중이고, 다이얼이 움직이고 있지 않을 때, 다이얼의 조작이 가능.
        {
            dialMove();
        }
    }
    //다이얼의 움직임을 구현하는 함수.
    private void dialMove()
    {
        if(playerInput)
        {
            if (playerInput.Mini_LeftKey || playerInput.Mini_RightKey)
            {
                //금고 다이얼을 움직이지만 좌우의 기준값은 금고를 기준으로 좌우 방향값을 정한다. 그렇지 않으면 다이얼이 움직이면서 좌우값이 조금씩 틀어지기 때문.
                if (Direction == Vector3.zero)
                {
                    if (playerInput.Mini_RightKey)
                        Direction = transform.right;
                    else
                        Direction = -transform.right;
                }
                else if (playerInput.Mini_RightKey)
                {
                    if (Direction != transform.right)
                    {
                        Direction = transform.right;
                        //여기서부터 아래 코드랑 겹치는 코드가 있기에 압축할 수 있을 때 압축 필요.
                        DialCurNum[Dial_Iter++] = Dial_MoveCnt;
                        Dial_MoveCnt = 0;
                    }
                }
                else if (playerInput.Mini_LeftKey)
                {
                    if (Direction != -transform.right)
                    {
                        Direction = -transform.right;
                        DialCurNum[Dial_Iter++] = Dial_MoveCnt;
                        Dial_MoveCnt = 0;
                    }
                }

                if (Dial_Iter >= DialNum.Length)
                {
                    DialWrong();
                    return;
                }

                Dial_MoveCnt++;

                /*
                else if (playerInput.Mini_LeftKey)
                {
                    Direction = -transform.right;
                    if (Dial_RightMoveCnt != 0)
                    {
                        DialCurNum[Dial_Iter] = Dial_RightMoveCnt;
                        Dial_RightMoveCnt = 0;
                    }
                    Dial_MoveCnt++;
                }
                Dial_Iter++;
                if (Dial_Iter >= DialNum.Length)
                {
                    DialWrong();
                    return;
                }
                DialCurNum[Dial_Iter] = Dial_MoveCnt;
                Dial_MoveCnt = 0;
                */

                Vector3 RotateScale = f_DialNum_Default_Rotation * Direction;//한번에 움직일 크기와 방향값을 구해서
                Quaternion MoveDistance = SafeDial.transform.rotation * Quaternion.Euler(RotateScale);//금고 다이얼이 도달할 위치목표값을 구한다.

                StartCoroutine("DialMoving", MoveDistance);//다이얼을 움직임.
            }
            if(playerInput.InteractionKey)
            {
                DialCurNum[Dial_Iter] = Dial_MoveCnt;

                for(int i=0; i<DialNum.Length; i++)
                {
                    if(DialNum[i] != DialCurNum[i])
                    {
                        DialWrong();
                        return;
                    }
                }
                DialGuess();
            }
        }
        else
        {
            Debug.Log("playerInput is Not Found!!");
            DialWrong();
            DefaultCancel();
        }
    }
    private IEnumerator DialMoving(Quaternion MoveDistance)
    {
        b_OnDialMove = true;
        float t = 0;

        while (1 >= t)
        {
            t += Time.deltaTime * f_MoveSpeed;
            SafeDial.transform.rotation = Quaternion.Lerp(SafeDial.transform.rotation, MoveDistance, t);

            yield return null;
        }
        b_OnDialMove = false;
    }

    //다이얼이 암호를 정확하게 맞췄을때 호출되는 함수.
    private void DialGuess()
    {
        if (SafeAnim)
        {
            SafeAnim.Play();
        }

        MiniGameClear();
        GameManager.instance.MiniGameClear(this.name);
    }
    private void DialWrong()
    {
        if(SafeLatchAnim)
        {
            SafeLatchAnim.Play();
        }

        Dial_Iter = 0;
        Direction = Vector3.zero;
        Dial_MoveCnt = 0;
        DialCurNum = new int[DialNum.Length];
        StartCoroutine("DialMoving", Dial_Default_Rotation);
    }
}
