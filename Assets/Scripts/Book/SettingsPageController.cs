using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPageController : MonoBehaviour
{
    public Slider musicSlider,soundSlider;
    public ToggleGroup textSpeedToggle;
    public Toggle slow,med,fast;
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
    public void UpdateTextSpeed(int speed){
        TextSpeed.textSpeed=speed;
    }
    public void SetToggles(){
        Toggle activeSpeedToggle = TextSpeed.textSpeed switch{
            1=>slow,
            2=>med,
            4=>fast,
            _=>med
        };
        activeSpeedToggle.isOn=true;
    }
    public void Activate(){
        SetToggles();
        SetSliders();
    }
    void Update(){

    }
}
