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

        //??�����ڴ� ���ʰ��� null�� ��� ??������ ���� ó���Ѵ�. ?.�����ڴ� �����ϴ� ��(���ʰ�)�� null�̸� null�� ó��. �ƴ϶�� ����.
        
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
