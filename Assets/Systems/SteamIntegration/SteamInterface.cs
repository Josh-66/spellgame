using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamInterface : MonoBehaviour
{
    #if UNITY_STANDALONE_WIN || (UNITY_EDITOR && !UNITY_WEBGL)
    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<SteamManager>();
        Steamworks.SteamUserStats.RequestCurrentStats();
        SteamWorkshopDownload.InitializeWorkshopSubscriptions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endif
}
