using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunCrosshair : MonoBehaviour
{
    [SerializeField] [Range(60, 250)] private float f_Size = 100;
    [SerializeField] private float f_MaxSize = 250;
    [SerializeField] private float f_MinSize = 60;
    [SerializeField] private RectTransform CrosshairUp;
    [SerializeField] private RectTransform CrosshairDown;
    [SerializeField] private RectTransform CrosshairLeft;
    [SerializeField] private RectTransform CrosshairRight;
    [SerializeField] private float Recoil_ReviveValue = 3f;
    [SerializeField] private float Recoil_Time = 0.5f;
    private float Recoil_LastTime;
    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        Recoil_LastTime = Time.time;
    }

    private void OnEnable()
    {
        f_Size = f_MinSize;
    }

    private void FixedUpdate()
    {
        if (Time.time >= Recoil_LastTime + Recoil_Time)
        {
            CrosshairUpdate();
        }
    }

    private void CrosshairUpdate()
    {
        if (f_MinSize < f_Size)
        {
            f_Size -= Recoil_ReviveValue;
            if (f_Size > 150) f_Size -= Recoil_ReviveValue;
            CrosshairSizeUpdate();
        }
        else
        {
            f_Size = f_MinSize;
            CrosshairSizeUpdate();
        }
    }

    private void CrosshairSizeUpdate()
    {
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, f_Size);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, f_Size);
        /*
        CrosshairUp.localPosition = new Vector3(CrosshairUp.localPosition.x, f_Size - 45f, CrosshairUp.localPosition.z);
        CrosshairDown.localPosition = new Vector3(CrosshairDown.localPosition.x, f_Size - 75f, CrosshairDown.localPosition.z);
        CrosshairLeft.localPosition = new Vector3(f_Size - 75f, CrosshairLeft.localPosition.y, CrosshairLeft.localPosition.z);
        CrosshairRight.localPosition = new Vector3(f_Size - 45f, CrosshairRight.localPosition.y, CrosshairRight.localPosition.z);
        */
    }
    public void CrosshairDivide(float AppendValue)
    {
        f_Size += AppendValue;
        if (f_MaxSize < f_Size) f_Size = f_MaxSize;
        Recoil_LastTime = Time.time;
        CrosshairSizeUpdate();
        /*
        CrosshairUp.localPosition = new Vector3(CrosshairUp.localPosition.x, CrosshairUp.localPosition.y + AppendValue, CrosshairUp.localPosition.z);
        CrosshairDown.localPosition = new Vector3(CrosshairDown.localPosition.x, CrosshairDown.localPosition.y - AppendValue, CrosshairDown.localPosition.z);
        CrosshairLeft.localPosition = new Vector3(CrosshairLeft.localPosition.x - AppendValue, CrosshairLeft.localPosition.y, CrosshairLeft.localPosition.z);
        CrosshairRight.localPosition = new Vector3(CrosshairRight.localPosition.x + AppendValue, CrosshairRight.localPosition.y, CrosshairRight.localPosition.z);
        */
    }

    public float GetSize()
    {
        return f_Size;
    }

    public void GunAccuracyToSize(float Accuracy)
    {
        f_MinSize = 60 + (190 * (1.0f - (Accuracy * 0.01f)));
    }
}
