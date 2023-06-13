#if UNITY_STANDALONE_WIN || (UNITY_EDITOR && !UNITY_WEBGL)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharacterEditorTab : MonoBehaviour
{
    public static CharacterEditorTab activeTab= null;
    public GameObject infoObj;
    public GameObject dataObj;
    // Start is called before the first frame update
    void Awake()
    {
        infoObj.SetActive(false);
        dataObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ActivateTab(){
        if (CharacterEditor.currentCharacter==null)
            return;
        DeactivateTab();
        activeTab=this;
        activeTab.infoObj.SetActive(true);
        activeTab.dataObj.SetActive(true);

    }
    public void DeactivateTab(){
        if (activeTab==null)
            return;
        activeTab.infoObj.SetActive(false);
        activeTab.dataObj.SetActive(false);
    }
}
#endif