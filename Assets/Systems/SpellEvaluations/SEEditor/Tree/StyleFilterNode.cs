using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEditor;
using System.Linq;
public class StyleFilterNode : SENode
{
    public List<GlyphTypeStyle> filterTypes;


    public override bool MatchSpell(Spell spell){
        bool match = false;
        foreach(GlyphTypeStyle g in filterTypes){
            if((GlyphType)g==GlyphType.Invalid)
                continue;
            if ((GlyphType)g==spell.style)
            {
                match=true;
                break;
            }
        } 
        return match;
    }

    public override Color GetColor()
    {
        return Color.green;
    }

}