using UnityEngine;
using System.Collections;

public class CPManager : MonoBehaviour {
    public float CP_HP;
    public bool EnemyCP;
    Damage_Red_Flash DRF;
    HPGauge HPG;
    // Use this for initialization
    void Start () {
        DRF = GetComponent<Damage_Red_Flash>();

        HPG = GetComponent<HPGauge>();
        HPG.SetHPMax(CP_HP);
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    public void HPLost(float Damage)
    {
        CP_HP -= Damage;
        DRF.Red_Flash();
        HPG.GaugeState(CP_HP);
        if (CP_HP <= 0)
        {
            if (EnemyCP)
            {
                GameObject.Find("GameManager").GetComponent<GameManager>().GameWin();
            }
            else
            {
                GameObject.Find("GameManager").GetComponent<GameManager>().GameOver();
            }
            Destroy(gameObject);
        }
    }
}
