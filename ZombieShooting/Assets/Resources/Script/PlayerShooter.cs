using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    Gun gun;
    public Transform gunPivot;
    public Transform leftHandleMount;
    public Transform rightHandleMount;

    private PlayerInput playerInput;
    private Animator playerAnimator;
    // Start is called before the first frame update
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        gun.gameObject.SetActive(true);
    }
    private void OnDisable()
    {
        gun.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(playerInput.fire)
        {
            gun.Fire();
        }
        else if(playerInput.reload)
        {
            if(gun.Reload())
            {
                playerAnimator.SetTrigger("Reload");
            }
        }
        UpdateUI();
    }
    private void UpdateUI()
    {

    }
}
