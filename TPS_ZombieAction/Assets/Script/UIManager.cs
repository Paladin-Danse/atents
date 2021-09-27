using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    //�̱���
    private static UIManager m_instance;
    public static UIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>();
            }
            return m_instance;
        }
    }
    public PlayerHealth playerHealth;
    public Slider playerHealthBar;

    private void Awake()
    {
        //�÷��̾ ã�Ƽ� PlayerHealth��ũ��Ʈ�� �����´�.
        var player = GameObject.Find("Player");
        if (player) playerHealth = player.GetComponent<PlayerHealth>();
    }

    private void Start()
    {
        Setup();
    }
    
    public void Setup()
    {
        if (playerHealth != null)
        {
            //������ ���۵Ǹ� �÷��̾�Լ� ü�°��� ������ UI�� �����Ų��.
            playerHealthBar.maxValue = playerHealth.f_StartingHealth;
            playerHealthBar.value = playerHealth.f_Health;
        }
    }

    //�÷��̾��� ü�¿� ������ ���������� UI�� ü�¼�ġ�� �����Ѵ�.
    public void UpdateplayerHealthBar()
    {
        playerHealthBar.value = playerHealth.f_Health;
    }
}
