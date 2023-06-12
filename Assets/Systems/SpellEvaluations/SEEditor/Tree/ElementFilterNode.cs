using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEditor;
using System.Linq;
public class ElementFilterNode : SENode
{
    public List<GlyphTypeElement> filterTypes;

    public override bool MatchSpell(Spell spell){
        bool match = false;
        foreach(GlyphTypeElement g in filterTypes){
            if((GlyphType)g==GlyphType.Invalid)
                continue;
            if ((GlyphType)g==spell.element)
            {
                match=true;
                break;
            }
        } 
        return match;
    }
    public override Color GetColor()
    {
        return Color.red;
    }
}