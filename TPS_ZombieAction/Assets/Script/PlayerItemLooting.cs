using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemLooting : MonoBehaviour
{
    private PlayerInput playerInput;
    private AudioSource playerAudioPlayer;
    private List<GetItem> Item;
    [SerializeField] private AudioClip itemPickUp;//아이템 주울 때 나는 소리


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAudioPlayer = GetComponent<AudioSource>();
        Item = new List<GetItem>();
    }

    private void Update()
    {
        //상호작용키(E키)를 눌렀을 때 리스트에 담아둔 아이템이 있을 경우, 해당 아이템을 주워서 인벤토리에 넣은 뒤, 리스트에서 제거하고 효과음 재생. 마무리로 리스트에 남은 아이템이 없다면 UI제거.
        if(playerInput.playerInteraction)
        {
            if (Item.Count != 0)
            {
                var item = Item.Find(i => i);
                item.Looting();
                Item.Remove(item);
                playerAudioPlayer.PlayOneShot(itemPickUp);//아이템 줍는 사운드 출력
                if(Item.Count == 0) UIManager.instance.InteractionExit();
            }
        }
    }
    //아이템과 충돌하면 UI를 띄우고 리스트에 해당 아이템을 추가.
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Item"))
        {
            UIManager.instance.InteractionEnter(INTERACTION.GETITEM);
            Item.Add(other.GetComponent<GetItem>());
        }
    }
    //키를 입력할 때 가끔씩 처리를 못하는 문제가 있어 Update에서 처리함. 키를 입력받지 않아도 되는 코드가 생기면 해당 함수에서 처리할 것.
    /*
    private void OnTriggerStay(Collider other)
    {
        
    }
    */

    //아이템의 충돌범위를 벗어나면 리스트에서 해당 아이템을 제거하고, 리스트에 남은 아이템이 없다면 UI를 제거.
    private void OnTriggerExit(Collider other)
    {
        if(other.tag.Equals("Item"))
        {
            var item = Item.Find(i => i == other.GetComponent<GetItem>());
            Item.Remove(item);
            if(Item.Count == 0) UIManager.instance.InteractionExit();
        }
    }
}
