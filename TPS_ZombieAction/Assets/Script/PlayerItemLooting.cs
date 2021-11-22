using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemLooting : MonoBehaviour
{
    private PlayerInput playerInput;
    private AudioSource playerAudioPlayer;
    private List<GetItem> Item;
    [SerializeField] private AudioClip itemPickUp;//������ �ֿ� �� ���� �Ҹ�


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAudioPlayer = GetComponent<AudioSource>();
        Item = new List<GetItem>();
    }

    private void Update()
    {
        //��ȣ�ۿ�Ű(EŰ)�� ������ �� ����Ʈ�� ��Ƶ� �������� ���� ���, �ش� �������� �ֿ��� �κ��丮�� ���� ��, ����Ʈ���� �����ϰ� ȿ���� ���. �������� ����Ʈ�� ���� �������� ���ٸ� UI����.
        if(playerInput.playerInteraction)
        {
            if (Item.Count != 0)
            {
                var item = Item.Find(i => i);
                item.Looting();
                Item.Remove(item);
                playerAudioPlayer.PlayOneShot(itemPickUp);//������ �ݴ� ���� ���
                if(Item.Count == 0) UIManager.instance.InteractionExit();
            }
        }
    }
    //�����۰� �浹�ϸ� UI�� ���� ����Ʈ�� �ش� �������� �߰�.
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Item"))
        {
            UIManager.instance.InteractionEnter(INTERACTION.GETITEM);
            Item.Add(other.GetComponent<GetItem>());
        }
    }
    //Ű�� �Է��� �� ������ ó���� ���ϴ� ������ �־� Update���� ó����. Ű�� �Է¹��� �ʾƵ� �Ǵ� �ڵ尡 ����� �ش� �Լ����� ó���� ��.
    /*
    private void OnTriggerStay(Collider other)
    {
        
    }
    */

    //�������� �浹������ ����� ����Ʈ���� �ش� �������� �����ϰ�, ����Ʈ�� ���� �������� ���ٸ� UI�� ����.
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
