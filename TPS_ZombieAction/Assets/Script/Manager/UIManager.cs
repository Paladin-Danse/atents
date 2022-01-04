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
    //싱글톤
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

    //클리어UI
    [SerializeField] private GameObject ClearGoal;//게임목표
    private Text Clear_Text;
    [SerializeField] private int Clear_GoalCount;//목표달성 요구값
    private int Clear_CurrentCount = 0;//현재달성한 목표값
    [SerializeField] private GameObject ClearUI;

    [SerializeField] private GameObject GameOverUI;
    //상호작용 키 UI
    [SerializeField] private Sprite ItemGetKey;
    [SerializeField] private Sprite ExecuteKey;
    private Image InteractionKeyImg;

    private void Awake()
    {
        //플레이어를 찾아서 PlayerHealth스크립트를 가져온다.
        var player = GameObject.Find("Player");
        if (player)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }
        var temp = transform.Find("InteractionKeyGuide");
        if (temp) InteractionKeyImg = temp.GetComponent<Image>();

        if (ClearGoal)
        {
            ClearGoal.SetActive(true);
            Clear_Text = ClearGoal.GetComponentInChildren<Text>();
            if (Clear_Text)
            {
                Clear_Text.text = string.Format("{0} / {1}", Clear_CurrentCount, Clear_GoalCount);
            }
            else
            {
                Debug.Log("UIManager Error : ClearGoal Text Not Found!");
            }
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
            //게임이 시작되면 플레이어에게서 체력값을 가져와 UI에 적용시킨다.
            playerHealthBar.maxValue = playerHealth.f_StartingHealth;
            playerHealthBar.value = playerHealth.f_Health;
        }
        InteractionExit();
        InventoryDisable();
        CrosshairUI.gameObject.SetActive(false);

        ClearUI.SetActive(false);
        GameOverUI.SetActive(false);
    }

    //플레이어의 체력에 변동이 있을때마다 UI의 체력수치를 수정한다.
    public void UpdateplayerHealthBar()
    {
        playerHealthBar.value = playerHealth.f_Health;
    }
    //탄약UI갱신함수
    public void UpdateAmmoText(int magAmmo, int remainAmmo)
    {
        playerAmmoText.text = string.Format("{0} / {1}", magAmmo, remainAmmo);
    }
    //인벤토리 UI갱신함수
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
    //목표 아이템을 먹을 때 갱신해주는 함수
    public void UpdateGoalCount(int Count)
    {
        Clear_CurrentCount += Count;
        Clear_Text.text = string.Format("{0} / {1}", Clear_CurrentCount, Clear_GoalCount);

        //목표 요구치 이상을 획득하면 게임 클리어
        if (Clear_CurrentCount >= Clear_GoalCount)
        {
            GameManager.instance.GameClear();
            OnClearUI();
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

    public void OnClearUI()
    {
        ClearUI.SetActive(true);
    }

    public void OnGameOverUI()
    {
        GameOverUI.SetActive(true);
    }

    public void OnRestart()
    {
        Debug.Log("Restart");
        GameManager.instance.LoadScene("Workbench");
    }
    public void OnExit()
    {
        Debug.Log("Exit");
        GameManager.instance.LoadScene("StartGame");
    }
}
