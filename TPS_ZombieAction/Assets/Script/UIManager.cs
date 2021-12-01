using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum INTERACTION
{
    NONE=0,
    GETITEM,
    EXECUTE
}

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
                if(m_instance == null)
                {
                    m_instance = Instantiate(new UIManager());
                }
            }
            return m_instance;
        }
    }
    private PlayerHealth playerHealth;
    [SerializeField] private Slider playerHealthBar;
    [SerializeField] private Image inventoryImage;
    [SerializeField] private Text playerAmmoText;
    [SerializeField] private Text inventoryItemNum;
    [SerializeField] private GunCrosshair CrosshairUI;

    //��ȣ�ۿ� Ű UI
    [SerializeField] private Sprite ItemGetKey;
    [SerializeField] private Sprite ExecuteKey;
    private Image InteractionKeyImg;

    private void Awake()
    {
        //�÷��̾ ã�Ƽ� PlayerHealth��ũ��Ʈ�� �����´�.
        var player = GameObject.Find("Player");
        if (player)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }
        var temp = transform.Find("InteractionKeyGuide");
        if (temp) InteractionKeyImg = temp.GetComponent<Image>();

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
        InteractionExit();
        InventoryDisable();
        CrosshairUI.gameObject.SetActive(false);
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
        if (spriteName == null)
        {
            inventoryImage.sprite = null;
            inventoryItemNum.gameObject.SetActive(false);
            return;
        }
        else
        {
            inventoryImage.sprite = Resources.Load<Sprite>(string.Format("Sprites/{0}", spriteName));
            if (inventoryImage.sprite == null)
            {
                Debug.Log(string.Format("Sprites/{0}", spriteName));
                return;
            }

            if (!inventoryItemNum.gameObject.activeSelf)
                inventoryItemNum.gameObject.SetActive(true);
            inventoryItemNum.text = newNum.ToString();
        }
    }
    public void InventoryDisable()
    {
        inventoryImage.sprite = null;
        inventoryItemNum.gameObject.SetActive(false);
    }

    public void InteractionEnter(INTERACTION inter)
    {
        switch (inter)
        {
            case INTERACTION.EXECUTE:
                InteractionKeyImg.sprite = ExecuteKey;
                break;
            case INTERACTION.GETITEM:
                InteractionKeyImg.sprite = ItemGetKey;
                break;
        }
        InteractionKeyImg.gameObject.SetActive(true);
    }

    public void InteractionExit()
    {
        InteractionKeyImg.gameObject.SetActive(false);
        InteractionKeyImg.sprite = null;
    }

    public void CrosshairEnable()
    {
        CrosshairUI.gameObject.SetActive(true);
    }
    public void CrosshairDisable()
    {
        CrosshairUI.gameObject.SetActive(false);
    }

    public void CrosshairRecoil(float RecoilValue)
    {
        CrosshairUI.CrosshairDivide(RecoilValue);
    }

    public float CrosshairReturnSize()
    {
        return CrosshairUI.GetSize();
    }

    public void SetGunAccuracy(float Accuracy)
    {
        CrosshairUI.GunAccuracyToSize(Accuracy);
    }
}
