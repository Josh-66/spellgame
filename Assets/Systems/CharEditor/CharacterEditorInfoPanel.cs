#if UNITY_STANDALONE_WIN || (UNITY_EDITOR && !UNITY_WEBGL)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SFB;
using System.IO;
public class CharacterEditorInfoPanel : MonoBehaviour
{
    static CharacterEditorInfoPanel instance;
    public EditorImageLoadHandler imageLoadHandler;
    public EditorWorkshopInfoPanel workshopInfoPanel;
    public TMP_InputField nameInputField;
    public GameObject infoStuff;

    public AudioSource textSoundSource;
    public TextMeshProUGUI textSoundName;
    // Start is called before the first frame update
    void Awake()
    {
        instance=this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetAudioClip(){
        ExtensionFilter[] filters = new[] {
            new ExtensionFilter("Audio files", "ogg"),
            new ExtensionFilter("All files", "*"),
        };
        string[] selectedFiles = StandaloneFileBrowser.OpenFilePanel("Select Text Sound", "", filters, false);

        // Check if the user selected a file
        if (selectedFiles.Length > 0)
        {
            // if (Path.GetExtension(selectedFiles[0])!=".ogg")
            //     return;
            CharacterEditor.currentCharacter.textSoundName = Path.GetFileName(selectedFiles[0]);
            byte[] clipBytes = File.ReadAllBytes(selectedFiles[0]);
            CharacterEditor.currentCharacter.SetTextSound(clipBytes);
            
            UpdateCharacterInfo();
        }
        
    }
    public void ResetTextSound(){
        CharacterEditor.currentCharacter.textSound = CustomCharacter.defaultSound;
        CharacterEditor.currentCharacter.textSoundName="default.ogg";
        UpdateCharacterInfo();
    }
    public void PlayAudioClip(){
        if (textSoundSource.clip!=null)
            textSoundSource.Play();
    }
    public static void UpdateCharacterInfo(){
        instance.nameInputField.text=CharacterEditor.currentCharacter.name;
        selectedColor = CharacterEditor.currentCharacter.color;
        instance.imageLoadHandler.UpdateImages();

        instance.textSoundName.text = CharacterEditor.currentCharacter.textSoundName;
        instance.textSoundSource.clip=CharacterEditor.currentCharacter.GetTextSound();

        instance.workshopInfoPanel.UpdateInfo();
    }

    public static Color selectedColor{
        get{
            return new Color(instance.redSlider.value,instance.greenSlider.value,instance.blueSlider.value);
        }
        set{
            instance.redSlider.value  =value.r;
            instance.greenSlider.value=value.g;
            instance.blueSlider.value =value.b;
        }
    }
    public Slider redSlider,greenSlider,blueSlider;
    public TextMeshProUGUI colorText;
    public Image colorSquare;
    public void UpdateColor(){
        CharacterEditor.currentCharacter.color=selectedColor;
        colorSquare.color=selectedColor;
        colorText.color=selectedColor;
        colorText.text = "0x" + ColorUtility.ToHtmlStringRGB(selectedColor);
    }
    
}
#endif