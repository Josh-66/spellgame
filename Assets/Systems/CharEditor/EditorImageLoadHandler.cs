#if UNITY_STANDALONE_WIN || UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SFB;
using System.IO;
public class EditorImageLoadHandler : MonoBehaviour ,IDropHandler,IPointerClickHandler
{
    public Image normal,happy,angry,special,icon;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<Image>() != null)
        {
            normal.sprite = eventData.pointerDrag.GetComponent<Image>().sprite;
        }
    }
    public void UpdateImages(){

        normal.sprite = CharacterEditor.currentCharacter.GetSprite(Expression.Normal);
        happy.sprite  = CharacterEditor.currentCharacter.GetSprite(Expression.Happy);
        angry.sprite  = CharacterEditor.currentCharacter.GetSprite(Expression.Angry);
        special.sprite= CharacterEditor.currentCharacter.GetSprite(Expression.Special);
        icon.sprite   = CharacterEditor.currentCharacter.GetSprite(Expression.Icon);

        SizeImage(normal);
        SizeImage(happy);
        SizeImage(angry);
        SizeImage(special);
        SizeImage(icon);
        
    }
    public void SizeImage(Image i){
        float size = 250;
        i.SetNativeSize();
        Vector2 sizeDelta = (i.transform as RectTransform).sizeDelta;
        if (sizeDelta.x>=sizeDelta.y){
            sizeDelta/=sizeDelta.x;
            sizeDelta*=size;
        }
        else{
            sizeDelta/=sizeDelta.y;
            sizeDelta*=size;
        }
        (i.transform as RectTransform).sizeDelta = sizeDelta;

    }
    public void OnPointerClick(PointerEventData eventData)
    {

        // Open the file browser dialog and wait for the user to select a file
        Expression expression;
        GameObject pressedObject = eventData.rawPointerPress.transform.parent.gameObject;
        if (pressedObject==normal.gameObject){
            expression=Expression.Normal;
        }
        else if (pressedObject==happy.gameObject){
            expression=Expression.Happy;
        }
        else if (pressedObject==angry.gameObject){
            expression=Expression.Angry;
        }
        else if (pressedObject==special.gameObject){
            expression=Expression.Special;
        }
        else if (pressedObject==icon.gameObject){
            expression=Expression.Icon;
        }
        else{
            return;
        }

        ExtensionFilter[] filters = new[] {
            new ExtensionFilter("Texture2D files", "png", "jpg", "jpeg", "bmp", "tga"),
            new ExtensionFilter("All files", "*"),
        };
        string[] selectedFiles = StandaloneFileBrowser.OpenFilePanel("Select "+expression+" Image", "", filters, false);

        // Check if the user selected a file
        if (selectedFiles.Length > 0)
        {
            byte[] imageData = File.ReadAllBytes(selectedFiles[0]);
            CharacterEditor.currentCharacter[expression]=imageData;
            UpdateImages();
        }
    }

}
#endif