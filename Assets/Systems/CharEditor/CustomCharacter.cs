#if UNITY_STANDALONE_WIN || UNITY_EDITOR
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
[Serializable]
public class CustomCharacter{
    public string name;
    public Color color = new Color(1,1,1);
    //Textures Base64
    public string normal,happy,angry,special,icon;
    //OGG File Data Base64
    public string textSound;
    public string textSoundName = "None";

    public CustomSpellEvalTree spellEvalTree = new CustomSpellEvalTree();
    public CharacterDialogue dialogue = new CharacterDialogue();
    public SpellEvaluationTree GetTree(){
        return spellEvalTree.GetTree();
    }
    public Character GetCharacter(){
        Character c = ScriptableObject.CreateInstance<Character>();
        c.name=name;
        c.textColor=color;
        c.textSound=GetTextSound();
        c.spellEvalTree=GetTree();
        c.baseSprite=GetSprite(Expression.Normal);
        c.happy=GetSprite(Expression.Happy);
        c.angry=GetSprite(Expression.Angry);
        c.special=GetSprite(Expression.Special);
        c.profileIcon=GetSprite(Expression.Icon);
        c.dialogue=dialogue;
        return c;
    }
    public Sprite GetSprite(Expression expression){
        byte[] bytes = this[expression];
        Texture2D newTex = new Texture2D(1,1);
        newTex.LoadImage(bytes);
        Sprite sprite = Sprite.Create(newTex,new Rect(0,0,newTex.width,newTex.height),Vector2.right*.5f,Mathf.Max(1,Mathf.Max(newTex.width,newTex.height)/1056f*120f),10,SpriteMeshType.Tight);

        return sprite;
    }
    public void SetTextSound(byte[] bytes){
        textSound = System.Convert.ToBase64String(bytes);
    }
    public AudioClip GetTextSound(){
        byte[] clipBytes = System.Convert.FromBase64String(textSound);

        string ext = Path.GetExtension(textSoundName);
        if (ext==".ogg"){
           UnityEngine.AudioClip sourceAudioClip = OggVorbis.VorbisPlugin.ToAudioClip(clipBytes, textSoundName);
           return sourceAudioClip;
        }
        return null;

    }
    public byte[] this[Expression e]
    {
        get
        {
            string str =  e switch
            {
                Expression.Normal => normal ,
                Expression.Happy =>  happy  ,
                Expression.Angry =>  angry  ,
                Expression.Special =>special,
                Expression.Icon => icon,
                _ =>                 normal 
            };
            return System.Convert.FromBase64String(str);
        }
       set
        {
            string data = System.Convert.ToBase64String(value);
            switch (e)
            {
                case Expression.Normal:
                    normal = data; 
                    break;
                case Expression.Happy:
                    happy = data;
                    break;
                case Expression.Angry:
                    angry = data;
                    break;
                case Expression.Special:
                    special = data;
                    break;
                case Expression.Icon:
                    icon = data;
                    break;
                default:
                    normal = data;
                    break;
            }
        }
    }


}
#endif