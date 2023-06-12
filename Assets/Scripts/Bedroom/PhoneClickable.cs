using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneClickable : BedroomObject
{
    public AudioClip pickUp,putDown;
    public AudioSource audioSource;
    public override void Awake(){
        base.Awake();
        audioSource=GetComponent<AudioSource>();
    }
    public override void Activate()
    {
        if (!BedroomController.morning)
            BedClickable.instance.inactive=false;
        PhoneController.instance.Toggle();
        audioSource.clip = PhoneController.isOpen ? pickUp:putDown;
        audioSource.Play();
    }
}
