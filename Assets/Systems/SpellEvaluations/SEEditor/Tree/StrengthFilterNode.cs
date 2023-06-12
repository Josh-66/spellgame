using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEditor;
using System.Linq;
public class StrengthFilterNode : SENode
{
    public List<GlyphTypeStrength> filterTypes;

    public override bool MatchSpell(Spell spell){
        bool match = false;
        foreach(GlyphTypeStrength g in filterTypes){
            if((GlyphType)g==GlyphType.Invalid)
                continue;
            if ((GlyphType)g==spell.strength)
            {
                match=true;
                break;
            }
        } 
        return match;
    }
    public override Color GetColor()
    {
        return Color.yellow;
    }
}