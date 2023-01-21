using UnityEngine;

public class CharacterStorage:MonoBehaviour{
    public Character florist,mayor,prankster;
    public static CharacterStorage instance;
    void Awake(){
        instance=this;
    }
    public static Character GetCharacter(string name) => name switch{
        "Florist" => instance.florist,
        "Mayor" => instance.mayor,
        "Prankster" => instance.prankster,
        _=> null,
    } ;
}