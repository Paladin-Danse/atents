using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mannequin_GetParts : ItemInteraction
{
    private AudioSource InteractSound;
    private AudioClip pullout_Clip;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        InteractionEvent += Manager_MannequinData_Delete;
        InteractSound = transform.parent.parent.GetComponent<AudioSource>();
    }

    public void SetClip(AudioClip newClip)
    {
        pullout_Clip = newClip;
    }

    public void Manager_MannequinData_Delete()
    {
        if(pullout_Clip != null)
        {
            InteractSound.clip = pullout_Clip;
            InteractSound.Play();
        }
        GameManager.instance.M_InteractableData_Delete(item);
    }
}