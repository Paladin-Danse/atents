using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//미니게임에서만 사용되는 전용 UIPanel이 있을 것.(가지고 있는 포션)

public class MixPotion : MiniGameInteraction
{
    [SerializeField] private List<Mini_Flask> Flasks;
    private Mini_Flask SelectedMixFlask;
    private int SelectFlask_Num;
    [SerializeField] private GameObject SelectHalo;
    [SerializeField] private GameObject SelectMixHalo;
    private GameObject halo;
    private GameObject mixhalo;

    [SerializeField] private ItemData GreenPotion;
    [SerializeField] private Material GreenPotion_Requid_Mat;
    [SerializeField] private ItemData RedPotion;
    [SerializeField] private Material RedPotion_Requid_Mat;
    [SerializeField] private ItemData BluePotion;
    [SerializeField] private Material BluePotion_Requid_Mat;

    [SerializeField] private Dictionary<InventoryItem, int>[] PotionRecipe;

    private new void Start()
    {
        base.Start();

        SelectFlask_Num = 0;
        SelectedMixFlask = null;
        halo = Instantiate(SelectHalo, transform);
        halo.SetActive(false);
        mixhalo = Instantiate(SelectMixHalo, transform);
        mixhalo.SetActive(false);

        MiniGameCancel += MixPotion_Cancel;
        InteractionEvent += MiniGameStart;
    }

    private new void Update()
    {
        base.Update();

        if (b_OnMiniGame)
        {
            if (playerInput.Mini_LeftKey || playerInput.Mini_RightKey)
            {
                int Num = playerInput.Mini_LeftKey ? -1 : playerInput.Mini_RightKey ? 1 : 0;//왼쪽키를 누른거면 -1을 오른쪽키를 누른거면 +1값을 준다. 이도저도 아닌 값인데 들어온 혹시 모를 상황이면 0을 준다.
                SelectFlask_Num = (Mathf.Clamp(SelectFlask_Num + Num, 0, Flasks.Count - 1));
                SelectFlask();
            }

            if (playerInput.MixKey)
            {
                SelectMixFlask();
            }
        }
    }

    private new void MiniGameStart()
    {
        base.MiniGameStart();

        UIManager.instance.On_MiniUI();
        
        PotionUICheck(InventoryManager.instance.InventoryitemCheck(GreenPotion));
        PotionUICheck(InventoryManager.instance.InventoryitemCheck(RedPotion));
        PotionUICheck(InventoryManager.instance.InventoryitemCheck(BluePotion));
    }

    private void PotionUICheck(InventoryItem item)
    {
        if (item != null)
        {
            UIManager.instance.MixPotionUICheck(item.data.name);
        }
    }

    private void SelectFlask()
    {
        Active_halo(halo);
    }
    private void SelectMixFlask()
    {
        if(SelectedMixFlask != null)
        {
            mixhalo.SetActive(false);
            Mix();
            SelectedMixFlask = null;
        }
        else
        {
            SelectedMixFlask = Flasks[SelectFlask_Num];

            Active_halo(mixhalo);
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
        UIManager.instance.Off_MiniUI();
    }
    private void Active_halo(GameObject halo)
    {
        halo.transform.SetParent(Flasks[SelectFlask_Num].gameObject.transform);
        halo.transform.SetAsFirstSibling();
        halo.transform.localPosition = new Vector3(0.0f, 0.25f, 0.0f);
        halo.transform.localRotation = Quaternion.identity;

        halo.SetActive(true);
    }

    private void MixPotion_Cancel()
    {
        halo.SetActive(false);
        mixhalo.SetActive(false);
        Mini_OffUI();
    }
}
