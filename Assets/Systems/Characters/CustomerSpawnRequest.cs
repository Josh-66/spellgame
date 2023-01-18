
using System;

[Serializable]
public class CustomerSpawnRequest{
    public string name;
    public bool returning=false;
    public string complaint;

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
        return CharacterStorage.GetCharacter(name);
    }
    
}