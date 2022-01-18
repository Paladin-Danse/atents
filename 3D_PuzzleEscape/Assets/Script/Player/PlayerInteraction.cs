using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerInput playerInput;

    //아이템 스크립트를 만들면 플레이어가 현재 들고 있는 아이템값을 가짐.
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
                //UIManager를 만들고 상호작용키 가이드UI를 만들경우 추가
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
