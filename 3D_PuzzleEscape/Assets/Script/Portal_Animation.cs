using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal_Animation : MonoBehaviour
{
    private Material portal_mat;
    float t = 0.0f;

    private void Start()
    {
        portal_mat = GetComponent<Renderer>().material;
    }
    // Update is called once per frame
    void Update()
    {
        if (portal_mat)
        {
            t = (t + Time.deltaTime) % 1.0f;
            portal_mat.mainTextureOffset = new Vector2(t, t);
        }
    }
}
