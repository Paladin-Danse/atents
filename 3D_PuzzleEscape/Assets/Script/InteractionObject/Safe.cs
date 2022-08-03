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
        if(b_OnMiniGame && !b_OnDialMove)//�̴ϰ��� ���̰�, ���̾��� �����̰� ���� ���� ��, ���̾��� ������ ����.
        {
            dialMove();
        }
    }
    //���̾��� �������� �����ϴ� �Լ�.
    private void dialMove()
    {
        if(playerInput)
        {
            if (playerInput.Mini_LeftKey || playerInput.Mini_RightKey)
            {
                //�ݰ� ���̾��� ���������� �¿��� ���ذ��� �ݰ� �������� �¿� ���Ⱚ�� ���Ѵ�. �׷��� ������ ���̾��� �����̸鼭 �¿찪�� ���ݾ� Ʋ������ ����.
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
                        //���⼭���� �Ʒ� �ڵ�� ��ġ�� �ڵ尡 �ֱ⿡ ������ �� ���� �� ���� �ʿ�.
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

                Vector3 RotateScale = f_DialNum_Default_Rotation * Direction;//�ѹ��� ������ ũ��� ���Ⱚ�� ���ؼ�
                Quaternion MoveDistance = SafeDial.transform.rotation * Quaternion.Euler(RotateScale);//�ݰ� ���̾��� ������ ��ġ��ǥ���� ���Ѵ�.

                StartCoroutine("DialMoving", MoveDistance);//���̾��� ������.
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

    //���̾��� ��ȣ�� ��Ȯ�ϰ� �������� ȣ��Ǵ� �Լ�.
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
