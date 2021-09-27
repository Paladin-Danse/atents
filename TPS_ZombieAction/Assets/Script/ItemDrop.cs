using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private GameObject[] dropItem;
    [SerializeField] [Range(0, 1)] private float f_DropPercent = 0.25f;


    public void DropItem()
    {
        float Percentage = Random.Range(0f, 1f);
        Debug.Log(Percentage);

        if (Percentage <= f_DropPercent)
        {
            Instantiate(dropItem[Random.Range(0, dropItem.Length)], transform.position, Quaternion.identity);
        }
    }
}
