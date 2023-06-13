
using System;

[Serializable]
public class CustomerSpawnRequest{
    public string name;
    public bool returning=false;
    public string complaint;
    public CharType type;
    Character character = null;
    public CustomerSpawnRequest(Character c){
        character=c;
        this.type=c.type;
    }
    public CustomerSpawnRequest(Character c,string key){
        character=c;
        returning=true;
        complaint=key;
        this.type=c.type;
    }
    public CustomerSpawnRequest(string n,CharType type){
        name=n;
        returning=false;
        this.type=type;
    }
    public CustomerSpawnRequest(string n,string key,CharType type){
        name=n;
        returning=true;
        complaint=key;
        this.type=type;
    }
    public Character GetCharacter(){
        if (character!=null)
            return character;
        return CharacterStorage.GetCharacter(name,type);
    }
    
}