using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookClickable : DeskObject
{



    public override void Activate()
    {
        BookController.ToggleBook();
    }

}
