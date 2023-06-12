
using UnityEngine;
using System.Collections.Generic;
public static class AudioUtility{

    public static UnityEngine.Audio.AudioMixer mixer {get{return Resources.Load<UnityEngine.Audio.AudioMixer>("MainMixer");}}
    public static Dictionary<string,float> volume{get{
        if (_volume==null){
            _volume = new Dictionary<string, float>(){
                {"MasterVol",PlayerPrefs.GetFloat("MasterVol",.8f)},
                {"SoundVol",PlayerPrefs.GetFloat("SoundVol",.8f)},
                {"MusicVol",PlayerPrefs.GetFloat("MusicVol",.8f)},
            };
        }
        return _volume;
    }}
    private static Dictionary<string,float> _volume=null;

    //Channels are "MasterVol" "SoundVol" and "MusicVol"
    public static void SetChannelVolumeNormalized(string channel, float amount){
        mixer.SetFloat(channel,Mathf.Log10(amount)*20f);
        volume[channel]=amount;
    }
    public static float GetChannelVolumeNormalized(string channel){
        return volume[channel];
        // float val;

        // mixer.GetFloat(channel, out val);
        // return Mathf.Pow(10,val/20);
        // return GetChannelVolumeFromPrefs(channel);
    }
    public static void SetAllVolumes(){
        SetChannelVolumeNormalized("MasterVol",volume["MasterVol"]);
        SetChannelVolumeNormalized("SoundVol" ,volume["SoundVol"]);
        SetChannelVolumeNormalized("MusicVol" ,volume["MusicVol"]);

    }
    public static void SaveToPrefs(){
        PlayerPrefs.SetFloat("MusicVol",GetChannelVolumeNormalized("MusicVol"));
        PlayerPrefs.SetFloat("SoundVol",GetChannelVolumeNormalized("SoundVol"));
        PlayerPrefs.SetFloat("MasterVol",GetChannelVolumeNormalized("MasterVol"));
    }
    public static void PlayRand(this AudioSource source,AudioClip[] clips){
        source.clip=clips.RandomElement();
        source.Play();
    }
}   