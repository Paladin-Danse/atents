using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemLooting : MonoBehaviour
{
    private PlayerInput playerInput;
    private AudioSource playerAudioPlayer;
    [SerializeField] private AudioClip itemPickUp;//������ �ֿ� �� ���� �Ҹ�


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
    //OnTriggerEnter�� ó�� ��������� �ȿ��� ���������� �÷��̾��� �Է�ó���� �޾Ƶ����� ���� Stay���� ó����.
    private void OnTriggerStay(Collider other)
    {
        if(other.tag.Equals("Item"))
        {
            if (playerInput.playerInteraction)//�÷��̾ ��ȣ�ۿ�Ű(EŰ)�� �������
            {
                var item = other.GetComponent<GetItem>();
                if(item)
                {
                    item.Looting();
                    playerAudioPlayer.PlayOneShot(itemPickUp);//������ �ݴ� ���� ���
                    UIManager.instance.InteractionExit();
                }
            }
        }
        //CItem item = other.GetComponent<CItem>();//�÷��̾�� �ε��� �������� �޾ƿ�.
        //if (item != null)
        //{
        //    if (playerInput.playerInteraction)//�÷��̾ ��ȣ�ۿ�Ű(EŰ)�� �������
        //    {
        //        item.Loot(gameObject);//�������� Loot�Լ��� ����
        //
        //        playerAudioPlayer.PlayOneShot(itemPickUp);//������ �ݴ� ���� ���
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
