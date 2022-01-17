using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private string HorizontalMoveKeyName = "Horizontal";
    private string VerticalMoveKeyName = "Vertical";
    private string RotateXKeyName = "Mouse X";
    private string RotateYKeyName = "Mouse Y";

    public float HorizontalMoveKey { get; private set; }
    public float VerticalMoveKey { get; private set; }
    public float RotateXKey { get; private set; }
    public float RotateYKey { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Update()
    {
        HorizontalMoveKey = Input.GetAxis(HorizontalMoveKeyName);
        VerticalMoveKey = Input.GetAxis(VerticalMoveKeyName);
        RotateXKey = Input.GetAxis(RotateXKeyName);
        RotateYKey = Input.GetAxis(RotateYKeyName);
    }
}
