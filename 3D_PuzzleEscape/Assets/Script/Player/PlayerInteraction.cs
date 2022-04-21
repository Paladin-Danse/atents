using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerInput playerInput;

    //������ ��ũ��Ʈ�� ����� �÷��̾ ���� ��� �ִ� �����۰��� ����.
    //private Item InteractionItem;
    [SerializeField] private float f_InteractionRayDistance;
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
        InteractableLayer = (1 << LayerMask.NameToLayer("Interactable")) + (1 << LayerMask.NameToLayer("Default"));
        b_OnInteraction = true;
    }

    void Update()
    {
        if (b_OnInteraction)
        {
            InteractionRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, f_InteractionRayDistance));

            if (Physics.Raycast(InteractionRay, out hit, f_InteractionRayDistance, InteractableLayer))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Default"))
                {
                    UIManager.instance.OffInteractionUI();
                    return;
                }

                UIManager.instance.OnInteractionUI();
                
                if(playerInput.InteractionKey)
                {
                    InteractionObject[] interObjs = hit.collider.GetComponents<InteractionObject>();
                    InteractionObject interObj = null;

                    foreach(InteractionObject iter in interObjs)
                    {
                        if (iter.enabled == true) interObj = iter;
                    }

                    if (interObj == null) return;

                    if(interObj)
                    {
                        if (interObj.NeedItemCheck() != null && InventoryManager.instance.SelectedItem != null)
                        {
                            if(InventoryManager.instance.SelectedItem.data.name == interObj.NeedItemCheck().Data.name)
                            {
                                InventoryManager.instance.UseItem(InventoryManager.instance.SelectedItem);
                                interObj.Interaction();
                            }
                            else
                            {
                                //�޼��� UI�� Active���Ѽ� "�� �������� ���⿡ ����� �� ����."��� ����ֱ�.
                            }
                        }
                        else if (interObj.NeedItemCheck() == null)
                        {
                            interObj.Interaction();
                        }
                    }
                }
            }
            else
            {
                UIManager.instance.OffInteractionUI();
            }
        }
    }

    public void LockInteraction()
    {
        b_OnInteraction = false;
    }
    public void UnlockInteraction()
    {
        b_OnInteraction = true;
    }
}
