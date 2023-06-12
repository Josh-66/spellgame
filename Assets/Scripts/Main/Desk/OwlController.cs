using UnityEngine;
using System;
public class OwlController:MonoBehaviour{
    
    public UnityEngine.UI.Image blackOut;
    public static OwlController instance;
    public AudioSource source;
    public SpriteRenderer spriteRenderer;
    public Sprite with,sans;
    public Character character;
    public void Awake(){
        instance=this;
    }
    public static void Appear(Action onAppear=null){
        instance.blackOut.enabled=true;
        instance.source.Play();
        Delayer.DelayAction(.1f,()=>{instance.spriteRenderer.sprite=instance.with;});
        Delayer.DelayAction(.2f,()=>{instance.blackOut.enabled=false;instance.source.Play();});
        Delayer.DelayAction(1f,()=>{if (onAppear!=null) onAppear();});
        

    }
    public static void Disappear(Action ondisAppear=null){
        instance.blackOut.enabled=true;
        instance.source.Play();
        Delayer.DelayAction(.1f,()=>{instance.spriteRenderer.sprite=instance.sans;});
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
            tal("Good morning. Hoot."),
            player("Hello, my ever faithful companion."),
            tal("Hoot. You still need to draw your stamp for the day."),
            player("Ughhh I knowww. I was about to do it anyways.")
        );
        return d;
    }
    public static Dialogue OpeningEndDialogue(){
        Dialogue d = new Dialogue("OpeningEnd",
            player("Alright, I think I'm ready to open."),
            tal("Alright, have fun. Hoot."),
            tal("Don't forget, your stamp needs to go on every spell you give out."),
            player("I know, I know. Now shoo!")
        );
        return d;
    }
    public static Dialogue EndingDialogue(){
        Dialogue d = new Dialogue("Ending",
            tal("Hoot. That looks like the last person."),
            player("Finally! I thought they'd never leave me alone."),
            tal("You run a shop. Hoot."),
            player("Doesn't mean I like interacting with these losers."),
            player("I'm in it for the art form."),
            tal("Whatever you say. Hoot.")
        );
        return d;
    }

}