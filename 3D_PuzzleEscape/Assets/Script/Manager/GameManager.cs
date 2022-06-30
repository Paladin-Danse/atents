using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        Player = GameObject.Find("Player");
        if(!Player)
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
    }

    void Start()
    {
        OffCursorVisible();
        portal.SetActive(false);
        hint_Obj.SetActive(true);
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
}