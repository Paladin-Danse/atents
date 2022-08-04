using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//�̴ϰ��ӿ����� ���Ǵ� ���� UIPanel�� ���� ��.(������ �ִ� ����)

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
                int Num = playerInput.Mini_LeftKey ? -1 : playerInput.Mini_RightKey ? 1 : 0;//����Ű�� �����Ÿ� -1�� ������Ű�� �����Ÿ� +1���� �ش�. �̵����� �ƴ� ���ε� ���� Ȥ�� �� ��Ȳ�̸� 0�� �ش�.
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
                        On_PotionUI = UIManager.instance.MixPotionUIOnOffCheck("�������");
                    }
                    else if (playerInput.Mini_Num2Key)
                    {
                        liquid = RedPotion_Liquid_Mat;
                        On_PotionUI = UIManager.instance.MixPotionUIOnOffCheck("��������");
                    }
                    else if (playerInput.Mini_Num3Key)
                    {
                        liquid = BluePotion_Liquid_Mat;
                        On_PotionUI = UIManager.instance.MixPotionUIOnOffCheck("�Ķ�����");
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
                //�ű� ������ �� = (��� ���� �ö�ũ�� �� ����) > (���õ� �ö�ũ�� ������ ��) �� ��, ���� ���� �ű� ������ ���� ��.
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
                //Flasks[SelectFlask_Num].Liquid_Change(Potion_Fail_Mat, newAmount);//���� �ű� ������ Material�� Potion_Fail_Mat���� ������. ���߿� �����ǰ� �߰��Ǹ� �����ʿ�.
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
        InventoryManager.instance.LostItem("����");
        InventoryManager.instance.LostItem("�������");
        InventoryManager.instance.LostItem("��������");
        InventoryManager.instance.LostItem("�Ķ�����");
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
