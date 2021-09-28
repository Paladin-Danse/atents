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

    //OnTriggerEnter�� ó�� ��������� �ȿ��� ���������� �÷��̾��� �Է�ó���� �޾Ƶ����� ���� Stay���� ó����.
    private void OnTriggerStay(Collider other)
    {
        CItem item = other.GetComponent<CItem>();//�÷��̾�� �ε��� �������� �޾ƿ�.
        if (item != null)
        {
            if (playerInput.playerInteraction)//�÷��̾ ��ȣ�ۿ�Ű(FŰ)�� �������
            {
                item.Loot(gameObject);//�������� Loot�Լ��� ����

                playerAudioPlayer.PlayOneShot(itemPickUp);//������ �ݴ� ���� ���
            }
        }
    }
}
