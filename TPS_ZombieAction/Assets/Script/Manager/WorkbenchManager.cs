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
    [SerializeField] private float TextActiveTime;//NotReadyText�� �������� �ð�
    float ActiveLastTime;//NotReadyText�� �ؽ�Ʈ�� ���� ������ �ð�
    float lerpTime;

    [SerializeField] private string MainWeapon;
    [SerializeField] private string SubWeapon;
    [SerializeField] private string MeleeWeapon;

    private void Awake()
    {
        MainWeapon = SubWeapon = MeleeWeapon = null;
    }

    private void FixedUpdate()
    {
        if (NotReadyText.gameObject.activeSelf)
        {
            if (Time.time >= TextActiveTime + ActiveLastTime)
            {
                lerpTime += Time.deltaTime / 1.0f;//1.0f = 1��
                NotReadyText.color = Color.Lerp(Color.black, Color.clear, lerpTime);//���������� ȿ��(������ ���������� 1���� �ð��� �ɸ�)
                if (NotReadyText.color == Color.clear) NotReadyText.gameObject.SetActive(false);
            }
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
            NotReadyText.text = "���� �������� ���� ���Ⱑ �־� ������ �� �����ϴ�.";
            NotReadyText.color = Color.black;
            ActiveLastTime = Time.time;
            lerpTime = 0f;
            NotReadyText.gameObject.SetActive(true);
        }
    }
}
