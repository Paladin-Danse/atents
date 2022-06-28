using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mannequin_GetParts : ItemInteraction
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        InteractionEvent += Manager_MannequinData_Delete;
    }

    public void Manager_MannequinData_Delete()
    {
        GameManager.instance.M_InteractableData_Delete(item);
    }

}
