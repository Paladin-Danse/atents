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
    //파일 불러오기
    public void WeaponFileLoad()
    {
        //weapons.Clear();
        weapons = new List<string>();

        weapon_file = Resources.Load("TextData/WeaponList/" + CharactorName + "_" + WeaponTypeName) as TextAsset;
        if (weapon_file)
        {
            string[] weapon = weapon_file.text.Split('\n');//무기 파일에서 무기 이름을 가져와 하나씩 배열에 저장
            for (int i = 0; i < weapon.Length; i++)
            {
                weapons.Add(weapon[i].Trim('\r', '\n'));//파일에서 데이터를 넘기면서 같이 넘어온 잔데이터는 Trim으로 쳐내기.
            }
        }
        else
        {
            Debug.Log("weapon_file Not Found!!\nCharactorName : " + CharactorName + "\nWeaponTypeName : " + WeaponTypeName);//예외처리
        }
    }
    //불러온 무기파일로 목록 만들기
    public void ListCreate()
    {
        if(weapons.Count > 0 && weapons != null)
        {
            for(int i = 0; i < weapons.Count; i++)
            {
                //transform.Find로 Content를 찾지 못함. 다른 방법 필요.
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
    //목록에 뜬 무기들 중 하나를 선택했을때
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
        WorkbenchManager.instance.WeaponSelectUpdate();//선택한 무기가 바뀌었음을 알려줌

        WeaponImage.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
