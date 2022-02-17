using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//미니게임에서만 사용되는 전용 UIPanel이 있을 것.(가지고 있는 포션)

public class MixPotion : MiniGameInteraction
{
    [SerializeField] private List<Mini_Flask> Flasks;
    private Mini_Flask SelectedMixFlask;
    private int SelectFlask_Num;

    private ItemData GreenPotion;
    [SerializeField] private Material GreenPotion_Requid_Mat;
    private ItemData RedPotion;
    [SerializeField] private Material RedPotion_Requid_Mat;
    private ItemData BluePotion;
    [SerializeField] private Material BluePotion_Requid_Mat;

    [SerializeField] private Dictionary<InventoryItem, int> PotionRecipe;

    private new void Start()
    {
        base.Start();

        SelectFlask_Num = 0;
        SelectedMixFlask = null;
    }

    private new void Update()
    {
        base.Update();
        int Num = 0;

        if(playerInput.Mini_LeftKey)
        {
            Num--;
        }
        if(playerInput.Mini_RightKey)
        {
            Num++;
        }
        
        if(playerInput.Mini_LeftKey || playerInput.Mini_RightKey)
        {
            SelectFlask_Num = (Mathf.Clamp(SelectFlask_Num + Num, 0, Flasks.Count - 1));
            SelectFlask();
        }

        if(playerInput.MixKey)
        {
            SelectMixFlask();
        }
    }

    private new void MiniGameStart()
    {
        base.MiniGameStart();

        if (InventoryManager.instance.InventoryitemCheck(GreenPotion))
        {

        }
        if (InventoryManager.instance.InventoryitemCheck(RedPotion))
        {

        }
        if (InventoryManager.instance.InventoryitemCheck(BluePotion))
        {

        }
    }

    private void SelectFlask()
    {
        
    }
    private void SelectMixFlask()
    {
        if(SelectedMixFlask != null)
        {

        }
        else
        {

        }
    }

    private void Put_the_Potion()
    {

    }
    private void Mix()
    {

    }

    private void MixSuccess()
    {

    }

    private void MixFail()
    {

    }

    private void Mini_OnUI()
    {

    }
    private void Mini_OffUI()
    {

    }
}
