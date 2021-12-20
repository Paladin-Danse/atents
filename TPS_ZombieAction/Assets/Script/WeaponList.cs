using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponList : MonoBehaviour
{
    [SerializeField] private string CharactorName = "test";
    [SerializeField] private string WeaponTypeName = "MainWeapon";
    [SerializeField] private List<string> weapons;
    [SerializeField] private GameObject Weapon_UI;
    [SerializeField] private GameObject Content;
    [SerializeField] private Image WeaponImage;

    private TextAsset weapon_file;
    private List<GameObject> weaponList;
    private ScrollRect Scrollview;
    // Start is called before the first frame update
    void Start()
    {
        weaponList = new List<GameObject>();

        WeaponFileLoad();
        ListCreate();

        gameObject.SetActive(false);
        WeaponImage.gameObject.SetActive(false);
        Scrollview = GetComponent<ScrollRect>();
    }

    public void ListOpen()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
    //���� �ҷ�����
    public void WeaponFileLoad()
    {
        //weapons.Clear();
        weapons = new List<string>();

        weapon_file = Resources.Load("TextData/WeaponList/" + CharactorName + "_" + WeaponTypeName) as TextAsset;
        if (weapon_file)
        {
            string[] weapon = weapon_file.text.Split('\n');//���� ���Ͽ��� ���� �̸��� ������ �ϳ��� �迭�� ����
            for (int i = 0; i < weapon.Length; i++)
            {
                weapons.Add(weapon[i].Trim('\r', '\n'));//���Ͽ��� �����͸� �ѱ�鼭 ���� �Ѿ�� �ܵ����ʹ� Trim���� �ĳ���.
            }
        }
        else
        {
            Debug.Log("weapon_file Not Found!!\nCharactorName : " + CharactorName + "\nWeaponTypeName : " + WeaponTypeName);//����ó��
        }
    }
    //�ҷ��� �������Ϸ� ��� �����
    public void ListCreate()
    {
        if(weapons.Count > 0 && weapons != null)
        {
            for(int i = 0; i < weapons.Count; i++)
            {
                //transform.Find�� Content�� ã�� ����. �ٸ� ��� �ʿ�.
                GameObject weapon = Instantiate(Weapon_UI, Content.transform);
                weapon.transform.localScale = new Vector3(Content.transform.localScale.x, weapon.transform.localScale.y, weapon.transform.localScale.z);

                Text weaponName = weapon.transform.Find("Text").GetComponent<Text>();
                if (weaponName)
                {
                    weaponName.text = weapons[i];
                }

                Button weaponOnButton = weapon.GetComponent<Button>();
                if (weaponOnButton)
                {
                    weaponOnButton.onClick.AddListener(() => WeaponSelect(weaponName.text));
                }

                weaponList.Add(weapon);
            }
        }
    }
    //��Ͽ� �� ����� �� �ϳ��� ����������
    public void WeaponSelect(string WeaponName)
    {
        WeaponImage.sprite = Resources.Load<Sprite>(string.Format("Sprites/{0}_img", WeaponName));
        if (WeaponImage.sprite)
        {
            Text text = WeaponImage.GetComponentInChildren<Text>();
            if (text)
            {
                text.text = WeaponName;
            }
            else
            {
                Debug.Log("Not Found ImageText");
            }
        }
        else
        {
            Debug.Log("Not Found WeaponSprite!");
        }
        WorkbenchManager.instance.WeaponSelectUpdate();//������ ���Ⱑ �ٲ������ �˷���

        WeaponImage.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
