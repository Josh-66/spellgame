using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEditor;
using System.Linq;
public class FormFilterNode : SENode
{
    public List<GlyphTypeForm> filterTypes;
    VisualElement customDataContainer;



   public override bool MatchSpell(Spell spell){
        bool match = false;
        foreach(GlyphTypeStrength g in filterTypes){
            if ((GlyphType)g==spell.form)
            {
                match=true;
                break;
            }
        } 
        return match;
    }
    public override Color GetColor()
    {
        return new Color(1,.5f,0);
    }
}