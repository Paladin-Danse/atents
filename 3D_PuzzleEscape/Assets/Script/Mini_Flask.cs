using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mini_Flask : MonoBehaviour
{
    [SerializeField] private Flask ScrObjdata;
    private GameObject LiquidObj;
    private Renderer Liquid_Renderer;
    private Dictionary<Material, float> Mixed_Liquid = new Dictionary<Material, float>();
    private int Size;
    private float Amount;
    private IEnumerator Get_Liquid_Court = null;
    private IEnumerator Get_Amount_Court = null;
    
    private void Awake()
    {
        LiquidObj = transform.Find("F_Liquid_03").gameObject;
        Liquid_Renderer = LiquidObj.GetComponent<Renderer>();
        Size = ScrObjdata.Data.Flask_Size;
        Amount = ScrObjdata.Data.Liquid_Amount;
    }

    public Dictionary<Material, float> Liquid_Drain(float newAmount)
    {
        if (Amount <= 0) return null;

        Dictionary<Material, float> DrainLiquid = new Dictionary<Material, float>();
        float temp = (Amount - newAmount) / Amount;

        Amount = Mathf.Clamp(Amount - newAmount, 0, Size);

        if(Amount > 0)
        {
            List<Material> keys = new List<Material>(Mixed_Liquid.Keys);

            foreach(Material iter in keys)
            {
                float tempAmount = Mixed_Liquid[iter] * temp;
                float DrainAmount = Mixed_Liquid[iter] * (1 - temp);

                Mixed_Liquid[iter] = tempAmount;
                DrainLiquid.Add(iter, DrainAmount);
                Debug.Log(gameObject.name + " : (" + iter + ", " + Mixed_Liquid[iter] + ")");
            }
        }
        else
        {
            DrainLiquid = Mixed_Liquid;
            Mixed_Liquid.Clear();
        }

        if (Get_Amount_Court == null)
        {
            Get_Amount_Court = Getting_Liquid();
            StartCoroutine(Get_Amount_Court);
        }
        return DrainLiquid;
    }
    public void Liquid_FillUp(float newAmount, Dictionary<Material, float> mixture)
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

            Debug.Log(gameObject.name + " : (" + key + ", " + Mixed_Liquid[key] + ")");
        }

        if (Get_Amount_Court == null)
        {
            Get_Amount_Court = Getting_Liquid();
            StartCoroutine(Get_Amount_Court);
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
        Get_Amount_Court = null;
    }
    public float EmptyAmount()
    {
        return (float)Size - Amount;
    }
    public int GetSize()
    {
        return Size;
    }
    public Dictionary<Material, float> Get_Potion_Mixture()
    {
        return Mixed_Liquid;
    }
    public Material GetLiquid()
    {
        return Liquid_Renderer.material;
    }
}
