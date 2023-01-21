using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneController : WindowController
{
    public static PhoneController instance;
    public static bool isOpen {get{return instance.gameObject.activeSelf;}}
    public override void Activate()
    {
        instance=this;
    }
}
