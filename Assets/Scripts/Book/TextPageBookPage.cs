using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "TextPage", menuName = "BookPage/TextPage", order = 1)]
public class TextPageBookPage:BookPage{

    public string titleL;

    [TextArea(30,40)]
    public string textL;

    public string titleR;

    [TextArea(30,40)]
    public string textR;
    public override void Activate(BookController bc){
        bc.activePageController=bc.textPageBookController;
        bc.textPageBookController.titleL.text=titleL;
        bc.textPageBookController.titleR.text=titleR;
        bc.textPageBookController.textL.text=textL;
        bc.textPageBookController.textR.text=textR;
    }
}