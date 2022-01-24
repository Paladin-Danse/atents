using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image item_img { get; private set; }
    public Text item_name { get; private set; }
    public int UI_Num { get; private set; }//UI순서를 결정하는 값.(이걸로 SelectItem의 위치를 결정함)

    private void Awake()
    {
        item_img = transform.Find("Image").GetComponent<Image>();
        item_name = transform.Find("Text").GetComponent<Text>();
    }

    public void UIUpdate(Sprite newsprite, string newname, int num)
    {
        item_img.sprite = newsprite;
        item_name.text = newname;
        UI_Num = num;
    }

    //UI_Num을 Active가 False가 된 UI이후의 오브젝트부터 한칸씩 당겨와서 UI_Num 중간에 비는 값이 없게하기.
    public void Pull_Num(int compareNum)
    {
        if (UI_Num > compareNum) UI_Num--;
    }
}
