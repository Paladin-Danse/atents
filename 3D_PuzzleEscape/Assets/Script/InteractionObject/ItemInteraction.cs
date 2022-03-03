using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteraction : InteractionObject
{
    [SerializeField] private ItemData item;
    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();
        e_ObjectType = OBJ_TYPE.OBJ_ITEM;
        InteractionEvent += Interaction_to_GetItem;
    }

    private void Interaction_to_GetItem()
    {
        InventoryManager.instance.GetItem(item);

        gameObject.SetActive(false);
    }
}
