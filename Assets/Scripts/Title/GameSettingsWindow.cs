using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class GameSettingsWindow : MonoBehaviour
{
    public static GameSettingsWindow instance;
    public GameObject listTemplate,baseScrollView,customScrollView;
    public List<string> baseCharacters,customCharacters;
    public RectTransform baseListContent,customListContent;
    public List<GameSettingsListCharacter> baseListCharacters = new List<GameSettingsListCharacter>();
    public List<GameSettingsListCharacter> customListCharacters = new List<GameSettingsListCharacter>();

    public GameSettingsData lastData;
    // Start is called before the first frame update
    void Awake()
    {
        instance=this;
        lastData=GameSettingsData.GetLast();
        baseCharacters=CharacterStorage.baseCharacterNames;
        PopulateList(baseListContent,baseCharacters,false);

        customCharacters = new List<string>();
        #if UNITY_EDITOR || UNITY_STANDALONE_WIN
        customCharacters = SaveLoad<CustomCharacter>
            .GetFolderContents("characters")
            .Where(n=>System.IO.Path.GetExtension(n)==".ch")
            .Select(n=>System.IO.Path.GetFileNameWithoutExtension(n))
            .ToList();
        
        PopulateList(customListContent,customCharacters,true);
        #endif
        customScrollView.SetActive(false);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PopulateList(RectTransform content, List<string> characters,bool custom){
        List<GameSettingsListCharacter> targetList = custom ? customListCharacters : baseListCharacters;
        List<string> settingsList = custom ? lastData.customCharacters : lastData.baseCharacters;
        foreach(string n in characters){
            Character c = CharacterStorage.GetCharacter(n);
            if (c==null)
                continue;
            GameSettingsListCharacter gslc = Instantiate(listTemplate).GetComponent<GameSettingsListCharacter>();
            gslc.transform.SetParent(content);
            gslc.transform.localScale=Vector3.one;
            bool on = settingsList.Contains(c.name);
            gslc.SetInfo(c,on);
            targetList.Add(gslc);
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
                csrs.Add(new CustomerSpawnRequest(c.nameText.text));
            }
        }
        foreach(GameSettingsListCharacter c in customListCharacters){
            if (c.toggle.isOn){
                data.customCharacters.Add(c.nameText.text);
                csrs.Add(new CustomerSpawnRequest(c.nameText.text));
            }
        }
        data.SetLast();
        GameController.PrepareGame(GameController.GameType.start,csrs.ToArray());
        BedroomController.morning=true;
        Utility.FadeToScene("Bedroom");
    }
    public void Base(){
        baseScrollView.SetActive(true);
        customScrollView.SetActive(false);
    }
    public void Custom(){
        customScrollView.SetActive(true);
        baseScrollView.SetActive(false);

    }
    public void SelectAll(){
        if (baseScrollView.activeSelf){
            foreach (GameSettingsListCharacter c in baseListCharacters){
                c.toggle.isOn=true;
            }
        }
        if (customScrollView.activeSelf){
            foreach (GameSettingsListCharacter c in customListCharacters){
                c.toggle.isOn=true;
            }
        }
    }
    public void ClearAll(){
        if (baseScrollView.activeSelf){
            foreach (GameSettingsListCharacter c in baseListCharacters){
                c.toggle.isOn=false;
            }
        }
        if (customScrollView.activeSelf){
            foreach (GameSettingsListCharacter c in customListCharacters){
                c.toggle.isOn=false;
            }
        }
    }
}

[System.Serializable]
public class GameSettingsData{
    public List <string> baseCharacters = new List<string>();
    public List <string> customCharacters = new List<string>();

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
