using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mini_Flask : MonoBehaviour
{
    [SerializeField] private Flask ScrObjdata;
    private GameObject LiquidObj;
    private Renderer Liquid_Renderer;
    private int Size;
    private int Amount;
    
    private void Awake()
    {
        LiquidObj = transform.Find("F_Liquid_03").gameObject;
        Liquid_Renderer = LiquidObj.GetComponent<Renderer>();
        Size = ScrObjdata.Data.Flask_Size;
        Amount = ScrObjdata.Data.Liquid_Amount;
    }

    public void Liquid_Change(Material newLiquid, int newAmount)
    {
        if(Amount == 0)
        {
            LiquidObj.SetActive(true);
            Liquid_Renderer.material = newLiquid;
        }
        else if(Amount == 1)
        {
            return;
        }
        
        Amount = Mathf.Clamp(Amount + newAmount, 0, Size);
        
        StartCoroutine("Getting_Liquid", newLiquid);
    }

    private IEnumerator Getting_Liquid(Material newLiquid)
    {
        float t = 0;
        var oldLiquid = Liquid_Renderer.material;
        var oldScale = LiquidObj.transform.localScale;
        var newScale = new Vector3(LiquidObj.transform.localScale.x, Amount / Size, LiquidObj.transform.localScale.z);

        while (t <= 1.0f)
        {
            Liquid_Renderer.material.Lerp(oldLiquid, newLiquid, t);
            LiquidObj.transform.localScale = Vector3.Lerp(oldScale, newScale, t);
            t += Time.deltaTime;

            yield return null;
        }
    }
    public int EmptyAmountCheck()
    {
        return Size - Amount;
    }
    public int GetSize()
    {
        return Size;
    }
}
