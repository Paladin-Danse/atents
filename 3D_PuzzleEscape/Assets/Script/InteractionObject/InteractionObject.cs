using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum OBJ_TYPE
{
    OBJ_NONE = 0, //�ƹ��͵� �������� ���� �⺻������ ������Ʈ(���ӳ����� �̰� ����� ��ȣ�ۿ� ������Ʈ�� �������.)
    OBJ_ITEM, //��ȣ�ۿ��� �ϸ� �κ��丮�� ������ ������ ������Ʈ
    OBJ_MINIGAME, //��ȣ�ۿ��� �ϸ� �̴ϰ����� ����ǰ� �̴ϰ����� Ŭ������ ��� �̺�Ʈ�� �����ϴ� ������Ʈ
    OBJ_INTERACT //��ȣ�ۿ��� �ϸ� ���Ǿ��� ��ȣ�ۿ��� �Ǵ� ������Ʈ �ٸ�, �̺�Ʈ�� ��� �������� �ʿ�� �ϴ� ��찡 ����.
}

public class InteractionObject : MonoBehaviour
{
    [SerializeField] protected ItemData NeedItem;
    protected event Action InteractionEvent;
    protected StageClearEvent SC_Event;
    public OBJ_TYPE e_ObjectType { get; protected set; }

    protected void Start()
    {
        SC_Event = GetComponent<StageClearEvent>();
        if (gameObject.layer != LayerMask.NameToLayer("Interactable")) gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

    public void Interaction()
    {
        if (InteractionEvent != null)
        {
            if (SC_Event) InteractionEvent += SC_Event.StageClear;
            InteractionEvent();
        }
    }
    public ItemData NeedItemCheck()
    {
        return NeedItem;
    }
}
