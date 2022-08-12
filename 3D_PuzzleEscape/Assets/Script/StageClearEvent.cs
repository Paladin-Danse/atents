using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageClearEvent : MonoBehaviour
{
    [SerializeField] private STAGE ClearStage;
    
    public void StageClear()
    {
        GameManager.instance.StageClear(ClearStage);
    }
}