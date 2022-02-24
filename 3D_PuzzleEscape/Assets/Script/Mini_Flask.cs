using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mini_Flask : MonoBehaviour
{
    [SerializeField] private Flask ScrObjdata;
    private GameObject LiquidObj;
    private Renderer Liquid_Renderer;
    private Dictionary<Material, int> Mixed_Liquid = new Dictionary<Material, int>();
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

        if (Mixed_Liquid.ContainsKey(Liquid_Renderer.material))
        {
            Mixed_Liquid[Liquid_Renderer.material] -= newAmount;
        }
        else Debug.Log("'Mixed_Liauid[Liquid_Renderer.material]' is Not Found!!");

        if (Get_Liquid_Court == null)
        {
            Get_Liquid_Court = Getting_Liquid();
            StartCoroutine(Get_Liquid_Court);
        }
    }
    public void Liquid_FillUp(int newAmount, Dictionary<Material, int> mixture)
    {
        if (Amount >= 1) return;

        if(Amount <= 0)
        {
            LiquidObj.SetActive(true);
        }

        Amount = Mathf.Clamp(Amount + newAmount, 0, Size);

        foreach(Material key in mixture.Keys)
        {
            if (!Mixed_Liquid.ContainsKey(key))
            {
                Mixed_Liquid.Add(key, mixture[key]);
            }
            else
            {
                Mixed_Liquid[key] += mixture[key];
            }
        }

        if (Get_Liquid_Court == null)
        {
            Get_Liquid_Court = Getting_Liquid();
            StartCoroutine(Get_Liquid_Court);
        }
    }

    public void Liquid_Change(Material newLiquid)
    {
        if (Get_Liquid_Court == null)
        {
            Get_Liquid_Court = Getting_Liquid(newLiquid);
            StartCoroutine(Get_Liquid_Court);
        }
    }

    private IEnumerator Getting_Liquid(Material newLiquid)
    {
        float t = 0;
        var oldLiquid = Liquid_Renderer.material;

        while (t <= 1.0f)
        {
            Liquid_Renderer.material.Lerp(oldLiquid, newLiquid, t);
            t += Time.deltaTime;

            yield return null;
        }
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
    public int EmptyAmount()
    {
        return Size - Amount;
    }
    public int GetSize()
    {
        return Size;
    }
    public Dictionary<Material, int> Get_Potion_Mixture()
    {
        return Mixed_Liquid;
    }
    public Material GetLiquid()
    {
        return Liquid_Renderer.material;
    }
}
