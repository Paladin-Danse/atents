using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemLooting : MonoBehaviour
{
    private PlayerInput playerInput;
    private AudioSource playerAudioPlayer;
    [SerializeField] private AudioClip itemPickUp;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAudioPlayer = GetComponent<AudioSource>();
    }

    private void OnTriggerStay(Collider other)
    {
        i_Item item = other.GetComponent<i_Item>();
        if (item != null)
        {
            if (playerInput.playerInteraction)
            {
                item.Loot(gameObject);

                playerAudioPlayer.PlayOneShot(itemPickUp);
            }
        }
    }
}
