using UnityEngine;
using System.Collections.Generic;

public class CharacterStorage:MonoBehaviour{
    public Character florist,mayor,prankster;
    public static List<string> baseCharacterNames = new List<string>(){"Florist","Mayor","Prankster"};
    public static CharacterStorage instance;
    void Awake(){
        if (instance!=null){
            GameObject.Destroy(gameObject);
        }
        instance=this;
        DontDestroyOnLoad(gameObject);

        florist.dialogue=PremadeDialogues.Florist();
        mayor.dialogue=PremadeDialogues.Mayor();
        prankster.dialogue=PremadeDialogues.Prankster();
    }
    public static Character GetCharacter(string name, CharType type) => type switch{
        CharType.workshop=>WorkshopGet(name),
        CharType.custom=>CustomGet(name),
        _=>BaseGet(name)
        
    };
    public static Character BaseGet(string name) => name switch{
            "Florist" => instance.florist,
            "Mayor" => instance.mayor,
            "Prankster" => instance.prankster,
            _=> instance.florist,
    };
    public static Character WorkshopGet(string name){
        #if UNITY_STANDALONE_WIN || UNITY_EDITOR
        try{
            Character c = SaveLoad<CustomCharacter>.Load("workshopcharacters",name+".wch").GetCharacter(CharType.workshop);
            c.type=CharType.workshop;
            c.workshopID=name;
            return c;
        }
        catch{
            return null;
        }
        #else
        return null;
        #endif
    }
    public static Character CustomGet(string name){
        #if UNITY_STANDALONE_WIN || UNITY_EDITOR
        try{
            Character c = SaveLoad<CustomCharacter>.Load("characters",name+".ch").GetCharacter(CharType.custom);
            c.type=CharType.custom;
            return c;
        }
        catch{
            return null;
        }
        #else
        return null;
        #endif

    }
}