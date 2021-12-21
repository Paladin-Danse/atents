using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    private void Awake()
    {
        MainWeapon = SubWeapon = MeleeWeapon = null;
    }


    public void WeaponSelectUpdate()
    {
        if(MainWeaponText.text != "")
        {
            MainWeapon = MainWeaponText.text;
        }

        if(SubWeaponText.text != "")
        {
            SubWeapon = SubWeaponText.text;
        }

        if(MeleeWeaponText.text != "")
        {
            MeleeWeapon = MeleeWeaponText.text;
        }
    }

    public void Ready()
    {
        if (MainWeapon != null && SubWeapon != null && MeleeWeapon != null)
        {
            GameManager.instance.PlayerInfomation_Save(MainWeapon, SubWeapon, MeleeWeapon);
            SceneManager.LoadScene("Map_v1");
        }
    }
}
