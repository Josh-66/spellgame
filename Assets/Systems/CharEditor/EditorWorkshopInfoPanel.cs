#if UNITY_STANDALONE_WIN || (UNITY_EDITOR && !UNITY_WEBGL)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EditorWorkshopInfoPanel : MonoBehaviour
{
    public TextMeshProUGUI idText, log, buttontext;
    public GameObject blocker;
    public bool workshopInProgress;
    // Start is called before the first frame update
    void Start()
    {
        //log.transform.parent.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        blocker.SetActive(workshopInProgress);
        if (SteamWorkshopUpload.inProgress){
            
            if (workshopInProgress==false){
                //log.transform.parent.gameObject.SetActive(true);
                log.text="Log:\n"+SteamWorkshopUpload.message;
                log.pageToDisplay++;
                workshopInProgress=true;
            }
        }
        else{
            if (workshopInProgress==true){
                //log.transform.parent.gameObject.SetActive(true);
                workshopInProgress=false;
                log.text="Log:\n"+SteamWorkshopUpload.message;
                UpdateInfo();
            }
        }
    }
    public void UpdateInfo(){
        string id = CharacterEditor.currentCharacter.workshopID.m_PublishedFileId.ToString();
        idText.text="ID:\n " + id;
        buttontext.text = id=="0" ? "Upload to Workshop" : "Update in Workshop";
    }
}
#endif