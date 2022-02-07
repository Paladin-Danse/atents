using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Safe : MiniGameInteraction
{
    [SerializeField] protected GameObject SafeDial;
    [SerializeField] protected int[] DialNum;
    private float f_DialNum_Default_Rotation = 30f;
    private bool b_OnDialMove;

    protected PlayerInput playerInput;
    [SerializeField] protected float f_MoveSpeed;
    private void Awake()
    {
        if(GameManager.instance.playerInput) playerInput = GameManager.instance.playerInput;
        b_OnDialMove = false;
    }
    private void Update()
    {
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
                Vector3 Direction = Vector3.zero;//기본값이 없으면 에러나서 제로값을 줌.

                //금고 다이얼을 움직이지만 좌우의 기준값은 금고를 기준으로 좌우 방향값을 정한다. 그렇지 않으면 다이얼이 움직이면서 좌우값이 조금씩 틀어지기 때문.
                if (playerInput.Mini_RightKey) Direction = transform.right;
                else if (playerInput.Mini_LeftKey) Direction = -transform.right;

                Vector3 RotateScale = f_DialNum_Default_Rotation * Direction;//한번에 움직일 크기와 방향값을 구해서
                Quaternion MoveDistance = SafeDial.transform.rotation * Quaternion.Euler(RotateScale);//금고 다이얼이 도달할 위치목표값을 구한다.

                StartCoroutine("DialMoving", MoveDistance);//다이얼을 움직임.
            }
        }
        else
        {
            MiniGameCancel();
        }
    }
    private IEnumerator DialMoving(Quaternion MoveDistance)
    {
        b_OnDialMove = true;
        float t = 0;

        while (1 >= t)
        {
            t += Time.deltaTime;
            SafeDial.transform.rotation = Quaternion.Lerp(SafeDial.transform.rotation, MoveDistance, t);

            yield return null;
        }
        b_OnDialMove = false;
    }

    //다이얼이 암호를 정확하게 맞췄을때 호출되는 함수.
    private void DialGuess()
    {
        
    }
}
