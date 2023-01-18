using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Glyph", menuName = "", order = 1)]
public class Glyph : ScriptableObject
{   
    public static int size = 14;
    public static Glyph[] glyphs{ 
        get{
            if (_glyphs==null)
                LoadGlyphs();
            return _glyphs; 
        }
    }
    private static Glyph[] _glyphs;
    public static void ClearGlyphs(){_glyphs=null;}
    public static void LoadGlyphs(){_glyphs = Resources.LoadAll<Glyph>("Glyphs");}
    public GlyphType type=GlyphType.Invalid;
    public bool[] boxes = new bool[size*size];
    public int count{get{
        if (_count>=0)
            return _count;
        else{
            _count=0;
            foreach(bool b in boxes){
                if (b)
                    _count++;
            }
            return _count;
        }
    }}

    private int _count=-1;
    
    public bool this[int i]{
        get{
            return boxes[i];
        }
        set{
            boxes[i]=value;
        }
    }
    public bool this[int x,int y]{
        get{
            if (x>=size || x < 0 || y>=size || y<0)
                return false;
            return boxes[x+y*size];
        }
        set{
            boxes[x+y*size]=value;
        }
    }
    public bool this[Vector3Int coords]{
        get{
            return boxes[coords.x+coords.y*size];
        }
        set{
            boxes[coords.x+coords.y*size]=value;
        }
    }
    public bool this[Vector2Int coords]{
        get{
            return boxes[coords.x+coords.y*size];
        }
        set{
            boxes[coords.x+coords.y*size]=value;
        }
    }

    public float OldCompare(Glyph other){
        if (Mathf.Abs(count-other.count)>5)
            return 0;

        float score = 0;
        float total = 0;
        for(int i= 0; i < size * size; i++){
            if (boxes[i]||other[i])
                total++;
            if (boxes[i]&&other[i])
                score+=100;
        }
        return score/total;
    }
    public float Compare(Glyph other){
        // if (Mathf.Abs(count-other.count)>5)
        //     return 0;

        //Check this against other
        float diff1 = Diff(this,other);
        //Check other against this
        float diff2 = Diff(other,this);
        //Return worst diff
        return Mathf.Min(diff1,diff2);
        
    }
    private static float Diff(Glyph g1, Glyph g2){
        if (Mathf.Abs(g1.count-g2.count)>30)
            return 0;

        float total = 0;
        float score = 0;
        float bads = 0;
        for (int x = 0 ; x < size; x++){
            for (int y = 0 ; y < size;y++){
                if (g1[x,y]){
                    total++;
                    //Perfect match
                    if (g2[x,y]){
                        score+=100f;
                    }
                    //Adjacent
                    else if (g2[x+1,y]||g2[x-1,y]||g2[x,y+1]||g2[x,y-1])
                    { 
                        score+=60f;
                    }
                    //Diagonal
                    else{
                        bads++;
                        
                        if (g2[x+1,y+1]||g2[x+1,y-1]||g2[x-1,y+1]||g2[x-1,y-1]){
                            score+=50f;
                        }
                        //2 Away
                        else if (g2[x+2,y]||g2[x-2,y]||g2[x,y+2]||g2[x,y-2]){
                            score+=25f;
                        }
                        //2 away diagonal
                        else if (g2[x+2,y+2]||g2[x+2,y-2]||g2[x-2,y+2]||g2[x-2,y-2]){
                            score+=0f;
                        }
                        else{
                            //Really far away
                            bads+=4;
                            if (bads>8)
                            return 0;
                        }
                        if (bads>8)
                            return 0;
                    }
                }
            }
        }
        return score/total;
    }
}
public static class GlyphUtility{


    public static bool IsElement(this GlyphType type){
        return type.GetCategory()==GlyphCategory.Element;
    }
    public static bool IsForm(this GlyphType type){
        return type.GetCategory()==GlyphCategory.Form;
    }
    public static bool IsStrength(this GlyphType type){
        return type.GetCategory()==GlyphCategory.Strength;
    }
    public static bool IsStyle(this GlyphType type){
        return type.GetCategory()==GlyphCategory.Style;
    }
    public static Color GetElementColor(this GlyphType type) => type switch{
        GlyphType.Fire => Utility.ColorFromHex(0xdb5d44),
        GlyphType.Air => Utility.ColorFromHex(0xbaf6ff),
        GlyphType.Water => Utility.ColorFromHex(0x3643ba),
        GlyphType.Electricity => Utility.ColorFromHex(0xbd79d1),
        GlyphType.Earth => Utility.ColorFromHex(0x573915),
        GlyphType.Nature => Utility.ColorFromHex(0x9fd984),
        GlyphType.Blood => Utility.ColorFromHex(0x631e29),
        GlyphType.Gag => Utility.ColorFromHex(0x5e5863),
        _=>Color.white,
    };
    public static GlyphCategory GetCategory(this GlyphType type) => type switch{
        >=GlyphType.Fire and <= GlyphType.Gag => GlyphCategory.Element,
        >=GlyphType.Ball and <= GlyphType.Down => GlyphCategory.Form,
        >=GlyphType.Low and <= GlyphType.Ludicrous => GlyphCategory.Strength,
        >=GlyphType.Sparkles and <= GlyphType.Glowing => GlyphCategory.Style,
        _ => GlyphCategory.Invalid,
        
    };
}