using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image item_img { get; private set; }
    public Text item_name { get; private set; }

    private void Awake()
    {
        item_img = transform.Find("Image").GetComponent<Image>();
        item_name = transform.Find("Text").GetComponent<Text>();
    }

    public void UIUpdate(Sprite newsprite, string newname)
    {
        item_img.sprite = newsprite;
        item_name.text = newname;
    }
}
