using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerInput playerInput;

    //������ ��ũ��Ʈ�� ����� �÷��̾ ���� ��� �ִ� �����۰��� ����.
    //private Item InteractionItem;
    [SerializeField] private float f_InteractionRayDistance = 5f;
    private Ray InteractionRay;
    private RaycastHit hit;
    private int InteractableLayer;

    private bool b_OnInteraction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void Start()
    {
        InteractableLayer = 1 << LayerMask.NameToLayer("Interactable");
        b_OnInteraction = true;
    }

    void Update()
    {
        if (b_OnInteraction)
        {
            InteractionRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, f_InteractionRayDistance));

            if (Physics.Raycast(InteractionRay, out hit, InteractableLayer))
            {
                //UIManager�� ����� ��ȣ�ۿ�Ű ���̵�UI�� ������ �߰�
                //UIManager.instance.OnInteractionUI();
                
                if(playerInput.InteractionKey)
                {
                    InteractionObject interObj = hit.collider.GetComponent<InteractionObject>();
                    if(interObj)
                    {
                        interObj.Interaction();
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(InteractionRay);
    }
}
