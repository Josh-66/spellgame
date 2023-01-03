using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Prefabs{
    public static GameObject Load(string path){
        return GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(path));
    }
}