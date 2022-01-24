using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image item_img { get; private set; }
    public Text item_name { get; private set; }
    public int UI_Num { get; private set; }//UI������ �����ϴ� ��.(�̰ɷ� SelectItem�� ��ġ�� ������)

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

    //UI_Num�� Active�� False�� �� UI������ ������Ʈ���� ��ĭ�� ��ܿͼ� UI_Num �߰��� ��� ���� �����ϱ�.
    public void Pull_Num(int compareNum)
    {
        if (UI_Num > compareNum) UI_Num--;
    }
}
