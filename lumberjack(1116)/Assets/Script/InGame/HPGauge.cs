using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HPGauge : MonoBehaviour {
    public Slider Gauge;

    float HPMax;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void GaugeState(float HP)
    {
        Gauge.value = HP / HPMax;
    }
    public void SetHPMax(float HP)
    {
        HPMax = HP;
    }
}
