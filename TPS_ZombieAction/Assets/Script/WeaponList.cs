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
                GameObject weapon = Instantiate(Weapon_UI, gameObject.transform.Find("Content"));
                weapon.transform.Find("Text").GetComponent<Text>().text = weapons[i];//정상적으로 파일에서 가져온 텍스트값이 들어감. 다만 텍스트값만 들어갔기에 실행하면 UI Canvas밖에서 생성이 되는 바람에 보이진 않음.

                weaponList.Add(weapon);
            }
        }
    }
}
