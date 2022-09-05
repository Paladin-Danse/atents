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
    [SerializeField] private ItemData[] Mannequin_Data;//����ŷ��ǰ�� �� �� �ִ� ��� ������ �����Ͱ�.
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

        //���̺������� �ְ�, ���� �� ���߱⸦ Ŭ������ ������ ��� ���ӸŴ������� ���� ���������� �����͸� �޾ƿͼ� �Ȱ��� ���� �����ش�. �װ� �ƴ϶�� ����ŷ�� SetActive�� ���ش�.
        if (GameManager.instance.mySavedata != null)
        {
            if (GameManager.instance.mySavedata.Mini_3_Clear)
            {
                List<ItemData> partsData = GameManager.instance.Ex_PartsData;
                ItemData HeadData = partsData.Find(i => i.Data.name.Contains("�����Ӹ�"));
                ItemData ArmLData = partsData.Find(i => i.Data.name.Contains("���� ��"));
                ItemData ArmRData = partsData.Find(i => i.Data.name.Contains("������ ��"));
                ItemData LegLData = partsData.Find(i => i.Data.name.Contains("���� �ٸ�"));
                ItemData LegRData = partsData.Find(i => i.Data.name.Contains("������ �ٸ�"));

                Mannequin_fit(Mannequin_Head, HeadData.Data, HeadData.Data.name);
                Mannequin_fit(Mannequin_ArmL, ArmLData.Data, ArmLData.Data.name);
                Mannequin_fit(Mannequin_ArmR, ArmRData.Data, ArmRData.Data.name);
                Mannequin_fit(Mannequin_LegL, LegLData.Data, LegLData.Data.name);
                Mannequin_fit(Mannequin_LegR, LegRData.Data, LegRData.Data.name);

                if (InteractSound.isPlaying) InteractSound.Stop(); //���̺������� �ҷ����� �������� ����ŷ�� �������ߴ� �Ҹ��� ���ִ� �ڵ�
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

    //����ŷ�� ����ϰ� �ִ� ���������� ��ȣ�ۿ��� �� ����ŷ�� ��ǰ���� üũ�ϰ� ����ŷ�� ��ǰ�� �´ٸ� ��� ������ ���ϴ��� Ȯ���ϴ� �Լ�.
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

                //����ŷ ��ǰ�� �´ٸ� ��� ��ǰ���� �ٽ� üũ
                switch (name)
                {
                    case string key when name.Contains("�����Ӹ�"):
                        Parts = Mannequin_Head;
                        break;
                    case string key when name.Contains("���� ��"):
                        Parts = Mannequin_ArmL;
                        break;
                    case string key when name.Contains("������ ��"):
                        Parts = Mannequin_ArmR;
                        break;
                    case string key when name.Contains("���� �ٸ�"):
                        Parts = Mannequin_LegL;
                        break;
                    case string key when name.Contains("������ �ٸ�"):
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

    //����ŷ ��ǰ�� Ȱ��ȭ�ϰ� ���� ���� �������� Ȯ���ؼ� �ش� Material�� ������ �Լ�.
    void Mannequin_fit(GameObject Part, Item data, string name)
    {
        Material mat;

        Part.SetActive(true);
        
        if (name.Contains("��"))
            mat = Red_Mat;
        else if(name.Contains("û"))
            mat = Blue_Mat;
        else if(name.Contains("��"))
            mat = Green_Mat;
        else
            mat = Default_Mat;

        //���� Ʋ�� ���� ���������� ��� ��ǰ�� �ٽ� ���� �� �ְ� �ش� ��ǰ�� �ֿ� �� �ִ� ���������� �����Ѵ�.
        if (Part.GetComponent<Mannequin_GetParts>())
        {
            ItemData itemData = new ItemData();
            itemData.InputData(data);
            Part.GetComponent<Mannequin_GetParts>().SetItem(itemData);
            Part.GetComponent<Mannequin_GetParts>().SetClip(pullout_Clip);

            //GameManager�� ItemData�� ġȯ�� �� itemData�� M_PartsData�� �Է��Ѵ�.(�κ��丮 ���� �ִ� �������� InventoryItemŸ���̶� ItemData�� ȣȯ�Ұ�).
            GameManager.instance.M_InteractableData_Read(itemData);
        }
        //Material �����.
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
