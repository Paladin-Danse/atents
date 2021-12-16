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

    [SerializeField] private ScrollRect MainWeaponScrollView;
    [SerializeField] private ScrollRect SubWeaponScrollView;

    private void Start()
    {
    }

    public void ListOpen_Main()
    {
        if (MainWeaponScrollView)
        {
            if (MainWeaponScrollView.gameObject.activeSelf)
                MainWeaponScrollView.gameObject.SetActive(false);
            else
                MainWeaponScrollView.gameObject.SetActive(true);
            
        }
    }

    public void ListOpen_Sub()
    {
        if (SubWeaponScrollView)
        {
            if (SubWeaponScrollView.gameObject.activeSelf)
                SubWeaponScrollView.gameObject.SetActive(false);
            else
                SubWeaponScrollView.gameObject.SetActive(true);

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
