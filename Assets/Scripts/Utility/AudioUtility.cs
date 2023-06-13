
using UnityEngine;
using System.Collections.Generic;
public static class AudioUtility{

    public static UnityEngine.Audio.AudioMixer mixer {get{return Resources.Load<UnityEngine.Audio.AudioMixer>("MainMixer");}}


    //Channels are "MasterVol" "SoundVol" and "MusicVol"
    public static void SetChannelVolumeNormalized(string channel, float amount){
        mixer.SetFloat(channel,Mathf.Log10(amount)*20f);
        
        if (channel=="MasterVol"){
            Preferences.masterVol=amount;
        }
        else if (channel=="SoundVol"){
            Preferences.masterVol=amount;
        }
        else if (channel=="MusicVol"){
            Preferences.masterVol=amount;
        }
    }
    public static float GetChannelVolumeNormalized(string channel) => channel switch{
        "MasterVol"=>Preferences.masterVol,
        "SoundVol" =>Preferences.soundVol,
        "MusicVol" =>Preferences.musicVol,
        _=>throw new System.ArgumentException("Invalid sound channel: "+channel),
    };
    public static void SetAllVolumes(){
        SetChannelVolumeNormalized("MasterVol",Preferences.masterVol);
        SetChannelVolumeNormalized("SoundVol" ,Preferences.soundVol);
        SetChannelVolumeNormalized("MusicVol" ,Preferences.musicVol);

    }
    public static void PlayRand(this AudioSource source,AudioClip[] clips){
        source.clip=clips.RandomElement();
        source.Play();
    }
}   