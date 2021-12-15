using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkbenchManager : MonoBehaviour
{
    private static WorkbenchManager m_instance;
    public static WorkbenchManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<WorkbenchManager>();
            }
            return m_instance;
        }
    }

    private Weapon MainWeapon;
    private Weapon SubWeapon;
    private Weapon MeleeWeapon;

    [SerializeField] private List<string> MainWeaponList;
    [SerializeField] private List<string> SubWeaponList;
    [SerializeField] private List<string> MeleeWeaponList;

    [SerializeField] private ScrollRect MainWeaponScrollView;
    [SerializeField] private ScrollRect SubWeaponScrollView;

    private void Start()
    {
        List<Text> weaponlist = new List<Text>();
        foreach (var i in MainWeaponList)
        {
            Text text;

            //이런 느낌으로 코드를 짜야됨.
            //weaponlist.Add(text);
        }
    }

    public void ListOpen_Main()
    {
        if(MainWeaponList.Count > 0)
        {
            
        }
    }

    public void ListOpen_Sub()
    {
        if(SubWeaponList.Count > 0)
        {

        }
    }

    public void MainWeaponSelect(Weapon weapon)
    {
        MainWeapon = weapon;
    }

    public void SubWeaponSelect(Weapon weapon)
    {
        SubWeapon = weapon;
    }
}
