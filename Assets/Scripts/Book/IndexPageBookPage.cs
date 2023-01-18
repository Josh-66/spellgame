using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "IndexPage", menuName = "BookPage/IndexPage", order = 1)]
public class IndexPageBookPage:BookPage{

    public Sprite image;
    public Color color= new Color(.8f,.8f,.8f);
    public GlyphType type;
    [TextArea(15,25)]
    public string description;

    public override void Activate(BookController bc){
        
        IndexPageBookController ipbc = bc.indexPageBookController;
        bc.activePageController=ipbc;

        ipbc.image.enabled = (image!=null);
        ipbc.image.sprite=image;
        ipbc.image.color=color;
        ipbc.title.text=type.ToString();
        ipbc.description.text=description;
    }
}