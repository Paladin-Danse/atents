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
        DoorBreak script = transform.Find("GameObject").Find("door_02").GetComponent<DoorBreak>();
        script ??= transform.parent.Find("door_01").GetComponent<DoorBreak>();

        script.InteractionEvent -= SC_Event.StageClear;
        InteractionEvent -= InteractiontoMove;
    }
}
