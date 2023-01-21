using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneClickable : BedroomObject
{
    public override void Activate()
    {
        PhoneController.instance.Toggle();
    }
}
