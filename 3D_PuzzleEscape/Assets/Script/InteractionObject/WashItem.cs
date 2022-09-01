using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WashItem : InteractionObject
{
    [SerializeField] private ItemData[] Mannequin_Data;//����ŷ��ǰ�� �� �� �ִ� ��� ������ �����Ͱ�.
    [SerializeField] private ItemData Default_Head_data;
    [SerializeField] private ItemData Default_ArmL_data;
    [SerializeField] private ItemData Default_ArmR_data;
    [SerializeField] private ItemData Default_LegL_data;
    [SerializeField] private ItemData Default_LegR_data;
    [SerializeField] private AudioClip Wash_Sound;
    private AudioSource Interaction_Sound;
    private IEnumerator Sound_Court;

    // Start is called before the first frame update
    private new void Start()
    {
        base.Start();
        InteractionEvent += Mannequin_PartCheck;
        //Resources�� ���� ���ϵ��̶� �ٸ� �Լ��� ���ų� ���ҽ� ������ �ְ� �ٽ� ��� ���� �������.(����� ã���� ���� �ʿ�)
        //�������� ���� ������ �����͵��� ������Ʈ�� ����� �ִ� ������ �.
        if (!Default_Head_data)
            Default_Head_data = Resources.Load<ItemData>("Assets/Script/ScriptableData/Item/Puppet_Head.asset");
        if (!Default_ArmL_data)
            Default_ArmL_data = Resources.Load<ItemData>("Assets/Script/ScriptableData/Item/Puppet_left_arm.asset");
        if (!Default_ArmR_data)
            Default_ArmR_data = Resources.Load<ItemData>("Assets/Script/ScriptableData/Item/Puppet_right_arm.asset");
        if (!Default_LegL_data)
            Default_LegL_data = Resources.Load<ItemData>("Assets/Script/ScriptableData/Item/Puppet_left_leg.asset");
        if (!Default_LegR_data)
            Default_LegR_data = Resources.Load<ItemData>("Assets/Script/ScriptableData/Item/Puppet_right_leg.asset");

        Interaction_Sound = GetComponent<AudioSource>();
        if(!Interaction_Sound)
        {
#if UNITY_EDITOR
            Debug.Log("Error(WashItem) : Interaction_Sound is Not Found!");
#endif
        }
    }

    //����ŷ�� ��ǰ���� üũ�ϰ� ����ŷ�� ��ǰ�� �´ٸ� ��� ������ ���ϴ��� Ȯ���ϴ� �Լ�.
    public void Mannequin_PartCheck()
    {
        InventoryItem item = InventoryManager.instance.SelectedItem;
        if (item == null) return;
        string name = item.data.name;
        ItemData data;

        foreach (ItemData i in Mannequin_Data)
        {
            if (name == i.Data.name)
            {
                //����ŷ ��ǰ�� �´ٸ� ��� ��ǰ���� �ٽ� üũ
                //���⼭ ����� when�� ���̽�����(�߰� ���ǹ����� �⺻ ������ ������ �ش� ����"����" �����ؾ� �ϴ� �߰� ����. bool�ĸ� ����)
                data = name switch
                {
                    string key when key.Contains("�����Ӹ�") => Default_Head_data,
                    string key when key.Contains("���� ��") => Default_ArmL_data,
                    string key when key.Contains("������ ��") => Default_ArmR_data,
                    string key when key.Contains("���� �ٸ�") => Default_LegL_data,
                    string key when key.Contains("������ �ٸ�") => Default_LegR_data,
                    _ => null
                };
                /*
                switch(name)
                {
                    case string key when name.Contains("�����Ӹ�"):
                        data = Default_Head_data;
                        break;
                    case string key when name.Contains("���� ��"):
                        data = Default_ArmL_data;
                        break;
                    case string key when name.Contains("������ ��"):
                        data = Default_ArmR_data;
                        break;
                    case string key when name.Contains("���� �ٸ�"):
                        data = Default_LegL_data;
                        break;
                    case string key when name.Contains("������ �ٸ�"):
                        data = Default_LegR_data;
                        break;
                    default:
                        data = null;
                        break;
                }
                */
                /*
                if (name.Contains("�����Ӹ�"))
                {
                    data = Default_Head_data;
                }
                else if (name.Contains("���� ��"))
                {
                    data = Default_ArmL_data;
                }
                else if (name.Contains("������ ��"))
                {
                    data = Default_ArmR_data;
                }
                else if (name.Contains("���� �ٸ�"))
                {
                    data = Default_LegL_data;
                }
                else if (name.Contains("������ �ٸ�"))
                {
                    data = Default_LegR_data;
                }
                else
                {
                    data = null;
                }
                */
                if (data)
                {
                    if (Interaction_Sound.isPlaying)
                    {
                        StopCoroutine(Sound_Court);
                        Sound_Court = InteractionSound_Play();
                        StartCoroutine(Sound_Court);
                    }
                    else
                    {
                        Sound_Court = InteractionSound_Play();
                        StartCoroutine(Sound_Court);
                    }
                    InventoryManager.instance.GetItem(data.Data.name);
                    InventoryManager.instance.UseItem(item);
                }
                else
                {
                    Debug.Log("Error(WashItem) : Not Found data!\n�ش� ��ũ��Ʈ ������Ʈ�� �����Ͱ� �� ���ִ��� Ȯ���Ͻʽÿ�.");
                }
                break;
            }
        }
    }

    IEnumerator InteractionSound_Play()
    {
        if (Interaction_Sound.clip != Wash_Sound) Interaction_Sound.clip = Wash_Sound;
        Interaction_Sound.Play();

        yield return new WaitForSeconds(3.0f);

        Interaction_Sound.Stop();
    }
}
