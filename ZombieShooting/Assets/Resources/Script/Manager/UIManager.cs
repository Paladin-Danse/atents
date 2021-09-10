using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    private static UIManager m_instance;
    public static UIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>();
            }
            return m_instance;
        }
    }

    public Text ammoText;
    public Text scoreText;
    public Text waveText;
    public GameObject gameoverUI;


    public void UpdateAmmoText(int magAmmo, int remainAmmo)
    {

    }
    public void UpdateScoreText(int newScore)
    {

    }
    public void UpdateWaveText(int waves, int count)
    {

    }
    public void SetActiveGameoverUI(bool active)
    {

    }

    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
