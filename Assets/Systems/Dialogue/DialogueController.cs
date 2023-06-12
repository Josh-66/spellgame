using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
public class DialogueController : MonoBehaviour
{
    public GameObject playerWindow,customerWindow;
    public TextMeshProUGUI playerText,customerText; 
    public AudioSource playerAS,customerAS;
    public static DialogueController instance;
    public static Action OnComplete {get{return instance._onComplete;}set{instance._onComplete=value;}}
    Action _onComplete=null;
    public static bool playing {get{return instance._playing;}set{instance._playing=value;}}
    public Dialogue dialogue;
    bool _playing = false;
    public int lineIndex= -1;
    public float charIndex=0;
    public float soundTimer = .1f;
    float soundCD = .10f;

    public Image playerPrompt,customerPrompt;
    GameObject targetWindow,otherWindow;
    AudioSource targetAS,otherAS;
    TextMeshProUGUI targetText,otherText;
    bool lineInitialized=false;
    Line line = null;
    void Awake()
    {    
        instance=this;
        SetWindowsVisible(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playing){
            line = dialogue[lineIndex];
            if (line==null){
                EndDialogue();
                return;
            }
            while (line.type!=Line.Type.text){
                PerformLineAction(line);
                lineIndex++;
                if (lineIndex>=dialogue.lines.Count){
                    EndDialogue();
                    return;
                }
                line = dialogue[lineIndex];
            }
            if (!lineInitialized){
                InitializeLine(line);
                lineInitialized=true;
            }
            charIndex+=Time.deltaTime * 10 * TextSpeed.textSpeed;
            targetText.text=line.text.Substring(0,Mathf.FloorToInt(Mathf.Min(charIndex,line.text.Length)));
            
            playerPrompt.enabled = customerPrompt.enabled = charIndex>=line.text.Length;

            if (charIndex<line.text.Length){
                soundTimer-=Time.deltaTime;
                if (soundTimer<0){
                    soundTimer=soundCD;
                    targetAS.Play();
                }
            }
            if (charIndex>=line.text.Length && TimescaleController.isFastForward && TimescaleController.isDialogue){
                lineIndex++;
                lineInitialized=false;
                charIndex=0;
                if (lineIndex>=dialogue.lines.Count){
                    EndDialogue();
                }
            }
            
        }
    }
    public static void PlayDialogue(Character character, string dialogueKey){
        PlayDialogue(character,character.GetDialogue(dialogueKey));
    }
    public static void PlayDialogue(Character character, Dialogue dialogue){
        instance.dialogue=dialogue;
        playing=true;
        instance.lineIndex = 0;
        instance.customerAS.clip=character.textSound;
        instance.customerText.color=character.textColor;

        instance.playerText.text="";
        instance.customerText.text="";

        //CustomerController.instance.spriteRenderer.sprite=CustomerController.instance.character.GetSprite(Expression.Normal);

    }
    public void SetWindowsVisible(bool b){
        playerWindow.SetActive(b);
        customerWindow.SetActive(b);
    }
    public void SetTargetOther(Line line){
        targetWindow = line.customerSpeaking ? customerWindow : playerWindow;
        otherWindow  = line.customerSpeaking ? playerWindow : customerWindow;

        targetAS = line.customerSpeaking ? customerAS : playerAS;
        otherAS = line.customerSpeaking ? playerAS : customerAS;

        targetText = line.customerSpeaking ? customerText : playerText;
        otherText = line.customerSpeaking  ? playerText   : customerText;
    }
    public void InitializeLine(Line line){
        SetTargetOther(line);

        targetWindow.SetActive(true);
        soundTimer=0;
    }
    public void PerformLineAction(Line line){
        switch (line.type){
            case Line.Type.portrait:
                CustomerController.instance.spriteRenderer.sprite=CustomerController.instance.character.GetSprite((Expression)line.value);
                break;
        }
    }

    public void Clicked(){
        if (line!=null){
            if (charIndex>=line.text.Length){
                    lineIndex++;
                    lineInitialized=false;
                    charIndex=0;
                    if (lineIndex>=dialogue.lines.Count){
                        EndDialogue();
                    }
            }
            else{
                charIndex=line.text.Length;
            }
        }
    }
    void EndDialogue(){
        
        playing=false;
        SetWindowsVisible(false);
        if (OnComplete!=null){
            OnComplete();
            OnComplete=null;
        }
        TimescaleController.isDialogue=false;
    }
}
