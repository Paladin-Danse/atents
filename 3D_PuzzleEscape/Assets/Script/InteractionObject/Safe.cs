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
                if (playerInput.Mini_RightKey) Direction = transform.right;
                else if (playerInput.Mini_LeftKey) Direction = -transform.right;

                Vector3 RotateScale = f_DialNum_Default_Rotation * Direction;//�ѹ��� ������ ũ��� ���Ⱚ�� ���ؼ�
                Quaternion MoveDistance = SafeDial.transform.rotation * Quaternion.Euler(RotateScale);//�ݰ� ���̾��� ������ ��ġ��ǥ���� ���Ѵ�.

                StartCoroutine("DialMoving", MoveDistance);//���̾��� ������.
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

    //���̾��� ��ȣ�� ��Ȯ�ϰ� �������� ȣ��Ǵ� �Լ�.
    private void DialGuess()
    {
        
    }
}
