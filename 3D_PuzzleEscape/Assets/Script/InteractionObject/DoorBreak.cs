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
        
        /*
        DoorBreak script = null;

        //??연산자는 왼쪽값이 null일 경우 ??오른쪽 값을 처리한다. ?.연산자는 참조하는 값(왼쪽값)이 null이면 null로 처리. 아니라면 참조.
        
        script = transform.parent.Find("GameObject")?.Find("door_02")?.GetComponent<DoorBreak>();
        script ??= transform.parent.parent.Find("door_01")?.GetComponent<DoorBreak>();
        

        if (script)
        {
            script.SC_Event = null;
        }
        */
        InteractionEvent -= InteractiontoMove;
    }
}
