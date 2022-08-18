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

    private Mannequin_Example Mannequin_Ex;
    public List<ItemData> Ex_PartsData { get; private set; }
    List<ItemData> M_PartsData;
    private GameObject portal;
    private GameObject hint_Obj;
    public SaveData mySavedata { get; private set; }
    private bool b_Option;

    //[SerializeField] private Vector3 StagetoPlayerPosition;

    private void Awake()
    {
        if (GameManager.instance.gameObject != gameObject) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

        b_Option = false;
    }

    //����Ƽ ��ũ����API���� ������ ����
    //sceneLoaded<Scene, LoadSceneMode>�� ���� ��ȯ�� ������ ������ �ǰ� �Ʒ� �Լ��� �ش� ���������Ϳ� ���������ν� ���� ����ɶ����� �Ʒ� �Լ��� �����Ų��.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainScene")
            GameStart();
        UIManager.instance.OnScene(SceneManager.GetActiveScene().name);
        AudioManager.instance.OnScene(SceneManager.GetActiveScene().name);
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

        Ex_PartsData = new List<ItemData>();
        M_PartsData = new List<ItemData>();

        GameObject asphalt;
        asphalt = GameObject.Find("Hint&Portal");
        portal = asphalt.transform.Find("Portal").gameObject;
        hint_Obj = asphalt.transform.Find("Asphalt Moveable").gameObject;
        Mannequin_Ex = GameObject.Find("Mannequin").GetComponent<Mannequin_Example>();

        OffCursorVisible();
        b_Option = true;

        if (mySavedata != null)
        {
            if (!mySavedata.Mini_3_Clear)
            {
                portal.SetActive(false);
                hint_Obj.SetActive(true);
            }
            else
            {
                portal.SetActive(true);
                hint_Obj.SetActive(false);
            }

            Player.transform.position = new Vector3(mySavedata.PlayerPositionX, mySavedata.PlayerPositionY, mySavedata.PlayerPositionZ);
            Player.transform.rotation = Quaternion.Euler(mySavedata.PlayerRotationX, mySavedata.PlayerRotationY, mySavedata.PlayerRotationZ);

            if (mySavedata.itemdata != null)
            {
                InventoryManager.instance.LoadItem(mySavedata);
            }

            if(mySavedata.b_Mannequin_Data)
            {
                Mannequin_Ex.MannequinEx_DataLoad();
            }
            else
            {
                Mannequin_Ex.newRandomParts_Pit();
                mySavedata.b_Mannequin_Data = true;
                GameSave();
            }
        }
    }
    //���ӽ��۹�ư, ��������۹�ư
    public void FirstGame()
    {
        mySavedata = new SaveData();
        if(InventoryManager.instance) InventoryManager.instance.LostItem();
        StageClear(STAGE.GAMESTART);
    }

    //����ϱ��ư
    public void Continue()
    {
        GameLoad();
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
    public void SetActivePlayer(bool setbool)
    {
        playerMovement.LockMove(setbool);
        playerInteraction.LockInteraction(setbool);
    }
    //�ش� �Լ��� ������� ���� SetActivePlayer�� ���ԵǾ ũ�� ������� �Լ��̳�,
    //PlayerMovement�� Option�Լ����� ����� ��, �Ʒ� �Լ��� �۵��Ǹ� Option�Լ��� ����� �۵����� ���ϴ� �������� ����� ����� ���� �и���.
    public void SetActiveOption(bool setbool)
    {
        b_Option = setbool;
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
        MiniGameClear("Mannequin_Compare");
#if UNITY_EDITOR
        Debug.Log("On_Portal");
#endif
    }

    public void SceneMove(string SceneName)
    {
        UIManager.instance.ItemUIAllDisable();
        SceneManager.LoadScene(SceneName);
    }

    public Scene GetActiveScene()
    {
        return SceneManager.GetActiveScene();
    }

    public bool GetBoolOption()
    {
        return b_Option;
    }

    public void GameSave()
    {
        if (InventoryManager.instance)
        {
            mySavedata.itemdata = InventoryManager.instance.InventorySave(mySavedata.itemdata);
        }
        if (Player)
        {
            mySavedata.PlayerPositionX = Player.transform.position.x;
            mySavedata.PlayerPositionY = Player.transform.position.y;
            mySavedata.PlayerPositionZ = Player.transform.position.z;
            //����ϰ� rotation.x���� ��������� �ϸ� ������ ���� �������� ��찡 �ִ�. eulerAngles�� ���� �ϳ� ��Ȯ�ϴ�. �ٲ��� �� ��.
            mySavedata.PlayerRotationX = Player.transform.rotation.eulerAngles.x;
            mySavedata.PlayerRotationY = Player.transform.rotation.eulerAngles.y;
            mySavedata.PlayerRotationZ = Player.transform.rotation.eulerAngles.z;
        }
        if (!Json.instance.SaveFile(mySavedata))
        {
#if UNITY_EDITOR
            Debug.LogError("Save Failed!");
#endif

        }
        else
        {
            if (UIManager.instance)
            {
                StartCoroutine(UIManager.instance.SaveComplete());
            }
        }
    }

    public void GameSave(Transform StagePosition)
    {
        if (InventoryManager.instance)
        {
            mySavedata.itemdata = InventoryManager.instance.InventorySave(mySavedata.itemdata);
        }
        if (Player)
        {
            mySavedata.PlayerPositionX = StagePosition.position.x;
            mySavedata.PlayerPositionY = StagePosition.position.y;
            mySavedata.PlayerPositionZ = StagePosition.position.z;
            //����ϰ� rotation.x���� ��������� �ϸ� ������ ���� �������� ��찡 �ִ�. eulerAngles�� ���� �ϳ� ��Ȯ�ϴ�. �ٲ��� �� ��.
            mySavedata.PlayerRotationX = StagePosition.rotation.eulerAngles.x;
            mySavedata.PlayerRotationY = StagePosition.rotation.eulerAngles.y;
            mySavedata.PlayerRotationZ = StagePosition.rotation.eulerAngles.z;
        }
        if (!Json.instance.SaveFile(mySavedata))
        {
            Debug.LogError("Save Failed!");
        }
        else
        {
            if (UIManager.instance)
            {
                StartCoroutine(UIManager.instance.SaveComplete());
            }
        }
    }

    public void GameLoad()
    {
        //json��ũ��Ʈ�� ������ �־ �ҷ������� �ϸ� ������ ����. ����׸��� Ȯ���� ��� null������ ó���Ǳ� ������ ����ִ� ������ �Լ��� �ҷ����� ����� �Ǿ���� ������ �Ǵ� ��. �ڵ��� �����ʿ�.
        
        mySavedata = Json.instance.LoadFile();
        if(mySavedata == null)
        {
#if UNITY_EDITOR
            Debug.Log("Error(GameManager) : mySavedata is Not Found!");
#endif
            FirstGame();
        }
    }

    //�������� Ŭ��� �������� ����.
    //���� �۾� ���� ��.
    public void StageClear(STAGE currentStage)
    {
        mySavedata.Stage = currentStage + 1;

        GameObject st1obj = GameObject.Find("Stage1_Pos");
        GameObject st2obj = GameObject.Find("Stage2_Pos");
        GameObject st3obj = GameObject.Find("Stage3_Pos");

        if (st1obj != null && st2obj != null && st3obj != null)
        {
            Transform Stage_transform = mySavedata.Stage switch
            {
                STAGE Stage when Stage == STAGE.STAGE_1 => Stage_transform = st1obj.transform,
                STAGE Stage when Stage == STAGE.STAGE_2 => Stage_transform = st2obj.transform,
                STAGE Stage when Stage == STAGE.STAGE_3 => Stage_transform = st3obj.transform,
                _ => null
            };
            GameSave(Stage_transform);
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("Error(GameManager) : Stage_transform is Not Found!");
#endif
            GameSave();
        }
    }
    
    public void MiniGameClear(string MiniName)
    {
        switch(MiniName)
        {
            case "Safe":
                mySavedata.Mini_1_Clear = true;
                break;
            case "MixPotion":
                mySavedata.Mini_1_Clear = true;
                mySavedata.Mini_2_Clear = true;
                break;
            case "Mannequin_Compare":
                mySavedata.Mini_1_Clear = true;
                mySavedata.Mini_2_Clear = true;
                mySavedata.Mini_3_Clear = true;
                break;
            default:
                break;
        }
        GameSave();
    }

    public void GameExit()
    {
        Application.Quit();
    }
}