using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Safe : MiniGameInteraction
{
    [SerializeField] protected GameObject SafeDial;
    [SerializeField] protected int[] DialNum;
    private float f_DialNum_Default_Rotation = 30f;

    protected PlayerInput playerInput;
    [SerializeField] protected float f_MoveSpeed;
    private void Awake()
    {
        if(GameManager.instance.playerInput) playerInput = GameManager.instance.playerInput;
    }
    private void Update()
    {
        if(b_OnMiniGame)
        {
            dialMove();
        }
    }
    private void dialMove()
    {
        if(playerInput)
        {
            if (playerInput.HorizontalMoveKey != 0)
            {
                Vector3 MoveDistance = playerInput.HorizontalMoveKey * SafeDial.transform.right;
                SafeDial.transform.rotation *= Quaternion.Euler(MoveDistance * Time.deltaTime * f_MoveSpeed);
            }
        }
    }
    private void DialGuess()
    {
        
    }
}
