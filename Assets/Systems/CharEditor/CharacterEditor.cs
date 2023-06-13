#if UNITY_STANDALONE_WIN || (UNITY_EDITOR && !UNITY_WEBGL)
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CharacterEditor : MonoBehaviour
{
    public static CustomCharacter currentCharacter {get {return instance?._currentCharacter;}set{instance._currentCharacter=value;} }
    //[SerializeField]
    private CustomCharacter _currentCharacter=null;
    public static CharacterEditor instance;
    List<string> characters;

    public GameObject listNameTemplate;
    public List<GameObject> buttons = new List<GameObject>();
    public CharacterEditorTab infoTab;
    public Image lastListFrame;
    // Start is called before the first frame update
    void Awake()
    {
        instance=this;
        listNameTemplate.SetActive(false);
        currentCharacter=null;
        CharacterEditorTab.activeTab=null;
        UpdateList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateList(){
        foreach(GameObject b in buttons){
            GameObject.Destroy(b);
        }
        buttons.Clear();

        characters = SaveLoad<CustomCharacter>.GetFolderContents("characters");
        int i = 0;
        RectTransform templateTransform = listNameTemplate.transform as RectTransform;
        foreach (string s in characters)
        {
            GameObject newButton = GameObject.Instantiate<GameObject>(listNameTemplate);
            try {
                newButton.transform.SetParent(listNameTemplate.transform.parent);
                newButton.transform.localScale=Vector3.one;
                RectTransform newTransform = newButton.transform as RectTransform;
                newTransform.anchoredPosition =templateTransform.anchoredPosition+Vector2.down * templateTransform.sizeDelta.y * i;
                newTransform.sizeDelta=templateTransform.sizeDelta;
                newButton.GetComponentInChildren<TextMeshProUGUI>().text=s.Substring(0,s.Length-3);
                newButton.SetActive(true);
                buttons.Add(newButton);
                i++;
                
                if (currentCharacter!=null){
                    if (newButton.GetComponentInChildren<TextMeshProUGUI>().text==currentCharacter.name){
                        lastListFrame=newButton.transform.GetChild(1).GetComponent<Image>();
                        lastListFrame.enabled=true;
                    }
                }
            }
            catch {
                Debug.Log("here");
                GameObject.Destroy(newButton);
            }

        }
    }
    public void CreateNewCharacter(){
        CustomCharacter newChar = new CustomCharacter();
        newChar.textSound=CustomCharacter.defaultSound;
        newChar.textSoundName="default.ogg";
        characters = SaveLoad<CustomCharacter>.GetFolderContents("characters").Select(c=>c.ToLower()).ToList();
        newChar.name="NewCharacter";
        int i = 1;
        while(characters.Contains(newChar.name.ToLower()+".ch")){
            i++;
            newChar.name="NewCharacter"+i;
        }
        SaveCharacter(newChar,false);
        UpdateList();
    }
    public void UpdateCharacterName(string s){
        if (s == currentCharacter.name)
            return;
        string oldName = currentCharacter.name;
        string newName = s;
        int i = 1;
        characters = SaveLoad<CustomCharacter>.GetFolderContents("characters").Select(c=>c.ToLower()).Where(c=>c.ToLower()!=oldName.ToLower()).ToList();
  
        
        while(characters.Contains(newName.ToLower()+".ch")){
            i++;
            newName=s+i;
        }
        currentCharacter.name=newName;
        DeleteCharacter(oldName);
        SaveCharacter(currentCharacter);
        UpdateList();
        CharacterEditorInfoPanel.UpdateCharacterInfo();
    }
    public void UpdateCharacterColor(){
        currentCharacter.color = CharacterEditorInfoPanel.selectedColor;
    }
    public void SelectCharacter(Image i){
        if (currentCharacter!=null){
            SaveCharacter(currentCharacter);
        }
        if (lastListFrame!=null){
            lastListFrame.enabled=false;
        }
        try{
            LoadCharacter(i.transform.parent.GetComponentInChildren<TextMeshProUGUI>().text);
            lastListFrame=i;
            lastListFrame.enabled=true;
            infoTab.ActivateTab();
            CharacterEditorInfoPanel.UpdateCharacterInfo();
        }
        catch{
            UpdateList();
        }
    }
    

    void SaveCharacter(CustomCharacter character,bool saveTree= true){
        if (saveTree){
            character.spellEvalTree=EvaluationEditor.GetTree();
            character.dialogue=DialogueEditor.GetDialogue();
        }
        SaveLoad<CustomCharacter>.Save(character,"characters",character.name+".ch");
    }
    public static void StaticSaveCharacter(CustomCharacter character){
        SaveLoad<CustomCharacter>.Save(character,"characters",character.name+".ch");
    }
    public void SaveCharacter(){
        if (currentCharacter==null)
            return;
        currentCharacter.spellEvalTree=EvaluationEditor.GetTree();
        currentCharacter.dialogue=DialogueEditor.GetDialogue();
        SaveLoad<CustomCharacter>.Save(currentCharacter,"characters",currentCharacter.name+".ch");
    }
    void LoadCharacter(string name){
        currentCharacter=SaveLoad<CustomCharacter>.Load("characters",name+".ch");
        EvaluationEditor.LoadTree();
        DialogueEditor.LoadTree();
    }
    void DeleteCharacter(string name){
        SaveLoad<CustomCharacter>.Delete("characters",name+".ch");
    }
    public void DeleteCurrentCharacter(){
        if (currentCharacter==null)
            return;
        DeleteCharacter(currentCharacter.name);
        currentCharacter=null;
        CharacterEditorTab.activeTab.DeactivateTab();
        UpdateList();
    }

    public void TestCharacter(){
        if (currentCharacter==null)
            return;
        SaveCharacter();
        GameController.PrepareGame(GameController.GameType.test,new CustomerSpawnRequest(currentCharacter.GetCharacter(CharType.custom)));
        Utility.FadeToScene("Shop");
    }
    public void ReturnToMainMenu(){
        Utility.FadeToScene("Title");
    }
    public void UploadToWorkshop(){
        SteamWorkshopUpload.SumbitToWorkshop(currentCharacter);
    }
}
#endif