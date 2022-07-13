using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    /*
    [Serializable]
    public struct helpInfo
    {
        public Image PressButton_img;
        public Text Info_txt;
    }
    */
    private static UIManager m_instance;
    public static UIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = GameObject.FindObjectOfType<UIManager>();
            }
            return m_instance;
        }
    }
    //��ȣ�ۿ�UI
    [SerializeField] private Image InteractUI;
    //�̴ϰ���UI
    [SerializeField] private GameObject MiniGameUI;
    [SerializeField] private Button Button_GameStart;
    [SerializeField] private Button Button_GameOption;

    //������ ���
    private GameObject Content;
    [SerializeField] private GameObject ItemUI;
    [SerializeField] private GameObject SelectedUI;
    [SerializeField] private GameObject SelectedMixUI;

    //����UI
    [SerializeField] private GameObject Ending_RestartButtonUI;
    [SerializeField] private GameObject Ending_HomeButtonUI;

    /*
    //HelpInfo
    [SerializeField] private GameObject HelpInfoUI;
    private helpInfo Default_Info;
    */
    //�κ��丮 UI
    [SerializeField] private ScrollRect Inventory_Scroll;
    private RectTransform Select;
    private RectTransform SelectMix;
    private List<ItemUI> itemUIList;
    private GameObject Description;
    private Text Text_itemdes;

    //����UI ���۳�Ʈ
    private Button Button_Restart;
    private Button Button_Home;

    //�κ��丮�� ������ �ִ��� ������ üũ�ϰ� �ش� ������ ������ �÷��̾�� ���������� �����ִ� �̹���UI(������ üũ�ϴ� ������ �ƴϴ�.)
    private Image GreenPotion_Chk;
    private Image RedPotion_Chk;
    private Image BluePotion_Chk;

    //����UI
    //Intro, Main ��� �������� ���� �Ȱ��� �����Ǿ���ϸ� �̴� ����UI�� �ٸ� ���� �߰��Ǿ �Ȱ��� ����Ǿ�� �ϴ� ������.
    //�׷��Ƿ� ��� ������ ������Ʈ�� ���� ������Ʈ�� ����ִ� ������� ������ ä���ִ� �� �Ұ����ϸ� �ɼ�UI�� �� �� DataManager�� ����� ���� ������ �ҷ��� ä���ִ� ����� ����� �� �ϴ�.
    private GameObject GameOptionUI;
    private Button Button_OptionExit;
    public Slider Slider_Audio { get; private set; }//Intro, Main ��� �������� ���� �Ȱ��� �����Ǿ���ϸ� �̴� ����UI�� �ٸ� ���� �߰��Ǿ �Ȱ��� ����Ǿ�� �ϴ� ������.
    private Text Text_AudioVolume;

    private void Awake()
    {
        Content = Inventory_Scroll.transform.Find("Viewport/Content").gameObject;
        itemUIList = new List<ItemUI>();
        Description = transform.Find("Inventory_Scroll").Find("Description").gameObject;
        Text_itemdes = Description.transform.Find("Text").GetComponent<Text>();
        Button_Restart = Ending_RestartButtonUI.GetComponent<Button>();
        Button_Home = Ending_HomeButtonUI.GetComponent<Button>();
        Button_GameStart = transform.Find("GameStart").gameObject.GetComponent<Button>();
        Button_GameOption = transform.Find("GameOption").gameObject.GetComponent<Button>();
        GameOptionUI = transform.Find("OptionUI").gameObject;
        Button_OptionExit = GameOptionUI.transform.Find("ExitButton").GetComponent<Button>();
        Slider_Audio = GameOptionUI.transform.Find("Audio").Find("AudioSlider").GetComponent<Slider>();
        Text_AudioVolume = GameOptionUI.transform.Find("Audio").Find("VolumeValue").GetComponent<Text>();

        GreenPotion_Chk = MiniGameUI.transform.Find("MixPotionUI").Find("GreenPotion").Find("Empty").GetComponent<Image>();
        RedPotion_Chk = MiniGameUI.transform.Find("MixPotionUI").Find("RedPotion").Find("Empty").GetComponent<Image>();
        BluePotion_Chk = MiniGameUI.transform.Find("MixPotionUI").Find("BluePotion").Find("Empty").GetComponent<Image>();

        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        if(SelectedUI)
        {
            Select = Instantiate(SelectedUI, Content.transform).GetComponent<RectTransform>();
            Select.gameObject.SetActive(false);
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("SelectedUI is Not Found!");
#endif
        }

        if (SelectedMixUI)
        {
            SelectMix = Instantiate(SelectedMixUI, Content.transform).GetComponent<RectTransform>();
            SelectMix.gameObject.SetActive(false);
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("SelectedMixUI is Not Found!");
#endif
        }
        GameOptionUI.SetActive(false);
        Off_MiniUI();
        SetVolumeText(AudioManager.instance.GetVolume().ToString());
    }

    public void OnScene(string sceneName)
    {
        if(sceneName == "IntroScene")
        {
            SetUI(false);
            SetIntroUI(true);
        }
        else if(sceneName == "MainScene")
        {
            SetUI(true);
            GameOptionUI.SetActive(false);
            Off_MiniUI();
            //���ӸŴ����� IntroScene���� �����Ǿ� Editor���� ������Ʈ�� ���� ���� ���� ����.
            //��� UI�Ŵ����� �����Ǵ� ������ ���ӸŴ����� ã�� ��ư ���۳�Ʈ�� ������ onClick�� Event�� ����ִ´�.
            //�̿ܿ� �ٸ� ��ư�� ������ ����
            Button_Restart.onClick.AddListener(() => GameManager.instance.SceneMove("MainScene"));
            Button_Home.onClick.AddListener(() => GameManager.instance.SceneMove("IntroScene"));
            if (GameManager.instance.playerMovement)
                Button_OptionExit.onClick.AddListener(() => GameManager.instance.playerMovement.Option());

            SetIntroUI(false);
        }
    }

    public void ItemUICreate(InventoryItem m_item)
    {
        var UIObject = Instantiate(ItemUI, Content.transform);
        var UIScript = UIObject.GetComponent<ItemUI>();
        UIScript.item_img.sprite = m_item.data.ItemSprite;
        UIScript.item_name.text = m_item.data.name;

        itemUIList.Add(UIScript);
        UIObject.SetActive(false);
    }

    public void ItemUIEnable(InventoryItem m_item)
    {
        if (m_item.data.Quantity > 0)
        {
            var itemUI = itemUIList.Find(i => i.gameObject.activeSelf == false && i.item_name.text == m_item.data.name);
            if (itemUI)
            {
                itemUI.UIUpdate(m_item.data.ItemSprite, m_item.data.name);
                itemUI.gameObject.SetActive(true);
            }
        }
    }
    public void ItemUIDisable(InventoryItem m_item)
    {
        var itemUI = itemUIList.Find(i => i.item_name.text == m_item.data.name);
        if (itemUI)
        {
            itemUI.gameObject.SetActive(false);
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("itemUI is Not Found!");
#endif
        }
    }

    public void SelectItemUI(InventoryItem m_item)
    {
        var itemUI = itemUIList.Find(i => i.item_name.text == m_item.data.name);
        
        if (itemUI)
        {
            Select.SetParent(itemUI.transform);
            Select.SetAsFirstSibling();

            Select.offsetMin = new Vector2(0, 0);
            Select.offsetMax = new Vector2(0, 0);
            Select.gameObject.SetActive(true);

            SelectItemDescriptionUI(m_item);
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("itemUI is Not Found!");
#endif
        }
        //������â�� ��ũ�ѹٸ� �����ϴ� �ڵ�. ��ũ�ѹٰ� ��Ÿ�� ������ ���� �������� ������ ���� �۵�.
        if (Inventory_Scroll.verticalScrollbar.IsActive())
        {
            List<ItemUI> items = itemUIList.FindAll(i => i.gameObject.activeSelf == true);
            float itemScrollValue = 1.0f - ((float)items.FindIndex(i => i == itemUI) / (float)(items.Count - 1));
            Inventory_Scroll.verticalScrollbar.value = itemScrollValue;
        }
    }
    public void SelectItemDescriptionUI(InventoryItem m_item)
    {
        Text_itemdes.gameObject.SetActive(true);
        Text_itemdes.text = m_item.data.Description;
    }
    public void SetDescriptionUI(bool setbool)
    {
        Text_itemdes.gameObject.SetActive(false);//false�� ���� �����Ȱ��� �Ǽ��� �ƴ�. ������ ������ �������� ���õ� ���� Ȱ��ȭ�� �Ǳ� ����.
        Description.SetActive(setbool);
    }
    public void SelectUIDisable()
    {
        Select.gameObject.SetActive(false);
        SetDescriptionUI(true);
    }
    public void SelectMixUI()
    {
        if(Select.gameObject.activeSelf)
        {
            SelectMix.SetParent(Select.parent);
            SelectMix.SetAsFirstSibling();

            SelectMix.offsetMin = new Vector2(0, 0);
            SelectMix.offsetMax = new Vector2(0, 0);
            SelectMix.gameObject.SetActive(true);
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("Select(RactTransfrom) is Not Active!!");
#endif
        }
    }
    public void SelectMixUIDisable()
    {
        SelectMix.gameObject.SetActive(false);
    }

    public void OnInteractionUI()
    {
        InteractUI.color = Color.black;
    }

    public void OffInteractionUI()
    {
        InteractUI.color = new Color(1.0f, 1.0f, 1.0f, 30f / 255f);
    }
    //MainScene �÷��̾� UI ���� �Լ�
    public void SetUI(bool setbool)
    {
        InteractUI.gameObject.SetActive(setbool);
        Inventory_Scroll.gameObject.SetActive(setbool);
    }
    //IntroScene UI ���� �Լ�
    public void SetIntroUI(bool setbool)
    {
        Button_GameStart.gameObject.SetActive(setbool);
        Button_GameOption.gameObject.SetActive(setbool);
    }
    //���� ���� �Լ�
    //����UI����
    public void SetGameOptionUI(bool setbool)
    {
        GameOptionUI.SetActive(setbool);
    }
    //������ ���� �ؽ�Ʈ
    public void SetVolumeText(string value)
    {
        Text_AudioVolume.text = value;
    }
    public void On_MiniUI()
    {
        MiniGameUI.SetActive(true);
    }
    public void Off_MiniUI()
    {
        MiniGameUI.SetActive(false);
    }

    //�̴ϰ��� �������� UI���� �Լ�
    public void MixPotionUICheck(string name)
    {
        if(name == "�������")
        {
            GreenPotion_Chk.gameObject.SetActive(false);
        }
        else if(name == "��������")
        {
            RedPotion_Chk.gameObject.SetActive(false);
        }
        else if (name == "�Ķ�����")
        {
            BluePotion_Chk.gameObject.SetActive(false);
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("Error(UIManager.MixPotionUICheck) : The name of that UI could not be found.");
#endif
        }
    }
    public bool MixPotionUIOnOffCheck(string name)
    {
        if(name == "�������")
        {
            return !GreenPotion_Chk.gameObject.activeSelf;
        }
        else if (name == "��������")
        {
            return !RedPotion_Chk.gameObject.activeSelf;
        }
        else if (name == "�Ķ�����")
        {
            return !BluePotion_Chk.gameObject.activeSelf;
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("Error(UIManager.MixPotionUIOnOffCheck) : The name of that UI could not be found.");
#endif
            return false;
        }
    }
    /*
    public void Set_HelpInfo(Image image, Text txt)
    {
        helpInfo newinfo = new helpInfo();
        newinfo.PressButton_img = image;
        newinfo.Info_txt = txt;
    }
    */
}
