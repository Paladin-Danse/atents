using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteraction : InteractionObject
{
    [SerializeField] protected ItemData item;
    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();
        e_ObjectType = OBJ_TYPE.OBJ_ITEM;
        InteractionEvent += Interaction_to_GetItem;
    }

    private void Interaction_to_GetItem()
    {
        InventoryManager.instance.GetItem(item.Data.name);

        gameObject.SetActive(false);
    }

    public void SetItem(ItemData m_item)
    {
        item = m_item;
    }
}
