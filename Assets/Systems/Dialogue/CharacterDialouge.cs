using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CharacterDialouge{
    public Dialogue entry;
    public Dialogue[] chats;

    public Dialogue exit;

    public Dialogue[] returnings;

    public Dialogue this[string key]{get{
        if (entry.key==key)
            return entry;
        foreach(Dialogue d in chats){
            if (d.key==key)
            return d;
        }
        if (exit.key==key)
            return exit;

        foreach(Dialogue d in returnings){
            if (d.key==key)
            return d;
        }
        return null;
    }}
}
[System.Serializable]
public class Dialogue{
    public string key;
    public Line[] lines;

    public Dialogue(string k, params Line[] l){
        key=k;lines=l;
    }
    public Line this[int i] => lines[i];
}
[System.Serializable]
public class Line{
    public bool customerSpeaking;
    public string text;
    public bool isAction;
    public Expression expression;
    public int actionType = 0;//0 is normal, 1 is change expression
    public Line(bool b, string t,bool i=false){
        customerSpeaking=b;text=t;isAction=i;
    }
}