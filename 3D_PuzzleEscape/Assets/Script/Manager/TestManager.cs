using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TestManager : MonoBehaviour
{
    [SerializeField] private SaveData savedata;
    [SerializeField] private SaveData loaddata;
    private string Json_Data;
    // Start is called before the first frame update
    void Start()
    {
        Json_Data = ObjectToJson(savedata);
        Debug.Log(Json_Data);
    }

    public void SaveFile()
    {
        string path = Path.Combine(Application.dataPath + "/SaveData.json");
        File.WriteAllText(path, Json_Data);
    }

    public void LoadFile()
    {
        string GetJson = File.ReadAllText(Application.dataPath + "/Save/SaveData.json");
        loaddata = JsonToObject<SaveData>(GetJson);
    }

    string ObjectToJson(object obj)
    {
        return JsonUtility.ToJson(obj, true);
    }

    T JsonToObject<T>(string JsonData) where T : class
    {
        return JsonUtility.FromJson<T>(JsonData);
    }
}
