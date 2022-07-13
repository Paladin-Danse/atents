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
    //상호작용UI
    [SerializeField] private Image InteractUI;
    //미니게임UI
    [SerializeField] private GameObject MiniGameUI;
    [SerializeField] private Button Button_GameStart;
    [SerializeField] private Button Button_GameOption;

    //아이템 목록
    private GameObject Content;
    [SerializeField] private GameObject ItemUI;
    [SerializeField] private GameObject SelectedUI;
    [SerializeField] private GameObject SelectedMixUI;

    //엔딩UI
    [SerializeField] private GameObject Ending_RestartButtonUI;
    [SerializeField] private GameObject Ending_HomeButtonUI;

    /*
    //HelpInfo
    [SerializeField] private GameObject HelpInfoUI;
    private helpInfo Default_Info;
    */
    //인벤토리 UI
    [SerializeField] private ScrollRect Inventory_Scroll;
    private RectTransform Select;
    private RectTransform SelectMix;
    private List<ItemUI> itemUIList;
    private GameObject Description;
    private Text Text_itemdes;

    //엔딩UI 컴퍼넌트
    private Button Button_Restart;
    private Button Button_Home;

    //인벤토리에 포션이 있는지 없는지 체크하고 해당 포션의 유무를 플레이어에게 직접적으로 보여주는 이미지UI(실제로 체크하는 변수가 아니다.)
    private Image GreenPotion_Chk;
    private Image RedPotion_Chk;
    private Image BluePotion_Chk;

    //설정UI
    //Intro, Main 어느 씬에서든 값이 똑같이 유지되어야하며 이는 설정UI에 다른 값이 추가되어도 똑같이 적용되어야 하는 사항임.
    //그러므로 어느 씬에서 컴포넌트에 직접 오브젝트를 끌어넣는 방식으로 변수를 채워넣는 건 불가능하며 옵션UI를 열 때 DataManager에 저장된 값과 변수를 불러와 채워넣는 방식을 써야할 듯 하다.
    private GameObject GameOptionUI;
    private Button Button_OptionExit;
    public Slider Slider_Audio { get; private set; }//Intro, Main 어느 씬에서든 값이 똑같이 유지되어야하며 이는 설정UI에 다른 값이 추가되어도 똑같이 적용되어야 하는 사항임.
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
            //게임매니저가 IntroScene에서 생성되어 Editor에선 오브젝트를 직접 넣을 수가 없다.
            //고로 UI매니저가 생성되는 순간에 게임매니저를 찾아 버튼 컴퍼넌트를 가져와 onClick에 Event를 집어넣는다.
            //이외에 다른 버튼도 동일한 이유
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
        //아이템창의 스크롤바를 조작하는 코드. 스크롤바가 나타날 정도로 많은 아이템이 들어왔을 때만 작동.
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
        Text_itemdes.gameObject.SetActive(false);//false로 값이 고정된것은 실수가 아님. 설명은 무조건 아이템이 선택될 때만 활성화가 되기 때문.
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
    //MainScene 플레이어 UI 관련 함수
    public void SetUI(bool setbool)
    {
        InteractUI.gameObject.SetActive(setbool);
        Inventory_Scroll.gameObject.SetActive(setbool);
    }
    //IntroScene UI 관련 함수
    public void SetIntroUI(bool setbool)
    {
        Button_GameStart.gameObject.SetActive(setbool);
        Button_GameOption.gameObject.SetActive(setbool);
    }
    //설정 관련 함수
    //설정UI관련
    public void SetGameOptionUI(bool setbool)
    {
        GameOptionUI.SetActive(setbool);
    }
    //사운드의 볼륨 텍스트
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

    //미니게임 물약조합 UI관련 함수
    public void MixPotionUICheck(string name)
    {
        if(name == "녹색물약")
        {
            GreenPotion_Chk.gameObject.SetActive(false);
        }
        else if(name == "빨간물약")
        {
            RedPotion_Chk.gameObject.SetActive(false);
        }
        else if (name == "파란물약")
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
        if(name == "녹색물약")
        {
            return !GreenPotion_Chk.gameObject.activeSelf;
        }
        else if (name == "빨간물약")
        {
            return !RedPotion_Chk.gameObject.activeSelf;
        }
        else if (name == "파란물약")
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
