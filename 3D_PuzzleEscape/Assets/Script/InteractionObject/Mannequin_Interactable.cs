using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mannequin_Interactable : InteractionObject
{
    [SerializeField] private GameObject Mannequin_Head;
    [SerializeField] private GameObject Mannequin_ArmL;
    [SerializeField] private GameObject Mannequin_ArmR;
    [SerializeField] private GameObject Mannequin_LegL;
    [SerializeField] private GameObject Mannequin_LegR;
    [SerializeField] private ItemData[] Mannequin_Data;//마네킹부품이 될 수 있는 모든 아이템 데이터값.
    [SerializeField] private Material Red_Mat;
    [SerializeField] private Material Blue_Mat;
    [SerializeField] private Material Green_Mat;
    [SerializeField] private Material Default_Mat;

    [SerializeField] private AudioClip Fit_Clip;
    [SerializeField] private AudioClip pullout_Clip;
    [SerializeField] private AudioClip GameClear_Clip;
    private AudioSource InteractSound;
    // Start is called before the first frame update
    private new void Start()
    {
        base.Start();
        InteractionEvent += Mannequin_PartCheck;
        InteractSound = GetComponent<AudioSource>();

        //세이브파일이 있고, 인형 색 맞추기를 클리어한 이후인 경우 게임매니저에게 예시 나무인형의 데이터를 받아와서 똑같은 색을 입혀준다. 그게 아니라면 마네킹의 SetActive를 꺼준다.
        if (GameManager.instance.mySavedata != null)
        {
            if (GameManager.instance.mySavedata.Mini_3_Clear)
            {
                List<ItemData> partsData = GameManager.instance.Ex_PartsData;
                ItemData HeadData = partsData.Find(i => i.Data.name.Contains("인형머리"));
                ItemData ArmLData = partsData.Find(i => i.Data.name.Contains("왼쪽 팔"));
                ItemData ArmRData = partsData.Find(i => i.Data.name.Contains("오른쪽 팔"));
                ItemData LegLData = partsData.Find(i => i.Data.name.Contains("왼쪽 다리"));
                ItemData LegRData = partsData.Find(i => i.Data.name.Contains("오른쪽 다리"));

                Mannequin_fit(Mannequin_Head, HeadData.Data, HeadData.Data.name);
                Mannequin_fit(Mannequin_ArmL, ArmLData.Data, ArmLData.Data.name);
                Mannequin_fit(Mannequin_ArmR, ArmRData.Data, ArmRData.Data.name);
                Mannequin_fit(Mannequin_LegL, LegLData.Data, LegLData.Data.name);
                Mannequin_fit(Mannequin_LegR, LegRData.Data, LegRData.Data.name);

                if (InteractSound.isPlaying) InteractSound.Stop(); //세이브파일을 불러오는 과정에서 마네킹을 끼워맞추는 소리를 꺼주는 코드
            }
            else
            {
                Mannequin_Active(false);
            }
        }
        else
        {
            Mannequin_Active(false);
        }
    }

    //마네킹을 장비하고 있는 아이템으로 상호작용할 때 마네킹의 부품인지 체크하고 마네킹의 부품이 맞다면 어디 부위에 속하는지 확인하는 함수.
    public void Mannequin_PartCheck()
    {
        InventoryItem item = InventoryManager.instance.SelectedItem;
        if (item == null) return;
        string name = item.data.name;

        foreach (ItemData i in Mannequin_Data)
        {
            if(name == i.Data.name)
            {
                GameObject Parts;

                //마네킹 부품이 맞다면 어디 부품인지 다시 체크
                switch (name)
                {
                    case string key when name.Contains("인형머리"):
                        Parts = Mannequin_Head;
                        break;
                    case string key when name.Contains("왼쪽 팔"):
                        Parts = Mannequin_ArmL;
                        break;
                    case string key when name.Contains("오른쪽 팔"):
                        Parts = Mannequin_ArmR;
                        break;
                    case string key when name.Contains("왼쪽 다리"):
                        Parts = Mannequin_LegL;
                        break;
                    case string key when name.Contains("오른쪽 다리"):
                        Parts = Mannequin_LegR;
                        break;
                    default:
                        Parts = null;
                        break;
                }

                if (Parts)
                {
                    Mannequin_fit(Parts, item.data, name);
                    InventoryManager.instance.UseItem(item);

                    if (Mannequin_Head.activeSelf &&
                        Mannequin_ArmL.activeSelf &&
                        Mannequin_ArmR.activeSelf &&
                        Mannequin_LegL.activeSelf &&
                        Mannequin_LegR.activeSelf)
                    {
                        if (GameManager.instance.M_Example_Compare_Data() && GameClear_Clip != null)
                        {
                            if (InteractSound.isPlaying) InteractSound.Stop();
                            InteractSound.clip = GameClear_Clip;
                            InteractSound.Play();
                        }
                    }
                }
                else
                {
                    Debug.Log("Error(Mannequin_Interactable) : Not Found Parts Object!");
                }
                break;
            }
        }
    }

    //마네킹 부품을 활성화하고 무슨 색을 입혔는지 확인해서 해당 Material을 입히는 함수.
    void Mannequin_fit(GameObject Part, Item data, string name)
    {
        Material mat;

        Part.SetActive(true);
        
        if (name.Contains("적"))
            mat = Red_Mat;
        else if(name.Contains("청"))
            mat = Blue_Mat;
        else if(name.Contains("녹"))
            mat = Green_Mat;
        else
            mat = Default_Mat;

        //만약 틀린 색을 끼워맞췄을 경우 부품을 다시 뽑을 수 있게 해당 부품을 주울 수 있는 아이템으로 생성한다.
        if (Part.GetComponent<Mannequin_GetParts>())
        {
            ItemData itemData = new ItemData();
            itemData.InputData(data);
            Part.GetComponent<Mannequin_GetParts>().SetItem(itemData);
            Part.GetComponent<Mannequin_GetParts>().SetClip(pullout_Clip);

            //GameManager에 ItemData로 치환한 값 itemData를 M_PartsData에 입력한다.(인벤토리 내에 있는 아이템은 InventoryItem타입이라 ItemData와 호환불가).
            GameManager.instance.M_InteractableData_Read(itemData);
        }
        //Material 씌우기.
        if (mat)
        {
            foreach(Renderer iter in Part.GetComponentsInChildren<Renderer>())
            {
                iter.material = mat;
            }
        }
        if (Fit_Clip != null)
        {
            InteractSound.clip = Fit_Clip;
            InteractSound.Play();
        }
    }

    void Mannequin_Active(bool setbool)
    {
        Mannequin_Head.SetActive(setbool);
        Mannequin_ArmL.SetActive(setbool);
        Mannequin_ArmR.SetActive(setbool);
        Mannequin_LegL.SetActive(setbool);
        Mannequin_LegR.SetActive(setbool);
    }
}
