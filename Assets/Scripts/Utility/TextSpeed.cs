using UnityEngine;
public static class TextSpeed{


    public static int textSpeed{get{
            if (_textSpeed==-1)
                _textSpeed=PlayerPrefs.GetInt("TextSpeed",2);
            return _textSpeed;
        }
        set{
            _textSpeed=value;
            PlayerPrefs.SetInt("TextSpeed",_textSpeed);
        }
    }
    private static int _textSpeed=-1;
}