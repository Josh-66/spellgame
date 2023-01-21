
using UnityEngine;
public static class AudioUtility{

    public static UnityEngine.Audio.AudioMixer mixer {get{return Resources.Load<UnityEngine.Audio.AudioMixer>("MainMixer");}}
    public static float masterVol,soundVol,musicVol;

    //Channels are "MasterVol" "SoundVol" and "MusicVol"
    public static void SetChannelVolumeNormalized(string channel, float amount){
        mixer.SetFloat(channel,Mathf.Log10(amount)*20f);
    }
    public static float GetChannelVolumeNormalized(string channel){
        float val;
        mixer.GetFloat(channel, out val);
        return Mathf.Pow(10,val/20);
    }
    public static void SetAllVolumes(){
        SetChannelVolumeNormalized("MasterVol",GetChannelVolumeFromPrefs("MasterVol"));
        SetChannelVolumeNormalized("SoundVol",GetChannelVolumeFromPrefs("SoundVol"));
        SetChannelVolumeNormalized("MusicVol",GetChannelVolumeFromPrefs("MusicVol"));

    }
    public static void SaveToPrefs(){
        PlayerPrefs.SetFloat("MusicVol",GetChannelVolumeNormalized("MusicVol"));
        PlayerPrefs.SetFloat("SoundVol",GetChannelVolumeNormalized("SoundVol"));
        PlayerPrefs.SetFloat("MasterVol",GetChannelVolumeNormalized("MasterVol"));
    }
    public static float GetChannelVolumeFromPrefs(string channel){
        return PlayerPrefs.GetFloat(channel,.8f);
    }
}   