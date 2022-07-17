using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Rabbit_Sergeant_Animation : MonoBehaviour {
    Animator Rabbit_Anim;

    bool OnEyeClose = true;
    public float EyeClose_ReturnTime;

    public GameObject Rabbit_Talk;
	// Use this for initialization
	void Start () {
        Rabbit_Anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<Image>().sprite = GetComponent<SpriteRenderer>().sprite;

        if (OnEyeClose)
        {
            StartCoroutine("Rabbit_Eye_Close");
        }
	}

    IEnumerator Rabbit_Eye_Close()
    {

        OnEyeClose = false;
        yield return new WaitForSeconds(EyeClose_ReturnTime);
        Rabbit_Anim.SetTrigger("Eye_Close");
        OnEyeClose = true;

    }
    public void On_Rabbit_Talk()
    {
        Rabbit_Talk.SetActive(true);
    }
}
