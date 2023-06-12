using UnityEngine;
using System.Collections.Generic;

public class CharacterStorage:MonoBehaviour{
    public Character florist,mayor,prankster;
    public static List<string> baseCharacterNames = new List<string>(){"Florist","Mayor","Prankster"};
    public static CharacterStorage instance;
    void Awake(){
        if (instance!=null)
            GameObject.Destroy(gameObject);
        instance=this;
        DontDestroyOnLoad(gameObject);

        florist.dialogue=PremadeDialogues.Florist();
        mayor.dialogue=PremadeDialogues.Mayor();
        prankster.dialogue=PremadeDialogues.Prankster();
    }
    public static Character GetCharacter(string name) => name switch{
        "Florist" => instance.florist,
        "Mayor" => instance.mayor,
        "Prankster" => instance.prankster,
        _=> DefaultGet(name),
    };
    public static Character DefaultGet(string name){
        #if UNITY_STANDALONE_WIN || UNITY_EDITOR
        try{
            return SaveLoad<CustomCharacter>.Load("characters",name+".ch").GetCharacter();
        }
        catch{
            return null;
        }
        #else
        return null;
        #endif

    }
}