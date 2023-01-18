using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour,Clickable
{
    public Character character;
    public static CustomerController instance;
    public static bool active;
    public static bool isEntry = true;
    public static string complaint = "NONE";
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public static Material outlined,regular;
    bool hovered;
    public bool arrived = false;
    void Awake(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        instance=this;
        animator=GetComponent<Animator>();
    }
    public void Enter(){
        CustomerController.instance.animator.SetTrigger("Enter");
        spriteRenderer.sprite=character.baseSprite;
        arrived=false;
        active=true;
    }
    public void Exit(){
        CustomerController.instance.animator.SetTrigger("Exit");
        arrived=false;
    }
    public void FinishedEntering(){
        arrived=true;
        if (isEntry)
            DialogueController.PlayDialogue(character,character.dialogue.entry);
        else{
            DialogueController.PlayDialogue(character,complaint);
            DialogueController.OnComplete= ()=>{
                GameController.instance.LeaveCustomer(true);
                Exit();
            };
        }
    }
    public void FinishedExiting(){
        active=false;
    }
    void Update() {
        if (arrived){
            if (hovered || DialogueController.playing){
                spriteRenderer.material=regular;
            }
            else if (!hovered){
                spriteRenderer.material=outlined;
            }
        }
        else{
            spriteRenderer.material=regular;
        }
    }
    public void OnHover()
    {
        hovered = true;
    }

    public void OffHover()
    {
        hovered = false;
    }

    public void OnClick()
    {
        if (!arrived)
            return;
        //Chat
        if (!DialogueController.playing){
            DialogueController.PlayDialogue(character,character.dialogue.chats.RandomElement<Dialogue>());
        }
    }
}