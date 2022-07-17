using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Medal_Box : MonoBehaviour {
    public GameObject MedalBox;
    public static int[] Medal = new int[4];

	// Use this for initialization
	void Start () {
        for(int i=1; i<=3; i++)
        {
            if(Medal[i] < 0 && Medal[i] > 3)
            {
                Medal[i] = 0;
            }
        }

        if(MedalBox == null)
        {
            Debug.Log("훈장함 비었으니 채우시오.");
        }
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public void Open_Box()
    {
        MedalBox.SetActive(true);
    }

    public void Close_Box()
    {
        MedalBox.SetActive(false);
    }
    public int Get_All_Medal()
    {
        int Total = 0;
        for(int i=1; i<=3; i++)
        {
            Total += Medal[i];
        }

        return Total;
    }
    public void Medal_Set_1(int Stage)
    {
        if(Medal[Stage] < 1) Medal[Stage] = 1;
    }

    public void Medal_Set_2(int Stage)
    {
        if (Medal[Stage] < 2) Medal[Stage] = 2;
    }

    public void Medal_Set_3(int Stage)
    {
        if (Medal[Stage] < 3) Medal[Stage] = 3;
    }

    public int Get_Medal(int Stage)
    {
        return Medal[Stage];
    }

    public int Star_Set(int Stage)
    {
        return Medal[Stage];
    }
}
