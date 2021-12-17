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

    public void WeaponFileLoad()
    {
        //weapons.Clear();
        weapons = new List<string>();

        weapon_file = Resources.Load("TextData/WeaponList/" + CharactorName + "_" + WeaponTypeName) as TextAsset;
        if (weapon_file)
        {
            string[] weapon = weapon_file.text.Split('\n');
            for (int i = 0; i < weapon.Length; i++)
            {
                weapons.Add(weapon[i]);
            }
        }
        else
        {
            Debug.Log("weapon_file Not Found!!\nCharactorName : " + CharactorName + "\nWeaponTypeName : " + WeaponTypeName);
        }
    }

    public void ListCreate()
    {
        if(weapons.Count > 0 && weapons != null)
        {
            for(int i = 0; i < weapons.Count; i++)
            {
                //transform.Find로 Content를 찾지 못함. 다른 방법 필요.
                GameObject weapon = Instantiate(Weapon_UI, Content.transform);
                
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

    public void WeaponSelect(string WeaponName)
    {
        WeaponImage.sprite = Resources.Load<Sprite>(string.Format("Sprites/{0}_img", WeaponName.Trim('\r', '\n')));
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
        WorkbenchManager.instance.MainWeaponSelect(WeaponName.Trim('\r', '\n'));

        WeaponImage.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
