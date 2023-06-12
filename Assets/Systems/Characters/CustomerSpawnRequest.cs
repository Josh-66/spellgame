
using System;

[Serializable]
public class CustomerSpawnRequest{
    public string name;
    public bool returning=false;
    public string complaint;
    Character character = null;
    public CustomerSpawnRequest(Character c){
        character=c;
    }
    public CustomerSpawnRequest(Character c,string key){
        character=c;
        returning=true;
        complaint=key;
    }
    public CustomerSpawnRequest(string n){
        name=n;
        returning=false;
    }
    public CustomerSpawnRequest(string n,string key){
        name=n;
        returning=true;
        complaint=key;
    }
    public Character GetCharacter(){
        if (character!=null)
            return character;
        return CharacterStorage.GetCharacter(name);
    }
    
}