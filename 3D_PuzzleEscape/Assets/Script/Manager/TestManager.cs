using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TestManager : MonoBehaviour
{
    [SerializeField] private Json json_Save;

    void Start()
    {
    }

    public void SaveButton()
    {
        //json_Save.SaveFile();
        json_Save.Debug_JsonData();
    }

    public void LoadButton()
    {
        json_Save.LoadFile();
        json_Save.Debug_JsonData();
    }
}
