using UnityEngine;
using System.Collections.Generic;


public enum Expression{
    Normal,
    Happy,
    Angry,
    Special,
    Icon
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
    public CharacterDialogue dialogue{
        get{
            if (_characterDialogue==null || _characterDialogue.entry.lines.Count==0){
                _characterDialogue = name switch{
                    "Florist"=>PremadeDialogues.Florist(),
                    "Mayor"=>PremadeDialogues.Mayor(),
                    "Prankster"=>PremadeDialogues.Prankster(),
                    _=>throw new System.Exception("Invalid Character Name"),
                };
            }
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