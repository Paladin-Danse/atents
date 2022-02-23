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
    private IEnumerator Get_Liquid_Court = null;
    
    private void Awake()
    {
        LiquidObj = transform.Find("F_Liquid_03").gameObject;
        Liquid_Renderer = LiquidObj.GetComponent<Renderer>();
        Size = ScrObjdata.Data.Flask_Size;
        Amount = ScrObjdata.Data.Liquid_Amount;
    }

    public void Liquid_Drain(int newAmount)
    {
        if (Amount <= 0) return;

        Amount = Mathf.Clamp(Amount - newAmount, 0, Size);

        if (Get_Liquid_Court == null)
        {
            Get_Liquid_Court = Getting_Liquid();
            StartCoroutine(Get_Liquid_Court);
        }
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

        if (Get_Liquid_Court == null)
        {
            Get_Liquid_Court = Getting_Liquid(newLiquid);
            StartCoroutine("Getting_Liquid", newLiquid);
        }
    }

    private IEnumerator Getting_Liquid(Material newLiquid)
    {
        float t = 0;
        var oldLiquid = Liquid_Renderer.material;
        var oldScale = LiquidObj.transform.localScale;
        var newScale = new Vector3(LiquidObj.transform.localScale.x, ((float)Amount / (float)Size), LiquidObj.transform.localScale.z);

        while (t <= 1.0f)
        {
            Liquid_Renderer.material.Lerp(oldLiquid, newLiquid, t);
            LiquidObj.transform.localScale = Vector3.Lerp(oldScale, newScale, t);
            t += Time.deltaTime;

            yield return null;
        }
        if (Amount <= 0) LiquidObj.SetActive(false);
        Get_Liquid_Court = null;
    }

    private IEnumerator Getting_Liquid()
    {
        float t = 0;
        var oldScale = LiquidObj.transform.localScale;
        var newScale = new Vector3(LiquidObj.transform.localScale.x, ((float)Amount / (float)Size), LiquidObj.transform.localScale.z);

        while (t <= 1.0f)
        {
            LiquidObj.transform.localScale = Vector3.Lerp(oldScale, newScale, t);
            t += Time.deltaTime;

            yield return null;
        }
        if (Amount <= 0) LiquidObj.SetActive(false);
        Get_Liquid_Court = null;
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
