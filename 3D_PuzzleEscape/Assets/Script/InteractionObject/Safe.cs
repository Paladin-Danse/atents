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
    private int Dial_LeftMoveCnt = 0;
    private int Dial_RightMoveCnt = 0;

    protected PlayerInput playerInput;
    [SerializeField] protected float f_MoveSpeed;
    private void Awake()
    {
        if(GameManager.instance.playerInput) playerInput = GameManager.instance.playerInput;
        b_OnDialMove = false;
        DialCurNum = new int[DialNum.Length];
        Dial_Iter = 0;
    }
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
                Vector3 Direction = Vector3.zero;//�⺻���� ������ �������� ���ΰ��� ��.

                //�ݰ� ���̾��� ���������� �¿��� ���ذ��� �ݰ� �������� �¿� ���Ⱚ�� ���Ѵ�. �׷��� ������ ���̾��� �����̸鼭 �¿찪�� ���ݾ� Ʋ������ ����.
                if (playerInput.Mini_RightKey)
                {
                    Direction = transform.right;
                    if (Dial_LeftMoveCnt != 0)
                    {
                        DialCurNum[Dial_Iter++] = Dial_LeftMoveCnt;
                        Dial_LeftMoveCnt = 0;
                        if (Dial_Iter >= DialNum.Length)
                        {
                            DialWrong();
                            return;
                        }
                    }
                    Dial_RightMoveCnt++;
                }
                else if (playerInput.Mini_LeftKey)
                {
                    Direction = -transform.right;
                    if (Dial_RightMoveCnt != 0)
                    {
                        DialCurNum[Dial_Iter++] = Dial_RightMoveCnt;
                        Dial_RightMoveCnt = 0;
                        if (Dial_Iter >= DialNum.Length)
                        {
                            DialWrong();
                            return;
                        }
                    }
                    Dial_LeftMoveCnt++;
                }

                Vector3 RotateScale = f_DialNum_Default_Rotation * Direction;//�ѹ��� ������ ũ��� ���Ⱚ�� ���ؼ�
                Quaternion MoveDistance = SafeDial.transform.rotation * Quaternion.Euler(RotateScale);//�ݰ� ���̾��� ������ ��ġ��ǥ���� ���Ѵ�.

                StartCoroutine("DialMoving", MoveDistance);//���̾��� ������.
            }
            if(playerInput.InteractionKey)
            {
                DialCurNum[Dial_Iter] = Dial_LeftMoveCnt > Dial_RightMoveCnt ? Dial_LeftMoveCnt : Dial_RightMoveCnt;

                for(int i=0; i<DialNum.Length; i++)
                {
                    if(DialNum[i] != DialCurNum[i])
                    {
                        DialWrong();
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
    }
    private void DialWrong()
    {
        if(SafeLatchAnim)
        {
            SafeLatchAnim.Play();
        }

        Dial_Iter = 0;
        Dial_LeftMoveCnt = 0;
        Dial_RightMoveCnt = 0;
        StartCoroutine("DialMoving", Dial_Default_Rotation);
    }
}
