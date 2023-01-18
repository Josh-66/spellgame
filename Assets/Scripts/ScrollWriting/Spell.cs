using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Spell
{
    public GlyphType element = GlyphType.Invalid;
    public GlyphType form = GlyphType.Invalid;

    public GlyphType strength = GlyphType.Invalid;
    public GlyphType style = GlyphType.Invalid;
    public GlyphType bonus = GlyphType.Invalid;
    


    public GlyphType this[GlyphCategory g] {get => g switch{
            GlyphCategory.Element => element,
            GlyphCategory.Form => form,
            GlyphCategory.Strength => strength,
            GlyphCategory.Style => style,
            _ => GlyphType.Invalid,
        };
        set{
            switch(g){
                case GlyphCategory.Element:
                    element=value;
                    break;
                case GlyphCategory.Form:
                    form=value;
                    break;
                case GlyphCategory.Strength:
                    strength=value;
                    break;
                case GlyphCategory.Style:
                    style=value;
                    break;
                
            }
        }
    }
    public bool Contains(GlyphType type){
        if (element==type)
            return true;
        if (form==type)
            return true;
        if (strength==type)
            return true;
        if (style==type)
            return true;
        if (bonus==type)
            return true;
        return false;
    }
    public override string ToString(){
        string s = $"Element: {(element==GlyphType.Invalid ? "None":element)}, Form: {(form==GlyphType.Invalid ? "None":form)}, Strength: {(strength==GlyphType.Invalid ? "Medium":strength)}, Style: {(style==GlyphType.Invalid ? "None":style)}";

        if (bonus!=GlyphType.Invalid){
            s+=$", Bonus: {bonus}";
        }
        return s;
    }
}