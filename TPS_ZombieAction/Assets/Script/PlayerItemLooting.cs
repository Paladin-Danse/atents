using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemLooting : MonoBehaviour
{
    private PlayerInput playerInput;
    private AudioSource playerAudioPlayer;
    [SerializeField] private AudioClip itemPickUp;//아이템 주울 때 나는 소리


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAudioPlayer = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Item"))
        {
            UIManager.instance.InteractionEnter(INTERACTION.GETITEM);
        }
    }
    //OnTriggerEnter를 처음 사용했으나 안에서 지속적으로 플레이어의 입력처리를 받아들이지 못해 Stay에서 처리함.
    private void OnTriggerStay(Collider other)
    {
        if(other.tag.Equals("Item"))
        {
            if (playerInput.playerInteraction)//플레이어가 상호작용키(E키)를 누른경우
            {
                var item = other.GetComponent<GetItem>();
                if(item)
                {
                    item.Looting();
                    playerAudioPlayer.PlayOneShot(itemPickUp);//아이템 줍는 사운드 출력
                    UIManager.instance.InteractionExit();
                }
            }
        }
        //CItem item = other.GetComponent<CItem>();//플레이어와 부딪힌 아이템을 받아옴.
        //if (item != null)
        //{
        //    if (playerInput.playerInteraction)//플레이어가 상호작용키(E키)를 누른경우
        //    {
        //        item.Loot(gameObject);//아이템의 Loot함수를 실행
        //
        //        playerAudioPlayer.PlayOneShot(itemPickUp);//아이템 줍는 사운드 출력
        //    }
        //}
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag.Equals("Item"))
        {
            UIManager.instance.InteractionExit();
        }
    }
}
