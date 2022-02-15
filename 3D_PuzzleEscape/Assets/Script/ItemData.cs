using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemData", order = 1)]
public class ItemData : ScriptableObject
{
    [SerializeField] Item data;
    public Item Data { get { return data; } }
}
[CreateAssetMenu(fileName = "MixData", menuName = "ScriptableObjects/MixItemData", order = 2)]
public class MixItemData : ScriptableObject
{
    [SerializeField] MixItem data;
    public MixItem Data { get { return data; } }
}
