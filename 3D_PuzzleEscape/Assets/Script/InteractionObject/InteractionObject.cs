using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum OBJ_TYPE
{
    OBJ_NONE = 0,
    OBJ_ITEM,
    OBJ_MINIGAME,
    OBJ_INTERACT
}

public class InteractionObject : MonoBehaviour
{
    public event Action InteractionEvent;
    public OBJ_TYPE e_ObjectType { get; protected set; }

    public void Interaction()
    {
        if (InteractionEvent != null) InteractionEvent();
    }
}
