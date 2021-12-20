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

    [SerializeField] private Text MainWeaponText;
    [SerializeField] private Text SubWeaponText;
    [SerializeField] private Text MeleeWeaponText;

    [SerializeField] private string MainWeapon;
    [SerializeField] private string SubWeapon;
    [SerializeField] private string MeleeWeapon;

    public void WeaponSelectUpdate()
    {
        if(MainWeaponText.text != null)
        {
            MainWeapon = MainWeaponText.text;
        }

        if(SubWeaponText.text != null)
        {
            SubWeapon = SubWeaponText.text;
        }

        if(MeleeWeaponText.text != null)
        {
            MeleeWeapon = MeleeWeaponText.text;
        }
    }
}
