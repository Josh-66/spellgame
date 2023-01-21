using UnityEngine;
using System;
public class OwlController:MonoBehaviour{
    
    public UnityEngine.UI.Image blackOut;
    public static OwlController instance;
    public AudioSource source;
    public SpriteRenderer spriteRenderer;
    public Character character;
    public void Awake(){
        instance=this;
    }
    public static void Appear(Action onAppear=null){
        instance.blackOut.enabled=true;
        instance.source.Play();
        Delayer.DelayAction(.1f,()=>{instance.spriteRenderer.enabled=true;});
        Delayer.DelayAction(.2f,()=>{instance.blackOut.enabled=false;instance.source.Play();});
        Delayer.DelayAction(1f,()=>{if (onAppear!=null) onAppear();});
        

    }
    public static void Disappear(Action ondisAppear=null){
        instance.blackOut.enabled=true;
        instance.source.Play();
        Delayer.DelayAction(.1f,()=>{instance.spriteRenderer.enabled=false;});
        Delayer.DelayAction(.2f,()=>{instance.blackOut.enabled=false;instance.source.Play();});
        Delayer.DelayAction(1f,()=>{if (ondisAppear!=null) ondisAppear();});
        

    }
    public static Line tal(string t){
        return new Line(true,t);
    }
    public static Line player(string t){
        return new Line(false,t);
    }
    public static Dialogue OpeningDialogue(){
        Dialogue d = new Dialogue("Opening",
            tal("Good morning."),
            player("Hi"),
            tal("You still need to draw your stamp for the day."),
            player("I know, I know, give me a moment")
        );
        return d;
    }
    public static Dialogue OpeningEndDialogue(){
        Dialogue d = new Dialogue("OpeningEnd",
            player("Alright, I think I'm ready to open."),
            tal("Alright, sport, have fun."),
            tal("Don't forget, your stamp needs to go on every spell you give out."),
            player("I know, I know. Now shoo!")
        );
        return d;
    }
    public static Dialogue EndingDialogue(){
        Dialogue d = new Dialogue("Ending",
            tal("Hoot. That looks like the last person."),
            player("I'm beat, I can't wait to lie down.")
        );
        return d;
    }

}