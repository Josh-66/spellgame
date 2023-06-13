using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleOptionsController : MonoBehaviour
{
    public bool gameStarted;
    public GameObject continueButton,editorButton,exitButton,customGameButton;
    // Start is called before the first frame update
    void Start()
    {
        AudioUtility.SetAllVolumes();
        if (!SaveData.validSave)
            continueButton.SetActive(false);
        
        #if UNITY_STANDALONE_WIN || (UNITY_EDITOR && !UNITY_WEBGL)
        editorButton.SetActive(true);
        if (PlayerPrefs.GetInt("GamesFinished",0)>=2){
            customGameButton.SetActive(true);
            editorButton.SetActive(true);
        }
        #else
        exitButton.SetActive(false);
        editorButton.SetActive(false);      
        #endif
    }
    public void NewGame(){
        if (gameStarted)
            return;

        gameStarted=true;

        GameController.PrepareBaseGame();


        ReviewAppController.evaluations=null;
        BedroomController.morning=true;

        Utility.FadeToScene("Bedroom");
        
    }   
    public void CustomGame(){
        GameSettingsWindow.instance.gameObject.SetActive(true);
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
    public void Exit(){
        Application.Quit();
    }
    
}
