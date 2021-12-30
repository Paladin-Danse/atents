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
    [SerializeField] private Text NotReadyText;
    [SerializeField] private float TextActiveTime;

    [SerializeField] private string MainWeapon;
    [SerializeField] private string SubWeapon;
    [SerializeField] private string MeleeWeapon;

    private void Awake()
    {
        MainWeapon = SubWeapon = MeleeWeapon = null;
    }

    private void Update()
    {
        if (NotReadyText.gameObject.activeSelf)
        {
            NotReadyText.color = Color.Lerp(NotReadyText.color, Color.clear, 1.0f / (TextActiveTime * 60.0f));
            if (NotReadyText.color == Color.clear) NotReadyText.gameObject.SetActive(false);
        }
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
            GameManager.instance.LoadScene("Map_v1");
        }

        else
        {
            NotReadyText.text = "아직 장착하지 않은 무기가 있어 시작할 수 없습니다.";
            NotReadyText.color = Color.black;
            NotReadyText.gameObject.SetActive(true);
        }
    }
}
