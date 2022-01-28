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
        
    }

    void Start()
    {
        OffCursorVisible();
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
}
