using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//아이템류 인터페이스
public abstract class CItem : MonoBehaviour
{
    [SerializeField] private Image InventoryImage;
    private int Num;
    public abstract void Loot(GameObject target);
    public abstract void SetPosition(Vector3 pos);
    public abstract CItem NewItem();
}
