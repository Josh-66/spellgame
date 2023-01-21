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
    public static Action OnComplete;
    public Dialogue dialogue;
    public static bool playing = false;
    public int lineIndex= -1;
    public float charIndex=0;
    public float soundTimer = .1f;
    float soundCD = .10f;


    GameObject targetWindow,otherWindow;
    AudioSource targetAS,otherAS;
    TextMeshProUGUI targetText,otherText;
    bool lineInitialized=false;
    void Awake()
    {    
        instance=this;
        
        SetWindowsVisible(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playing){

            Line line = dialogue[lineIndex];
            while (line.isAction){
                PerformLineAction(line);
                lineIndex++;
                if (lineIndex>=dialogue.lines.Length){
                    playing=false;
                    SetWindowsVisible(false);
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
            
            if (charIndex<line.text.Length){
                soundTimer-=Time.deltaTime;
                if (soundTimer<0){
                    soundTimer=soundCD;
                    targetAS.Play();
                }
            }

            if (charIndex>line.text.Length+10){
                lineIndex++;
                lineInitialized=false;
                charIndex=0;
                if (lineIndex>=dialogue.lines.Length){
                    playing=false;
                    SetWindowsVisible(false);
                    if (OnComplete!=null){
                        OnComplete();
                        OnComplete=null;
                    }
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
        switch (line.actionType){
            case 1:
                CustomerController.instance.spriteRenderer.sprite=CustomerController.instance.character.GetSprite(line.expression);
                break;
        }
    }
}
