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

    private void Awake()
    {
        gameoverUI.SetActive(false);
    }

    public void UpdateAmmoText(int magAmmo, int remainAmmo)
    {
        ammoText.text = string.Format("{0} / {1}", magAmmo, remainAmmo);
    }
    public void UpdateScoreText(int newScore)
    {
        scoreText.text = string.Format("SCORE : {0}", newScore);
    }
    public void UpdateWaveText(int waves, int count)
    {
        waveText.text = string.Format("WAVE : {0}\nENEMY LEFT : {1}", waves, count);
    }
    public void SetActiveGameoverUI(bool active)
    {
        gameoverUI.SetActive(true);
    }

    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
