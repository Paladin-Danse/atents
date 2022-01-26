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
