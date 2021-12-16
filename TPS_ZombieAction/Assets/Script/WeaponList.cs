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
                //transform.Find�� Content�� ã�� ����. �ٸ� ��� �ʿ�.
                GameObject weapon = Instantiate(Weapon_UI, gameObject.transform.Find("Content"));
                weapon.transform.Find("Text").GetComponent<Text>().text = weapons[i];//���������� ���Ͽ��� ������ �ؽ�Ʈ���� ��. �ٸ� �ؽ�Ʈ���� ���⿡ �����ϸ� UI Canvas�ۿ��� ������ �Ǵ� �ٶ��� ������ ����.

                weaponList.Add(weapon);
            }
        }
    }
}
