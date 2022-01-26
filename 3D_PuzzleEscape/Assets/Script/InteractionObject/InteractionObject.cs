using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum OBJ_TYPE
{
    OBJ_NONE = 0, //아무것도 지정되지 않은 기본상태의 오브젝트(게임내에선 이게 적용된 상호작용 오브젝트는 없어야함.)
    OBJ_ITEM, //상호작용을 하면 인벤토리에 들어오는 아이템 오브젝트
    OBJ_MINIGAME, //상호작용을 하면 미니게임이 실행되고 미니게임을 클리어할 경우 이벤트를 실행하는 오브젝트
    OBJ_INTERACT //상호작용을 하면 조건없이 상호작용이 되는 오브젝트 다만, 이벤트의 경우 아이템을 필요로 하는 경우가 있음.
}

public class InteractionObject : MonoBehaviour
{
    [SerializeField] protected ItemData NeedItem;
    protected event Action InteractionEvent;
    public OBJ_TYPE e_ObjectType { get; protected set; }

    public void Interaction()
    {
        if (InteractionEvent != null) InteractionEvent();
    }
    public ItemData NeedItemCheck()
    {
        return NeedItem;
    }
}
