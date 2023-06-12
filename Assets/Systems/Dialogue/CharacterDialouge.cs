using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Text.RegularExpressions;

[System.Serializable]
public class CharacterDialogue{
    public Dialogue entry = new Dialogue("Entry");
    public Dialogue[] chats;

    public Dialogue exit= new Dialogue("Exit");

    public Dialogue[] returnings;
    
    public Dialogue this[string key]{get{
        foreach(Dialogue d in returnings){
            if (d.key==key)
            return d;
        }
        if (entry.key==key)
            return entry;
        foreach(Dialogue d in chats){
            if (d.key==key)
            return d;
        }
        if (exit.key==key)
            return exit;

        return null;
    }}
    public List<string> allReturningKeys{get{
        List<string> k = new List<string>();
        if (returnings==null){
            returnings=new Dialogue[0];
        }
        foreach(Dialogue d in returnings){
            k.Add(d.key);
        }
        return k;
    }}

    public string ExportToString(){
        string res = "";
        res+="<ENTRY>\n";
        res+=entry.ExportToString();
        res+="\n<EXIT>\n";
        res+=exit.ExportToString();
        res+="\n<CHATS>\n";
        Debug.Log(chats.Length);
        foreach(Dialogue d in chats){
            res+=$"[{d.key}]\n";
            res+=d.ExportToString();
            res+="\n";
        }
        res+="<RETURNS>\n";
        foreach(Dialogue d in returnings){
            res+=$"[{d.key}]\n";
            res+=d.ExportToString();
            res+="\n";
        }
        Debug.Log(res);
        return res;
    }
    public void ImportFromString(string str){
        str=str.Trim();
        Dialogue newEntry = new Dialogue("Entry");
        Dialogue newExit = new Dialogue("Exit");
        List<Dialogue> newChats = new List<Dialogue>();
        List<Dialogue> newReturnings= new List<Dialogue>();

        Match match = Regex.Match(str,@"<ENTRY>(.*)<EXIT>(.*)<CHATS>(.*)<RETURNS>(.*)",RegexOptions.Singleline);
        newEntry.ImportFromString(match.Groups[1].Value);
        newExit.ImportFromString(match.Groups[2].Value);

        string chatsString = match.Groups[3].Value;
        string[] chatsSplit =Regex.Split(chatsString,"\\[(.*)\\]");
        for(int i = 1 ; i < chatsSplit.Length-1;i+=2){
            Dialogue newDialogue = new Dialogue(chatsSplit[i].Trim());
            newDialogue.ImportFromString(chatsSplit[i+1]);
            newChats.Add(newDialogue);
        }

        string returnsString = match.Groups[4].Value;
        string[] returnsSplit =Regex.Split(returnsString,"\\[(.*)\\]");
        for(int i = 1 ; i < returnsSplit.Length-1;i+=2){
            Dialogue newDialogue = new Dialogue(returnsSplit[i].Trim());
            newDialogue.ImportFromString(returnsSplit[i+1]);
            newReturnings.Add(newDialogue);
        }

        entry=newEntry;
        exit=newExit;
        chats=newChats.ToArray();
        returnings=newReturnings.ToArray();


    }
}
[System.Serializable]
public class Dialogue{
    public string key="";
    public List<Line> lines=new List<Line>();

    public Dialogue(string k, params Line[] l){
        key=k;lines = new List<Line>(l);
    }
    public Line this[int i] {get{
        if (i<lines.Count)
            return lines[i];
        return null;
    }}
    public string ExportToString(){
        string res = "";
        foreach(Line l in lines){
            if (l.type==Line.Type.text){
                res += (l.customerSpeaking ? "C:" : "P:")+ l.text+"\n";
            }
            else if (l.type==Line.Type.portrait){
                res += "E:"+(Expression)l.value+"\n";
            }
        }
        return res.Trim();
    }
    public void ImportFromString(string input){
        input=input.Trim();
        List<Line> newLines = new List<Line>();
        string[] inputLines = System.Text.RegularExpressions.Regex.Split(input, "\n?([CPE]:)");
        for(int i = 1 ; i < inputLines.Length-1;i+=2){
            newLines.Add(inputLines[i].Trim() switch{
                "P:"=>new Line(false,inputLines[i+1].Trim()),
                "C:"=>new Line(true,inputLines[i+1].Trim()),
                "E:"=>new Line(true,"",Line.Type.portrait,(int)System.Enum.Parse<Expression>(inputLines[i+1].Trim(),true)),
                _=> throw new System.FormatException(inputLines[i].Trim() +" <- invalid start of line"),
            });
        }

        lines=newLines;
    
    }

}
[System.Serializable]
public class Line{
    public enum Type{
        text,
        portrait
    }
    public bool customerSpeaking;
    public string text;
    public Type type;
    public int value;
    public Line(bool customerSpeaking, string text,Type type=0,int value = 0){
        if (type==Type.portrait && value==(int)Expression.Icon)
            throw new System.ArgumentException("Icons can't be portraits dummy");
        this.customerSpeaking=customerSpeaking;this.text=text;this.type=type;this.value=value;
    }
    public static Line ChangeExpression(Expression e){
        return new Line(true,"",Type.portrait,(int)e);
    } 
}