using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEditor;
using System.Linq;
public class BonusFilterNode : SENode
{
    public List<GlyphType> filterTypes;
    VisualElement customDataContainer;


    
    public override bool MatchSpell(Spell spell){
        bool match = true;
        foreach(GlyphType g in filterTypes){
            if (!spell.Contains(g))
            {
                match=false;
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