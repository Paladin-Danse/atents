using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Button : MonoBehaviour {
    public QuizManager QM;
    Button Btn;
    public int ButtonNum;

	// Use this for initialization
	void Start () {
        
        QM = GameObject.Find("Quiz").GetComponent<QuizManager>();
        Btn = GetComponent<Button>();

    }
	
	// Update is called once per frame
	void Update () {
	    
	}
    
    void OnMouseDown()
    {
        QM.Quiz_Determine(ButtonNum);
    }
}
