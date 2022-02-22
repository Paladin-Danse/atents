using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private string HorizontalMoveKeyName = "Horizontal";
    private string VerticalMoveKeyName = "Vertical";
    private string Mini_LeftKeyName = "Mini_Left";
    private string Mini_RightKeyName = "Mini_Right";
    private string Mini_UpKeyName = "Mini_Up";
    private string Mini_DownKeyName = "Mini_Down";
    private string RotateXKeyName = "Mouse X";
    private string RotateYKeyName = "Mouse Y";
    private string InteractionKeyName = "Fire1";
    private string MixKeyName = "Mix";
    private string ItemSelectKeyName = "Mouse ScrollWheel";
    private string ItemDescriptionKeyName = "Description";
    private string ActionCancelKeyName = "Cancel";
    private string CrouchKeyName = "Crouch";
    private string Mini_Num1KeyName = "Mini_1";
    private string Mini_Num2KeyName = "Mini_2";
    private string Mini_Num3KeyName = "Mini_3";
    public float HorizontalMoveKey { get; private set; }
    public float VerticalMoveKey { get; private set; }
    public bool Mini_LeftKey { get; private set; }
    public bool Mini_RightKey { get; private set; }
    public bool Mini_UpKey { get; private set; }
    public bool Mini_DownKey { get; private set; }
    public float RotateXKey { get; private set; }
    public float RotateYKey { get; private set; }
    public bool InteractionKey { get; private set; }
    public bool MixKey { get; private set; }
    public float ItemSelectKey { get; private set; }
    public bool ItemDescriptionKey { get; private set; }
    public bool ActionCancelKey { get; private set; }
    public bool CrouchKey { get; private set; }
    public bool Mini_Num1Key { get; private set; }
    public bool Mini_Num2Key { get; private set; }
    public bool Mini_Num3Key { get; private set; }
    private void Update()
    {
        HorizontalMoveKey = Input.GetAxis(HorizontalMoveKeyName);
        VerticalMoveKey = Input.GetAxis(VerticalMoveKeyName);
        Mini_LeftKey = Input.GetButtonDown(Mini_LeftKeyName);
        Mini_RightKey = Input.GetButtonDown(Mini_RightKeyName);
        Mini_UpKey = Input.GetButtonDown(Mini_UpKeyName);
        Mini_DownKey = Input.GetButtonDown(Mini_DownKeyName);
        RotateXKey = Input.GetAxis(RotateXKeyName);
        RotateYKey = Input.GetAxis(RotateYKeyName);
        InteractionKey = Input.GetButtonDown(InteractionKeyName);
        MixKey = Input.GetButtonDown(MixKeyName);
        ItemSelectKey = Input.GetAxisRaw(ItemSelectKeyName);
        ItemDescriptionKey = Input.GetButtonDown(ItemDescriptionKeyName);
        ActionCancelKey = Input.GetButtonDown(ActionCancelKeyName);
        CrouchKey = Input.GetButtonDown(CrouchKeyName);
        Mini_Num1Key = Input.GetButtonUp(Mini_Num1KeyName);
        Mini_Num2Key = Input.GetButtonUp(Mini_Num2KeyName);
        Mini_Num3Key = Input.GetButtonUp(Mini_Num3KeyName);
    }
}
