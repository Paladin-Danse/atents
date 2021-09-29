using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//아이템류 인터페이스
public abstract class CItem : MonoBehaviour
{
    public abstract void Loot(GameObject target);
    public abstract void SetPosition(Vector3 pos);
    public abstract CItem NewItem();
}
