using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "QuickRef", menuName = "BookPage/QuickRef", order = 1)]
public class QuickRefBookPage:BookPage{
    public Sprite glyph1;
    public GlyphType name1;
    public Color color1 = new Color(.8f,.8f,.8f);
    public Sprite glyph2;
    public GlyphType name2;
    public Color color2 = new Color(.8f,.8f,.8f);
    public Sprite glyph3;
    public GlyphType name3;
    public Color color3 = new Color(.8f,.8f,.8f);
    public Sprite glyph4;
    public GlyphType name4;
    public Color color4 = new Color(.8f,.8f,.8f);

    public override void Activate(BookController bc){
        QuickRefBookController qr = bc.quickRefBookController;

        bc.activePageController=qr;

        qr.image1.enabled = (glyph1!=null);
        qr.image2.enabled = (glyph2!=null);
        qr.image3.enabled = (glyph3!=null);
        qr.image4.enabled = (glyph4!=null);
        
        qr.image1.sprite=glyph1;
        qr.image2.sprite=glyph2;
        qr.image3.sprite=glyph3;
        qr.image4.sprite=glyph4;

        qr.image1.color=color1;
        qr.image2.color=color2;
        qr.image3.color=color3;
        qr.image4.color=color4;

        qr.text1.text=name1.ToString();
        qr.text2.text=name2.ToString();
        qr.text3.text=name3.ToString();
        qr.text4.text=name4.ToString();

        qr.type1=name1;
        qr.type2=name2;
        qr.type3=name3;
        qr.type4=name4;

    }
}