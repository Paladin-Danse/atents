using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    enum GameState
    {
        Stay = 0,
        GameOver,
        Win,
        Play
    }
    GameState GS;
    public GameObject quizManager;
    public GameObject GameOver_UI;
    // Use this for initialization
    void Start () {
        GS = GameState.Play;
        quizManager.SetActive(false);
        GameOver_UI.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        switch (GS)
        {
            case GameState.Stay:
                Time.timeScale = 0;
                break;
            case GameState.GameOver:
                break;
            case GameState.Win:
                break;
            case GameState.Play:
                Time.timeScale = 1;
                break;
        }
            
	}

    public void GameStay()
    {
        GS = GameState.Stay;
    }

    public void GameOver()
    {
        GS = GameState.GameOver;
        GameOver_UI.SetActive(true);

        int Stage = quizManager.GetComponentInChildren<QuizManager>().Stage;
        GetComponent<Medal_Box>().Medal_Set_1(Stage);
        GameOver_UI.GetComponentInChildren<ClearMedal_Text>().Set_Medal(1);
    }
    public void GameWin()
    {
        GS = GameState.Win;
        quizManager.SetActive(true);
    }

    public void GamePlay()
    {
        GS = GameState.Play;
    }
    
    public string GetGameState()
    {
        return GS.ToString();
    }
}
