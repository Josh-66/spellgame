using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookshelfClickable : BedroomObject
{
    public override void Awake(){
        base.Awake();
    }
    public override void Activate()
    {
        BookController.ToggleBook();
    }
}
