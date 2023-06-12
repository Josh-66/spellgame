using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEditor;
using System.Linq;
public class FilterNode : SENode
{
    public List<GlyphType> filterTypes;
    public override bool MatchSpell(Spell spell){
        bool match = false;
        foreach(GlyphType g in filterTypes){
            if(g==GlyphType.Invalid)
                continue;
            if (spell.Contains(g))
            {
                match=true;
                break;
            }
        } 
        return match;
    }
    public override Color GetColor()
    {
        return Color.black;
    }

}