using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingAnim : MonoBehaviour
{
    public float speed;

    float t;
    private RectTransform rectTF;
    // Start is called before the first frame update
    private void Awake()
    {
        rectTF = gameObject.GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        rectTF.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        t += speed * Time.deltaTime;
        rectTF.rotation = Quaternion.Euler(new Vector3(0, 0, t % 360));
    }
}
