using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Preferences
{
    public static Preferences instance{
        get{
            if (_instance==null)
            {
                _instance = SaveLoad<Preferences>.Load("settings","preferences");
            }    
            return _instance;
        }
    }
    private static Preferences _instance=null;
    int _gamesFinished=0;
    float _masterVol=.8f,_soundVol=.8f,_musicVol=.8f; 

    int _textSpeed=2;
    bool _validSave=false;
    public static int gamesFinished {
        get {return instance._gamesFinished;}
        set {instance._gamesFinished=value;SaveToDisk();}
    }
    public static float masterVol {
        get {return instance._masterVol;}
        set {instance._masterVol=value;SaveToDisk();}
    }
    public static float soundVol {
        get {return instance._soundVol;}
        set {instance._soundVol=value;SaveToDisk();}
    }
    public static float musicVol {
        get {return instance._musicVol;}
        set {instance._musicVol=value;SaveToDisk();}
    }
    public static int textSpeed {
        get {return instance._textSpeed;}
        set {instance._textSpeed=value;SaveToDisk();}
    }
    // Start is called before the first frame update

    static void SaveToDisk(){
        SaveLoad<Preferences>.Save(instance,"settings","preferences");
    }
}
