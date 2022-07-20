using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageClearEvent : InteractionObject
{
    InteractionObject inter_obj;
    private void Awake()
    {
        inter_obj = GetComponent<InteractionObject>();
        
        InteractionEvent += StageClear;
    }

    private void StageClear()
    {
        GameManager.instance.StageClear();
        InteractionEvent -= StageClear;
    }
}
