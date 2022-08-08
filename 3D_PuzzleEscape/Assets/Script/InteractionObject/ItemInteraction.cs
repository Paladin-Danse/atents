using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MAPITEM_TYPE
{
    MAPITEM_KEY,
    MAPITEM_NORMAL,
    NONE
}

public class ItemInteraction : InteractionObject
{
    [SerializeField] protected ItemData item;
    [SerializeField] protected MINIGAME_SIRIES myGame;
    [SerializeField] protected MAPITEM_TYPE itemType;
    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();
        LoadSetEnable();
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

    protected void LoadSetEnable()
    {
        SaveData savedata = GameManager.instance.mySavedata;

        if (itemType == MAPITEM_TYPE.MAPITEM_NORMAL)
        {
            switch (myGame)
            {
                case MINIGAME_SIRIES.MINIGAME_SAFE:
                    if (savedata.Mini_1_Clear)
                        gameObject.SetActive(false);
                    break;
                case MINIGAME_SIRIES.MINIGAME_MIX_POTION:
                    if (savedata.Mini_2_Clear)
                        gameObject.SetActive(false);
                    break;
                case MINIGAME_SIRIES.MINIGAME_TWIN_PUPPET:
                    if (savedata.Mini_3_Clear)
                        gameObject.SetActive(false);
                    break;
                default:
                    break;
            }
        }
        if(itemType == MAPITEM_TYPE.MAPITEM_KEY)
        {
            switch(savedata.Stage)
            {
                case STAGE.STAGE_1:
                    if (savedata.Mini_1_Clear)
                        gameObject.SetActive(true);
                    break;
                case STAGE.STAGE_2:
                    if (savedata.Mini_2_Clear)
                        gameObject.SetActive(true);
                    break;
                case STAGE.STAGE_3:
                    if (savedata.Mini_3_Clear)
                        gameObject.SetActive(true);
                    break;
            }
        }
    }
}
