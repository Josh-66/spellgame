using UnityEngine;



public enum Expression{
    Normal,
    Happy,
    Angry,
    Special
}

[CreateAssetMenu()]
public class Character : ScriptableObject{
    new public string name;
    public Color textColor;
    public AudioClip textSound;
    public SpellEvaluationTree spellEvalTree;
    public Sprite baseSprite,happy,angry,special;
    public CharacterDialouge dialogue{
        get{
            if (_characterDialogue==null){
                _characterDialogue = name switch{
                    "Florist"=>PremadeDialogues.Florist(),
                    "Mayor"=>PremadeDialogues.Mayor(),
                    "Prankster"=>PremadeDialogues.Prankster(),
                    _=>throw new System.Exception("Invalid Character Name"),
                };
            }
            return _characterDialogue;
        }
    }
    private CharacterDialouge _characterDialogue;

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