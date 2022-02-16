using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MixData", menuName = "ScriptableObjects/MixData", order = 1)]
public class MixData : ScriptableObject
{
    [SerializeField] MixItem data;
    public MixItem Data { get { return data; } }
}