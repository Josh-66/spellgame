using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class GameSettingsWindow : WindowController
{
    public static GameSettingsWindow instance;
    public GameObject listTemplate,baseScrollView,customScrollView,workshopScrollView;
    public GameObject customDisabledText,workshopDisabledText;
    public List<string> baseCharacters;
    public List<GameSettingsListCharacter> baseListCharacters = new List<GameSettingsListCharacter>();
    public RectTransform baseListContent;
    public RectTransform customListContent,workshopListContent;
    
    #if UNITY_STANDALONE_WIN || (UNITY_EDITOR && !UNITY_WEBGL)
    public List<string> customCharacters,workshopCharacters;
    public List<GameSettingsListCharacter> customListCharacters = new List<GameSettingsListCharacter>();
    public List<GameSettingsListCharacter> workshopListCharacters = new List<GameSettingsListCharacter>();
    #endif
    public Toggle randomizeToggle;
    public bool randomizeOrder{get;set;}

    public GameSettingsData lastData;
    // Start is called before the first frame update
    public override void Activate()
    {
        instance=this;
        gameObject.SetActive(false);
        Delayer.DelayAction(.001f,PopulateAllLists);
    }
    void PopulateAllLists(){
        lastData=GameSettingsData.GetLast();
        randomizeToggle.isOn=lastData.randomize;
        baseCharacters=CharacterStorage.baseCharacterNames;
        PopulateList(baseListContent,baseCharacters,CharType.baseGame);

        #if UNITY_STANDALONE_WIN || (UNITY_EDITOR && !UNITY_WEBGL)
        customCharacters = new List<string>();
        customCharacters = SaveLoad<CustomCharacter>
            .GetFolderContents("characters")
            .Where(n=>System.IO.Path.GetExtension(n)==".ch")
            .Select(n=>System.IO.Path.GetFileNameWithoutExtension(n))
            .ToList();
        
        workshopCharacters = SaveLoad<CustomCharacter>
            .GetFolderContents("workshopcharacters")
            .Where(n=>System.IO.Path.GetExtension(n)==".wch")
            .Select(n=>System.IO.Path.GetFileNameWithoutExtension(n))
            .ToList();

        PopulateList(workshopListContent,workshopCharacters,CharType.workshop);
        PopulateList(customListContent,customCharacters,CharType.custom);
        #else
        workshopDisabledText.SetActive(true);
        customDisabledText.SetActive(true);
        #endif
        Base();
    }

    public void PopulateList(RectTransform content, List<string> characters,CharType type){
        
        #if UNITY_STANDALONE_WIN || (UNITY_EDITOR && !UNITY_WEBGL)
        var targetList = type switch{
            CharType.custom=>customListCharacters,
            CharType.workshop=>workshopListCharacters,
            _=>baseListCharacters
        };
        #else
        var targetList=baseListCharacters;
        #endif

        #if UNITY_STANDALONE_WIN || (UNITY_EDITOR && !UNITY_WEBGL)
        List<string> settingsList = type switch{
            CharType.custom=>lastData.customCharacters,
            CharType.workshop=>lastData.workshopCharacters,
            _=>lastData.baseCharacters
        };
        #else 
        List<string> settingsList=baseCharacters;
        #endif

        foreach(string n in characters){
            Character c = CharacterStorage.GetCharacter(n,type);
            if (c==null)
                continue;
            GameSettingsListCharacter gslc = Instantiate(listTemplate).GetComponent<GameSettingsListCharacter>();
            gslc.transform.SetParent(content);
            (gslc.transform as RectTransform).anchoredPosition3D=Vector3.zero;
            gslc.transform.localScale=Vector3.one;
            bool on = settingsList.Contains(c.name);
            #if UNITY_STANDALONE_WIN || (UNITY_EDITOR && !UNITY_WEBGL)
                if (type==CharType.workshop)
                    on=settingsList.Contains(n);
            #endif
            gslc.SetInfo(c,on);
            targetList.Add(gslc);
            if (type==CharType.workshop){
                gslc.workshopID=n;
            }
        }
    }

    public void Exit(){
        gameObject.SetActive(false);
    }
    public void StartGame(){
        GameSettingsData data = new GameSettingsData();
        List<CustomerSpawnRequest> csrs = new List<CustomerSpawnRequest>();
        foreach(GameSettingsListCharacter c in baseListCharacters){
            if (c.toggle.isOn){
                data.baseCharacters.Add(c.nameText.text);
                csrs.Add(new CustomerSpawnRequest(c.nameText.text,CharType.baseGame));
            }
        }
        #if UNITY_STANDALONE_WIN || (UNITY_EDITOR && !UNITY_WEBGL)
        foreach(GameSettingsListCharacter c in customListCharacters){
            if (c.toggle.isOn){
                data.customCharacters.Add(c.nameText.text);
                csrs.Add(new CustomerSpawnRequest(c.nameText.text,CharType.custom));
            }
        }
        foreach(GameSettingsListCharacter c in workshopListCharacters){
            if (c.toggle.isOn){
                data.workshopCharacters.Add(c.workshopID);
                csrs.Add(new CustomerSpawnRequest(c.workshopID,CharType.workshop));
            }
        }
        #endif

        if (randomizeOrder){
            csrs = csrs.OrderBy(i=>Random.value).ToList();
            data.randomize=true;
        }
        data.SetLast();
        Debug.Log("randomizing...");
        GameController.PrepareGame(GameController.GameType.start,csrs.ToArray());
        BedroomController.morning=true;
        Utility.FadeToScene("Bedroom");
    }
    public void Base(){
        baseScrollView.SetActive(true);
        customScrollView.SetActive(false);
        workshopScrollView.SetActive(false);
    }
    public void Custom(){
        customScrollView.SetActive(true);
        baseScrollView.SetActive(false);
        workshopScrollView.SetActive(false);

    }
    public void Workshop(){
        customScrollView.SetActive(false);
        baseScrollView.SetActive(false);
        workshopScrollView.SetActive(true);

    }
    public void SelectAll(){
        if (baseScrollView.activeSelf){
            foreach (GameSettingsListCharacter c in baseListCharacters){
                c.toggle.isOn=true;
            }
        }
        #if UNITY_STANDALONE_WIN || (UNITY_EDITOR && !UNITY_WEBGL)
        if (customScrollView.activeSelf){
            foreach (GameSettingsListCharacter c in customListCharacters){
                c.toggle.isOn=true;
            }
        }
        if (workshopScrollView.activeSelf){
            foreach (GameSettingsListCharacter c in workshopListCharacters){
                c.toggle.isOn=true;
            }
        }
        #endif
    }
    public void ClearAll(){
        if (baseScrollView.activeSelf){
            foreach (GameSettingsListCharacter c in baseListCharacters){
                c.toggle.isOn=false;
            }
        }
        #if UNITY_STANDALONE_WIN || (UNITY_EDITOR && !UNITY_WEBGL)
        if (customScrollView.activeSelf){
            foreach (GameSettingsListCharacter c in customListCharacters){
                c.toggle.isOn=false;
            }
        }
        if (workshopScrollView.activeSelf){
            foreach (GameSettingsListCharacter c in workshopListCharacters){
                c.toggle.isOn=false;
            }
        }
        #endif
    }
}

[System.Serializable]
public class GameSettingsData{
    public List <string> baseCharacters = new List<string>();
    public bool randomize = false;
    #if UNITY_STANDALONE_WIN || (UNITY_EDITOR && !UNITY_WEBGL)
    public List <string> customCharacters = new List<string>();
    public List <string> workshopCharacters = new List<string>();
    #endif
    public static GameSettingsData GetLast(){
        try {
            GameSettingsData data = SaveLoad<GameSettingsData>.Load("settings","gameSettings");
            if (data==null)
                return new GameSettingsData(){baseCharacters=CharacterStorage.baseCharacterNames};
            return data;
        }
        catch{
            return new GameSettingsData(){baseCharacters=CharacterStorage.baseCharacterNames};
        }
    }
    public void SetLast(){
        SaveLoad<GameSettingsData>.Save(this,"settings","gameSettings");
    }
}
