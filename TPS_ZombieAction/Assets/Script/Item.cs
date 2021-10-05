using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�����۷� �������̽�
public abstract class CItem : MonoBehaviour
{
    [SerializeField] protected Sprite m_InventoryImage;
    public Sprite InventoryImage { get { return m_InventoryImage; } }
    public int Num { get; protected set; }
    public abstract void Loot(GameObject target);
    public abstract void SetPosition(Vector3 pos);
    public void NumUp() { Num++; }
    public void NumDown() { Num--; }
    
    public virtual void Use()
    {
        NumDown();
        UIManager.instance.UpdateInventory(InventoryImage, Num);
    }
}
