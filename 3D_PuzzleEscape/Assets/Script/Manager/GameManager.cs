using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance;
    public static GameManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = GameObject.FindObjectOfType<GameManager>();
            }
            return m_instance;
        }
    }
    public GameObject Player { get; private set; }
    public PlayerInput playerInput { get; private set; }
    public PlayerMovement playerMovement { get; private set; }
    public PlayerInteraction playerInteraction { get; private set; }

    List<ItemData> Ex_PartsData;
    List<ItemData> M_PartsData;
    [SerializeField] private GameObject portal;
    [SerializeField] private GameObject hint_Obj;
    private Json json_save;
    private SaveData mySavedata;

    private void Awake()
    {
        Ex_PartsData = new List<ItemData>();
        M_PartsData = new List<ItemData>();
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        json_save = GetComponent<Json>();
        if (!json_save) Debug.LogError("GameManager Error : Json is Not Found!");
    }
    //유니티 스크립팅API에서 가져온 정보
    //sceneLoaded<Scene, LoadSceneMode>가 씬이 전환될 때마다 실행이 되고 아래 함수를 해당 델리게이터에 대입함으로써 씬이 실행될때마다 아래 함수를 실행시킨다.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainScene")
            GameStart();
        UIManager.instance.OnScene(SceneManager.GetActiveScene().name);
    }

    public void GameStart()
    {
        Player = GameObject.Find("Player");
        if (!Player)
        {
            Debug.Log("Player is Not Found!");
        }
        else
        {
            playerInput = Player.GetComponent<PlayerInput>();
            playerMovement = Player.GetComponent<PlayerMovement>();
            playerInteraction = Player.GetComponent<PlayerInteraction>();
        }

        portal = GameObject.Find("Portal");
        hint_Obj = GameObject.Find("Asphalt Moveable");

        OffCursorVisible();
        portal.SetActive(false);
        hint_Obj.SetActive(true);
        mySavedata = new SaveData();
        GameSave(mySavedata);
    }

    public void OnCursorVisible()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void OffCursorVisible()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void M_Example_RandData_Read(Mannequin_Example M_example)
    {
        Ex_PartsData.Add(M_example.Mannequin_Head);
        Ex_PartsData.Add(M_example.Mannequin_ArmL);
        Ex_PartsData.Add(M_example.Mannequin_ArmR);
        Ex_PartsData.Add(M_example.Mannequin_LegL);
        Ex_PartsData.Add(M_example.Mannequin_LegR);
    }
    public void M_InteractableData_Read(ItemData m_itemData)
    {
        M_PartsData.Add(m_itemData);
    }

    public void M_InteractableData_Delete(ItemData m_itemData)
    {
        M_PartsData.RemoveAll(i => i.Data.name == m_itemData.Data.name);
    }

    public void M_Example_Compare_Data()
    {
        foreach(ItemData iter in Ex_PartsData)
        {
            ItemData Contain = M_PartsData.Find(i => i.Data.name == iter.Data.name);
            if (!Contain)
            {
                return;
            }
        }
        if(hint_Obj) hint_Obj.SetActive(false);
        On_Portal();
    }

    public void On_Portal()
    {
        portal.SetActive(true);
        Debug.Log("On_Portal");
    }

    public void SceneMove(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

    public Scene GetActiveScene()
    {
        return SceneManager.GetActiveScene();
    }

    public void GameSave(SaveData newSave)
    {
        if (!json_save.SaveFile(newSave))
        {
            Debug.LogError("Save Failed!");
        }
    }

    public void GameLoad()
    {
        mySavedata = json_save.LoadFile();
        if (mySavedata == null)
        {
            Debug.LogError("Load Failed!");
        }
    }

    //스테이지 클리어를 기점으로 저장.
    //현재 작업 진행 중.
    public void StageClear()
    {
        mySavedata.Stage += 1;
        mySavedata.itemdata = InventoryManager.instance.InventorySave(mySavedata.itemdata);

        GameSave(mySavedata);
    }
}