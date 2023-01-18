using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleOptionsController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioUtility.SetAllVolumes();
    }
    public void NewGame(){
        GameController.loadData=false;
        StampPaperController.stampTexture=null;
        AsyncOperation ao = SceneManager.LoadSceneAsync("Shop");
        BlackFade.FadeInAndAcion(()=>{ao.allowSceneActivation=true;});
    }   
    public void Continue(){
        GameController.loadData=true;
        AsyncOperation ao = SceneManager.LoadSceneAsync("Shop");
        BlackFade.FadeInAndAcion(()=>{ao.allowSceneActivation=true;});
    }
    public void Options(){
        BookController.OpenBook();
        BookController.instance.GoToPageByName(BookInfo.PageTabs.Options);
        Debug.Log(BookController.instance.pageNumber);
    }
}
