using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneEvent : MonoBehaviour
{
    void Start()
    {
        GameManager.instance.GameStart();
    }
}
