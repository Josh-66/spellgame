using UnityEngine;
using System.Collections.Generic;


public enum Expression{
    Normal,
    Happy,
    Angry,
    Special,
    Icon
}
public enum CharType{
    baseGame,custom,workshop
}

[System.Serializable]
[CreateAssetMenu()]
public class Character : ScriptableObject{
    new public string name;
    public Color textColor;
    public AudioClip textSound;
    public SpellEvaluationTree spellEvalTree;
    public Sprite baseSprite,happy,angry,special;
    public Sprite profileIcon;
    public CharType type=CharType.baseGame;
    #if UNITY_STANDALONE_WIN || (UNITY_EDITOR && !UNITY_WEBGL)
    public string workshopID;
    #endif
    public string GetSaveName(){
        #if UNITY_STANDALONE_WIN || (UNITY_EDITOR && !UNITY_WEBGL)
        if (type==CharType.workshop)
            return workshopID;
        #endif
        return name;
    }
    public CharacterDialogue dialogue{
        get{
            return _characterDialogue;
        }
        set{
            _characterDialogue=value;
        }
    }
    private CharacterDialogue _characterDialogue=null;

    public Sprite GetSprite(Expression expression) => expression switch{
        Expression.Normal=>baseSprite,
        Expression.Angry=>angry,
        Expression.Happy=>happy,
        Expression.Special=>special,
        _=>baseSprite,
    };   
    public Dialogue GetDialogue(string dialogueKey){
        return dialogue[dialogueKey];
    }
    public Evaluation GetEvaluation(Spell s){
        if (spellEvalTree==null)
            return new Evaluation("error",0,"no eval tree",false);
        return spellEvalTree.EvaluateSpell(s);
    }
}