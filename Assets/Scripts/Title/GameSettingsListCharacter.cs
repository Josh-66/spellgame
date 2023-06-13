using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameSettingsListCharacter : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI nameText;
    public Toggle toggle;
    public string workshopID="";
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Toggle(bool value){

    }
    public void SetInfo(Character c,bool on){
        icon.sprite=c.profileIcon;
        nameText.text=c.name;
        toggle.isOn=on;
        gameObject.SetActive(true);
    }
}
