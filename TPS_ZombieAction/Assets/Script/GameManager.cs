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
            if (!m_instance)
                m_instance = FindObjectOfType<GameManager>();
            return m_instance;
        }
    }

    public PlayerShooter playerShooter { get; private set; }
    public PlayerHealth playerHealth { get; private set; }
    public PlayerInput playerInput { get; private set; }
    public PlayerItemLooting playerItemLooting { get; private set; }
    public PlayerMovement playerMovement { get; private set; }

    private void Awake()
    {
        GameObject player = GameObject.Find("Player");
        playerShooter = player.GetComponent<PlayerShooter>();
        playerHealth = player.GetComponent<PlayerHealth>();
        playerInput = player.GetComponent<PlayerInput>();
        playerItemLooting = player.GetComponent<PlayerItemLooting>();
        playerMovement = player.GetComponent<PlayerMovement>();
    }
}
