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
    private PlayerHealth playerHealth;
    [SerializeField] private Slider playerHealthBar;
    [SerializeField] private Image inventoryImage;
    [SerializeField] private Text playerAmmoText;
    [SerializeField] private Text inventoryItemNum;
    
    //��ȣ�ۿ� Ű UI
    [SerializeField] private Image ItemGetKey;
    [SerializeField] private Image ExecuteKey;
    
    private void Awake()
    {
        //�÷��̾ ã�Ƽ� PlayerHealth��ũ��Ʈ�� �����´�.
        var player = GameObject.Find("Player");
        if (player)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }
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
        InventoryDisable();
    }

    //�÷��̾��� ü�¿� ������ ���������� UI�� ü�¼�ġ�� �����Ѵ�.
    public void UpdateplayerHealthBar()
    {
        playerHealthBar.value = playerHealth.f_Health;
    }
    public void UpdateAmmoText(int magAmmo, int remainAmmo)
    {
        playerAmmoText.text = string.Format("{0} / {1}", magAmmo, remainAmmo);
    }
    public void UpdateInventory(string spriteName, int newNum)
    {
        inventoryImage.sprite = Resources.Load<Sprite>(string.Format("Sprites/{0}", spriteName)); ;
        if (inventoryImage.sprite == null)
        {
            Debug.Log(string.Format("Sprites/{0}", spriteName));
            return;
        }

        if(!inventoryItemNum.gameObject.activeSelf)
            inventoryItemNum.gameObject.SetActive(true);
        inventoryItemNum.text = newNum.ToString();
    }
    public void InventoryDisable()
    {
        inventoryImage.sprite = null;
        inventoryItemNum.gameObject.SetActive(false);
    }
}
