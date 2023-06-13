
#if UNITY_STANDALONE_WIN || (UNITY_EDITOR && !UNITY_WEBGL)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using System.IO;

public class SteamWorkshopUpload
{
    static CallResult<CreateItemResult_t> createCallRes;
    static CallResult<SubmitItemUpdateResult_t> updateCallRes;
    static CustomCharacter character;
    static UGCUpdateHandle_t updateHandle;
    static string characterFilePath;
    static int tries;
    public static bool inProgress = false;
    public static string message="";
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void SumbitToWorkshop(CustomCharacter character){
        if (character==null)
            return;
        SteamWorkshopUpload.character=character;
        characterFilePath=SaveLoad<CustomCharacter>.GetFilePath("characters",character.name+".ch");;
        if (File.Exists(characterFilePath) && character.normal!=""){
            if (!SteamManager.Initialized)
                return;

            character = SaveLoad<CustomCharacter>.LoadFullPath(characterFilePath);
            
            tries=10;
            inProgress=true;
            if (character.workshopID!=PublishedFileId_t.Invalid)
                UpdateCharacter();
            else{
                CreateCharacter();
            }
        }
    }
    public static void UpdateCharacter(){
        Debug.Log("Updating Character");
        message="Updating Character";
        updateHandle = SteamUGC.StartItemUpdate(new AppId_t(2473030),character.workshopID);
        SteamUGC.SetItemContent(updateHandle,characterFilePath);
        SteamAPICall_t handle = SteamUGC.SubmitItemUpdate(updateHandle,"Workshop Character Upload");

        updateCallRes = CallResult<SubmitItemUpdateResult_t>.Create(OnUpdateItem);
        updateCallRes.Set(handle);
    }
    public static void CreateCharacter(){
        Debug.Log("Creating New Character");
        message="Creating New Character";
        createCallRes = CallResult<CreateItemResult_t>.Create(OnCreateItem);

        SteamAPICall_t handle = SteamUGC.CreateItem(new AppId_t(2473030), EWorkshopFileType.k_EWorkshopFileTypeCommunity);
        createCallRes.Set(handle);
    }
    public static void OnCreateItem(CreateItemResult_t pCallback, bool bIOFailure){
        Debug.Log("Character Created");
        string color = pCallback.m_eResult==EResult.k_EResultOK ? "green" : "red";
        string result= pCallback.m_eResult==EResult.k_EResultOK ? "Done" : pCallback.m_eResult.ToString();
        message=$"<color={color}>{result}</color={color}>";
        if (pCallback.m_eResult==EResult.k_EResultBusy && tries>0){
            tries--;
            Delayer.DelayAction(1,()=>{
                createCallRes = CallResult<CreateItemResult_t>.Create(OnCreateItem);
                SteamAPICall_t handle = SteamUGC.CreateItem(new AppId_t(2473030), EWorkshopFileType.k_EWorkshopFileTypeCommunity);
                createCallRes.Set(handle);
            });
        }
        else if (pCallback.m_eResult==EResult.k_EResultBusy && tries==0){
            inProgress=false;
            return;
        }

        if (pCallback.m_bUserNeedsToAcceptWorkshopLegalAgreement){
            SteamFriends.ActivateGameOverlayToWebPage("https://steamcommunity.com/sharedFiles/workshoplegalagreement");
            inProgress=false;
            return;
        }
        if (pCallback.m_eResult!=EResult.k_EResultOK)
        {   
            inProgress=false;
            return;
        }

        character.workshopID=pCallback.m_nPublishedFileId;
        CharacterEditor.StaticSaveCharacter(character);
        updateHandle = SteamUGC.StartItemUpdate(new AppId_t(2473030),character.workshopID);

        SteamUGC.SetItemTitle(updateHandle,character.name);
        SteamUGC.SetItemDescription(updateHandle,"Test Description");


        string tempImagePath=SaveLoad<Texture2D>.GetFilePath("temp","preview.png");
        File.WriteAllBytes(tempImagePath,character.GetSprite(Expression.Normal).texture.EncodeToPNG());
        SteamUGC.SetItemPreview(updateHandle,tempImagePath);

        SteamUGC.SetItemContent(updateHandle,characterFilePath);

        SteamUGC.SetItemVisibility(updateHandle,Steamworks.ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPrivate);
        SteamAPICall_t handle = SteamUGC.SubmitItemUpdate(updateHandle,"Workshop Character Upload");
        
        updateCallRes = CallResult<SubmitItemUpdateResult_t>.Create(OnUpdateItem);
        updateCallRes.Set(handle);
            
    }
   
    public static void OnUpdateItem(SubmitItemUpdateResult_t pCallback, bool bIOFailure){
        Debug.Log("Character Updated");
        string color = pCallback.m_eResult==EResult.k_EResultOK ? "green" : "red";
        string result= pCallback.m_eResult==EResult.k_EResultOK ? "Done" : pCallback.m_eResult.ToString();
        message=$"<color={color}>{result}</color={color}>";
        
        if (pCallback.m_eResult==EResult.k_EResultOK){
            SteamFriends.ActivateGameOverlayToWebPage("steam://url/CommunityFilePage/"+character.workshopID);
        }
        inProgress=false;
    }
}
#endif
