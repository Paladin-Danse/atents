using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Into_the_Portal : InteractionObject
{
    [SerializeField] private GameObject GameEndingUI;

    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();
        GameEndingUI.SetActive(false);
        InteractionEvent += Ending;
    }

    public void Ending()
    {
        GameEndingUI.SetActive(true);
        GameManager.instance.playerMovement.LockMove();
    }
}
