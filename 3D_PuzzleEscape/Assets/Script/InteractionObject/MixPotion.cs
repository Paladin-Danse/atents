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
    [SerializeField] private Material GreenPotion_Liquid_Mat;
    [SerializeField] private ItemData RedPotion;
    [SerializeField] private Material RedPotion_Liquid_Mat;
    [SerializeField] private ItemData BluePotion;
    [SerializeField] private Material BluePotion_Liquid_Mat;
    [SerializeField] private Material Potion_Fail_Mat;
    [SerializeField] private Material Potion_Success_Mat;

    private Dictionary<Material, float> PotionRecipe = new Dictionary<Material, float>();
    [SerializeField] private Material[] PotionRecipe_liquid;
    [SerializeField] private float[] PotionRecipe_Amount;

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

        for (int i = 0; i < PotionRecipe_liquid.Length; i++)
        {
            PotionRecipe.Add(PotionRecipe_liquid[i], PotionRecipe_Amount[i]);
        }
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
            if (Flasks[SelectFlask_Num].b_OnGettingLiquid == false)
            {
                if (playerInput.MixKey)
                {
                    SelectMixFlask();
                }
                if (playerInput.ItemDescriptionKey)
                {
                    DestroyPotion();
                }

                if (playerInput.Mini_Num1Key || playerInput.Mini_Num2Key || playerInput.Mini_Num3Key)
                {
                    Material liquid = null;
                    bool On_PotionUI = false;
                    if (playerInput.Mini_Num1Key)
                    {
                        liquid = GreenPotion_Liquid_Mat;
                        On_PotionUI = UIManager.instance.MixPotionUIOnOffCheck("녹색물약");
                    }
                    else if (playerInput.Mini_Num2Key)
                    {
                        liquid = RedPotion_Liquid_Mat;
                        On_PotionUI = UIManager.instance.MixPotionUIOnOffCheck("빨간물약");
                    }
                    else if (playerInput.Mini_Num3Key)
                    {
                        liquid = BluePotion_Liquid_Mat;
                        On_PotionUI = UIManager.instance.MixPotionUIOnOffCheck("파란물약");
                    }
                    if (liquid != null && On_PotionUI) Put_the_Potion(liquid);
                }
            }
        }
    }

    private new void MiniGameStart()
    {
        base.MiniGameStart();

        UIManager.instance.On_MiniUI();
        Active_halo(halo);

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
        if(SelectedMixFlask != null && SelectedMixFlask != Flasks[SelectFlask_Num])
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

    private void Put_the_Potion(Material liquid)
    {
        var flask = Flasks[SelectFlask_Num];
        float emptySize = flask.EmptyAmount();

        Dictionary<Material, float> plusLiquid = new Dictionary<Material, float>();
        plusLiquid.Add(liquid, emptySize);
        flask.Liquid_FillUp(emptySize, plusLiquid);

        if (flask.GetSize() == emptySize || flask.GetLiquid() == liquid)
            flask.Liquid_Change(liquid);
        else
            flask.Liquid_Change(Get_MixCheck(flask.Get_Potion_Mixture()));
    }
    private void DestroyPotion()
    {
        var flask = Flasks[SelectFlask_Num];
        flask.Liquid_Drain(flask.GetSize() - flask.EmptyAmount());
    }
    private void Mix()
    {
        var flask = Flasks[SelectFlask_Num];

        if (SelectedMixFlask != null && flask.EmptyAmount() != 0)
        {
            if(SelectedMixFlask.EmptyAmount() != SelectedMixFlask.GetSize())
            {
                //옮길 물약의 량 = (담고 싶은 플라스크의 빈 공간) > (선택된 플라스크의 물약의 량) 일 때, 작은 쪽이 옮길 물약의 량이 됨.
                float newAmount = flask.EmptyAmount() > (SelectedMixFlask.GetSize() - SelectedMixFlask.EmptyAmount()) ?
                    (SelectedMixFlask.GetSize() - SelectedMixFlask.EmptyAmount()) : flask.EmptyAmount();
                float flaskEptAmount = flask.EmptyAmount();

                var DrainLiquid = SelectedMixFlask.Liquid_Drain(newAmount);
                flask.Liquid_FillUp(newAmount, DrainLiquid);

                if (flask.GetSize() == flaskEptAmount || flask.GetLiquid() == SelectedMixFlask.GetLiquid())
                {
                    flask.Liquid_Change(SelectedMixFlask.GetLiquid());
                }
                else
                {
                    flask.Liquid_Change(Get_MixCheck(flask.Get_Potion_Mixture()));
                }
                //Flasks[SelectFlask_Num].Liquid_Change(Potion_Fail_Mat, newAmount);//현재 옮길 물약의 Material은 Potion_Fail_Mat으로 고정됨. 나중에 레시피가 추가되면 수정필요.
            }
        }
    }

    private Material Get_MixCheck(Dictionary<Material, float> mixture)
    {
        if (mixture.Count <= 0) return Potion_Fail_Mat;
        var tempRecipe = new Dictionary<Material, float>(PotionRecipe);

        foreach (Material Key in mixture.Keys)
        {
            if(!tempRecipe.ContainsKey(Key))
            {
                return Potion_Fail_Mat;
            }

            if (Mathf.Abs(tempRecipe[Key] - mixture[Key]) >= 0.01f)
            {
                return Potion_Fail_Mat;
            }
            tempRecipe.Remove(Key);
        }

        if (tempRecipe.Count <= 0)
        {
            MixSuccess();
            return Potion_Success_Mat;
        }
        else
        {
            return Potion_Fail_Mat;
        }
    }
    private void MixSuccess()
    {
        MixPotion_Cancel();
        MiniGameClear();
        GameManager.instance.MiniGameClear("MixPotion");
        Flasks[SelectFlask_Num].GetComponentInChildren<ItemInteraction>().enabled = true;
        InventoryManager.instance.LostItem("피펫");
        InventoryManager.instance.LostItem("녹색물약");
        InventoryManager.instance.LostItem("빨간물약");
        InventoryManager.instance.LostItem("파란물약");
    }

    private void Active_halo(GameObject halo)
    {
        halo.transform.SetParent(Flasks[SelectFlask_Num].gameObject.transform);
        halo.transform.SetAsFirstSibling();
        halo.transform.localPosition = new Vector3(0.1f, 0.25f, 0.0f);
        halo.transform.localRotation = Quaternion.identity;
        halo.SetActive(true);
    }

    private void MixPotion_Cancel()
    {
        halo.SetActive(false);
        mixhalo.SetActive(false);
        UIManager.instance.Off_MiniUI();
    }
}
