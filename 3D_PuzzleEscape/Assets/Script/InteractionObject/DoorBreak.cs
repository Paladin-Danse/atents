using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBreak : MoveInteraction
{
    protected new void Start()
    {
        base.Start();
        InteractionEvent += DoorBroken;
    }

    protected void DoorBroken()
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
        InteractionEvent -= InteractiontoMove;
    }
}
