using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPageController : MonoBehaviour
{
    public Slider musicSlider,soundSlider;
    void OnDisable() {
        AudioUtility.SaveToPrefs();
    }
    public void SetMusicVolume(float amount){
        AudioUtility.SetChannelVolumeNormalized("MusicVol",amount);
    }
    public void SetSoundVolume(float amount){
        AudioUtility.SetChannelVolumeNormalized("SoundVol",amount);
    }
    public void SetMasterVolume(float amount){
        AudioUtility.SetChannelVolumeNormalized("MasterVol",amount);
    }
    public void SetSliders(){
        musicSlider.value=AudioUtility.GetChannelVolumeNormalized("MusicVol");
        soundSlider.value=AudioUtility.GetChannelVolumeNormalized("SoundVol");
    }
    void Update(){
        if (Input.GetKeyDown(KeyCode.Backspace)){
            SetSliders();
        }
    }
}
