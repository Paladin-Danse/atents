using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GameManager : MonoBehaviour {
    enum GameState
    {
        Play = 0,
        Option,
        MonsterBattle,
        GameWin,
        GameOver
    }
    GameState GS;

    AudioSource AudioManager;
    public AudioClip Play_Sound;
    public AudioClip Battle_Sound;
    public AudioClip Clear_Sound;

    public GameObject Option_UI;
    public GameObject GameOver_UI;
    public GameObject GameWin_UI;
	// Use this for initialization
	void Start () {
        AudioManager = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        GameOver_UI.SetActive(false);
        Option_UI.SetActive(false);
        GS = GameState.Play;
	}
	
	// Update is called once per frame
	void Update () {
        switch(GS)
        {
            case GameState.Play:
                Time.timeScale = 1;
                if (Option_UI.active) Option_UI.SetActive(false);
                if (GameOver_UI.active) GameOver_UI.SetActive(false);
                break;
            case GameState.Option:
                Time.timeScale = 0;
                if (!Option_UI.active)
                {
                    Option_UI.SetActive(true);
                }
                break;
            case GameState.MonsterBattle:
                Time.timeScale = 1;
                if (Option_UI.active) Option_UI.SetActive(false);
                if (GameOver_UI.active) GameOver_UI.SetActive(false);
                break;
            case GameState.GameWin:
                Time.timeScale = 0;
                if (!GameWin_UI.active)
                {
                    GameWin_UI.SetActive(true);
                }
                break;
            case GameState.GameOver:
                Time.timeScale = 0;
                if (!GameOver_UI.active)
                {
                    GameOver_UI.SetActive(true);
                }
                break;
        }
	}

    public void GamePlay()
    {
        GS = GameState.Play;
    }
    public void On_MonsterBattle()
    {
        GS = GameState.MonsterBattle;
        AudioManager.clip = Battle_Sound;
        AudioManager.Play();
    }
    public void GameOver()
    {
        GS = GameState.GameOver;
    }
    public void GameWin()
    {
        GS = GameState.GameWin;
    }

	public string GetGameState()
	{
		return GS.ToString();
	}

    public void Continue()
    {
        Application.LoadLevel("InGame");
    }

    public void Go_Main()
    {
        Application.LoadLevel("Main");
    }
    public void On_Option()
    {
        GS = GameState.Option;
    }

    public void On_PlaySound()
    {
        AudioManager.clip = Play_Sound;
        AudioManager.loop = true;
        AudioManager.Play();
    }
    public void On_ClearSound()
    {
        AudioManager.clip = Clear_Sound;
        AudioManager.loop = false;
        AudioManager.Play();
    }
}
