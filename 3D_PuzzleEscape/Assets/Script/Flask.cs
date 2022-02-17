using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct FlaskData
{
    public int Flask_Size;
    public int Liquid_Amount;
    public Material Liquid;
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Flask", order = 1)]
public class Flask : ScriptableObject
{
    [SerializeField] FlaskData data;
    public FlaskData Data { get { return data; } }
}
