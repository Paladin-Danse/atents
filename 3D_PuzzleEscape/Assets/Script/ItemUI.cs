using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image item_img { get; private set; }
    public Text item_name { get; private set; }
    public Text item_description { get; private set; }
    private GameObject Description;
    //public int UI_Num { get; private set; }//UI순서를 결정하는 값.(이걸로 SelectItem의 위치를 결정함)

    private void Awake()
    {
        item_img = transform.Find("Image").GetComponent<Image>();
        item_name = transform.Find("Text").GetComponent<Text>();
        Description = transform.Find("Description").gameObject;
        item_description = Description.transform.Find("Text").GetComponent<Text>();
    }

    public void UIUpdate(Sprite newsprite, string newname, string newdescription)
    {
        item_img.sprite = newsprite;
        item_name.text = newname;
        item_description.text = newdescription;
    }

    public void SetDescription(bool setbool)
    {
        Description.SetActive(setbool);
    }
}
