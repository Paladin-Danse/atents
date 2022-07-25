using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Into_the_Portal : InteractionObject
{
    [SerializeField] private GameObject GameEndingUI;

    // Start is called before the first frame update
    protected void Awake()
    {
        GameEndingUI = UIManager.instance.transform.Find("GameEndingUI").gameObject;
    }
    protected new void Start()
    {
        base.Start();
        if (GameEndingUI) GameEndingUI.SetActive(false);
        else Debug.Log("Error(Into_the_Portal) : GameEndingUI is Not Found!");
        InteractionEvent += Ending;
    }

    public void Ending()
    {
        GameEndingUI.SetActive(true);
        GameManager.instance.SetActivePlayer(false);
        GameManager.instance.OnCursorVisible();
    }
}
