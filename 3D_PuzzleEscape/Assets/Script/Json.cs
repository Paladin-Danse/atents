using System;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

/// <summary>
/// Newtonsoft JSON Library가 없다면 https://github.com/JamesNK/Newtonsoft.Json/releases 에서 다운받아서 추가.<br/>
/// Unity에 기본적으로 Newtonsoft JSON Library가 포함되어 있다.
/// </summary>
public class Json : MonoBehaviour
{
    //[SerializeField] private SaveData savedata;
    //[SerializeField] private SaveData loaddata;
    private string Json_Data;
    private string savePath = "/Resource/Save/SaveData.json";

    public bool SaveFile(SaveData newSave)
    {
        Json_Data = SerializeObject(newSave);
        if(Json_Data != null) File.WriteAllText(Application.dataPath + savePath, Json_Data);
        else
        {
            Debug.LogError("Json(SaveFile) Error : Json_Data is Not Found");
            return false;
        }
        return true;
    }

    public SaveData LoadFile()
    {
        SaveData loaddata;
        Json_Data = File.ReadAllText(Application.dataPath + savePath);
        if(Json_Data != null) loaddata = DeserializeObject<SaveData>(Json_Data);
        else
        {
            Debug.LogError("Json(LoadFile) Error : Json_Data is Not Found");
            return null;
        }
        return loaddata;
    }

    public void Debug_JsonData()
    {
        Debug.Log(Json_Data);
    }

    /// <summary>
    /// T는 반드시 직열화([SerializeField] or [System.Serializable]) 되어야 한다.<br/>
    /// ex) [SerializeField] class A{} T=A or T=ListA... OK<br/>
    /// Json 검사기 : https://kr.piliapp.com/json/validator/
    /// </summary>
    public static T DeserializeObject<T>(string data)
    {
        if (string.IsNullOrEmpty(data)) throw new ArgumentNullException("name");

        // name에서 확장자를 제외.
        //name = System.IO.Path.GetFileNameWithoutExtension(name);
        //var jsonData = Resources.Load<TextAsset>(name);
        //if (data == null) throw new Exception(string.Format("Not Find TextAsset : {0}", data));

        // Json 파일을 역직열화하여 T 형식으로 반환.
        return JsonConvert.DeserializeObject<T>(data);
    }

    public static string SerializeObject(object data)
    {
        if (null == data) throw new NullReferenceException("data");

        // data를 직열화.
        return JsonConvert.SerializeObject(data);
    }
}
