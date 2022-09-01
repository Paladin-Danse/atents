using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WashItem : InteractionObject
{
    [SerializeField] private ItemData[] Mannequin_Data;//마네킹부품이 될 수 있는 모든 아이템 데이터값.
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
        //Resources에 없는 파일들이라 다른 함수를 쓰거나 리소스 폴더에 넣고 다시 경로 변경 해줘야함.(방법을 찾으면 수정 필요)
        //그전까진 직접 아이템 데이터들을 컴포넌트에 끌어다 넣는 식으로 운영.
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

    //마네킹의 부품인지 체크하고 마네킹의 부품이 맞다면 어디 부위에 속하는지 확인하는 함수.
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
                //마네킹 부품이 맞다면 어디 부품인지 다시 체크
                //여기서 사용한 when은 케이스가드(추가 조건문으로 기본 패턴을 포함해 해당 조건"까지" 충족해야 하는 추가 조건. bool식만 가능)
                data = name switch
                {
                    string key when key.Contains("인형머리") => Default_Head_data,
                    string key when key.Contains("왼쪽 팔") => Default_ArmL_data,
                    string key when key.Contains("오른쪽 팔") => Default_ArmR_data,
                    string key when key.Contains("왼쪽 다리") => Default_LegL_data,
                    string key when key.Contains("오른쪽 다리") => Default_LegR_data,
                    _ => null
                };
                /*
                switch(name)
                {
                    case string key when name.Contains("인형머리"):
                        data = Default_Head_data;
                        break;
                    case string key when name.Contains("왼쪽 팔"):
                        data = Default_ArmL_data;
                        break;
                    case string key when name.Contains("오른쪽 팔"):
                        data = Default_ArmR_data;
                        break;
                    case string key when name.Contains("왼쪽 다리"):
                        data = Default_LegL_data;
                        break;
                    case string key when name.Contains("오른쪽 다리"):
                        data = Default_LegR_data;
                        break;
                    default:
                        data = null;
                        break;
                }
                */
                /*
                if (name.Contains("인형머리"))
                {
                    data = Default_Head_data;
                }
                else if (name.Contains("왼쪽 팔"))
                {
                    data = Default_ArmL_data;
                }
                else if (name.Contains("오른쪽 팔"))
                {
                    data = Default_ArmR_data;
                }
                else if (name.Contains("왼쪽 다리"))
                {
                    data = Default_LegL_data;
                }
                else if (name.Contains("오른쪽 다리"))
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
                    Debug.Log("Error(WashItem) : Not Found data!\n해당 스크립트 컴포넌트에 데이터가 잘 들어가있는지 확인하십시오.");
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
