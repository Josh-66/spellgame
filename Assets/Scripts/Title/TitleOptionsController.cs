using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleOptionsController : MonoBehaviour
{
    public bool gameStarted;
    public GameObject continueButton;
    // Start is called before the first frame update
    void Start()
    {
        AudioUtility.SetAllVolumes();
        if (!SaveData.validSave)
            continueButton.SetActive(false);
    }
    public void NewGame(){
        if (gameStarted)
            return;

        if (PlayerPrefs.GetInt("GamesFinished",0)>2){
            GameSettingsWindow.instance.gameObject.SetActive(true);
        }
        else{
            gameStarted=true;

            GameController.PrepareBaseGame();


            ReviewAppController.evaluations=null;
            BedroomController.morning=true;
            Utility.FadeToScene("Bedroom");
        }
        
    }   
    public void Continue(){
        if (gameStarted)
            return;
        gameStarted=true;
        GameController.PrepareGame(GameController.GameType.load);
        Utility.FadeToScene("Shop");
    }
    public void Options(){
        BookController.OpenOptions();
    }
    public void Editor(){
        Utility.FadeToScene("CharacterEditor");
    }
    
}
