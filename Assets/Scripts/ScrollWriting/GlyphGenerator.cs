#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
using System.Text.RegularExpressions;
public class GlyphGenerator : MonoBehaviour
{
    void Start(){
        string[] categories = new string[]{"Elements","Forms","Strengths","Styles","Other"};

        Stroke s = gameObject.AddComponent<Stroke>();
       
        string pattern = "[0-9]*";
        foreach (string category in categories)
        {
            Texture2D[] templates = Resources.LoadAll<Texture2D>("Templates/"+category);

            
            foreach(Texture2D t in templates){ 
                s.texture=t;
                s.CalculateBoxes();
                Glyph g = s.glyph;
                g.type=Enum.Parse<GlyphType>(Regex.Replace(t.name,pattern,"")); 
                AssetDatabase.CreateAsset(g, $"Assets/Glyphs/Resources/Glyphs/{category}/{t.name}.asset");
            }
        }

        
        AssetDatabase.SaveAssets();
        Glyph.ClearGlyphs();
    }
}
#endif