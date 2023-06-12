using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public static class Utility{
    public static Color ColorFromHex(int color){
        float r = color>>16 & 0xFF;
        float g = color>>8 & 0xFF;
        float b = color & 0xFF;

        return new Color(r/255f,g/255f,b/255f);
    }
    public static T RandomElement<T>(this T[] arr){
        return arr[Random.Range(0,arr.Length)];
    }


    public static bool FadeToScene(string s){

        if (BlackFade.instance.OnReachTarg!=null) //Dont allow multiple fades
            return false;
        AsyncOperation ao = SceneManager.LoadSceneAsync(s);
        ao.allowSceneActivation=false;

        BlackFade.FadeInAndAcion(()=>{ao.allowSceneActivation=true;});

        return true;
    }

}