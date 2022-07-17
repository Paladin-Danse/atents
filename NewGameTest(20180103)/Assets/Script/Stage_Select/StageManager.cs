using UnityEngine;
using System.Collections;

public class StageManager : MonoBehaviour {
    static int CurrentStage = 1;
    
    
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

    public int GetCurrentStage()
    {
        return CurrentStage;
    }

    public void CurrentStage_Up(int Stage)
    {
        if(CurrentStage <= Stage)
        {
            CurrentStage = Stage + 1;
        }
    }
}
