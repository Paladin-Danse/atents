using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note_Interaction : LookInteraction
{
    [SerializeField] private List<GameObject> TextList;
    private GameObject Cur_Text;

    private new void Start()
    {
        base.Start();
        for (int i = 1; ; i++)
        {
            GameObject text = GameObject.Find("Text" + i.ToString());
            if (text != null)
            {
                TextList.Add(text);
                text.SetActive(false);
            }
            else
            {
                break;
            }
        }
        Cur_Text = TextList[0];
        if (Cur_Text)
        {
            Cur_Text.SetActive(true);
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("Note_Interaction Error : Cur_Text is Not Found!");
#endif
        }
    }

    private new void Update()
    {
        base.Update();
        if(playerInput)
        {
            if(b_Lookobj && (playerInput.Mini_LeftKey || playerInput.Mini_RightKey))
            {
                Cur_Text.SetActive(false);

                int ArrowValue = playerInput.Mini_LeftKey == true ? -1 : playerInput.Mini_RightKey == true ? 1 : 0; //좌우키 입력값. 좌키 : -1 우키 : +1
                int curIndex = Mathf.Clamp(TextList.FindIndex(i => i == Cur_Text) + ArrowValue, 0, TextList.Count - 1); //현재 인덱스번호. 현재 텍스트 넘버 + 좌우키 입력값
                Cur_Text = TextList[curIndex];
                
                Cur_Text.SetActive(true);
            }
        }
    }
}
