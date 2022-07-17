using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class QuizManager : MonoBehaviour {
    enum TestButton
    {
        One,
        Two,
        Three,
        Four
    }
    //Scene_Move SM;
    Medal_Box MB;
    StageManager stageManager;

    public Sprite[] Quiz_bundle;
    //public int[] Quiz_Used = { 0, };
    public int[] answer_bundle;
    public int RdomRng;
    
    public int Stage;
    public GameObject Clear_UI;

    bool On_Button_Pressed = false;
    //public GameObject Canvas;

    //public AudioClip Enemy_Attack_Sound;
    //public AudioClip Player_Damage_Sound;
    


    // Use this for initialization
    void Start () {
        //SM = GameObject.Find("GameManager").GetComponent<Scene_Move>();
        MB = GameObject.Find("GameManager").GetComponent<Medal_Box>();
        stageManager = GameObject.Find("GameManager").GetComponent<StageManager>();

        Clear_UI.SetActive(false);
        RdomRng = Random.Range(0, Quiz_bundle.Length);
        GetComponent<Image>().sprite = Quiz_bundle[RdomRng];

        
        /*퀴즈 만들기
        GameObject Quiz = Instantiate(Quiz_bundle[RdomRng], gameObject.transform)as GameObject;
        Quiz.GetComponent<RectTransform>().offsetMin = new Vector2(60, 60);
        Quiz.GetComponent<RectTransform>().offsetMax = new Vector2(60, 60);
        */
    }
	
	// Update is called once per frame
	void Update () {
        
    }
    
    void answer()
    {
        //SoundManager.instance.PlaySingle(Enemy_Attack_Sound);
        
        Debug.Log("Answer");
        On_Button_Pressed = true;
        MB.Medal_Set_3(Stage);
        stageManager.CurrentStage_Up(Stage);

        Set_Clear_UI(3);
        gameObject.SetActive(false);
        
        //Shuffle_Quiz();
        //Quiz_Used[RdomRng] = 1;
    }

    void Incorrect()
    {
        //SoundManager.instance.PlaySingle(Player_Damage_Sound);
        
        Debug.Log("Incorrect");
        On_Button_Pressed = true;
        MB.Medal_Set_2(Stage);
        stageManager.CurrentStage_Up(Stage);

        Set_Clear_UI(2);
        gameObject.SetActive(false);
        //Shuffle_Quiz();
        //Quiz_Used[RdomRng] = 1;
    }

    public void Quiz_Determine(int ButtonNumber)
    {
        if (!On_Button_Pressed)
        {
            if (answer_bundle[RdomRng] == ButtonNumber)
            {
                answer();
            }
            else
            {
                Incorrect();
            }
        }
    }

    void Set_Clear_UI(int Medal)
    {
        Clear_UI.SetActive(true);
        ClearMedal_Text CMT = Clear_UI.GetComponentInChildren<ClearMedal_Text>();
        CMT.Set_Medal(Medal);
    }

    
    /*
    void Shuffle_Quiz()
    {
        do
        {
            RdomRng = Random.Range(0, Quiz_bundle.Length);
        } while (Quiz_Used[RdomRng] == 1);
    }
    */
}
