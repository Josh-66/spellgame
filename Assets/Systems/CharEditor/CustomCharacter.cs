#if UNITY_STANDALONE_WIN || (UNITY_EDITOR && !UNITY_WEBGL)
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
[Serializable]
public class CustomCharacter{
    public static readonly string defaultSound = "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";
    public string name;
    public Color color = new Color(1,1,1);
    //Textures Base64
    public string normal,happy,angry,special,icon;
    //OGG File Data Base64
    public string textSound = defaultSound;
    public string textSoundName = "None";
    public Steamworks.PublishedFileId_t workshopID=Steamworks.PublishedFileId_t.Invalid;

    public CustomSpellEvalTree spellEvalTree = new CustomSpellEvalTree();
    public CharacterDialogue dialogue = new CharacterDialogue();
    public SpellEvaluationTree GetTree(){
        return spellEvalTree.GetTree();
    }
    public Character GetCharacter(CharType type){
        Character c = ScriptableObject.CreateInstance<Character>();
        c.type=type;
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