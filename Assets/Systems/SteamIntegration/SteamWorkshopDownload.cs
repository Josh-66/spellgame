
#if UNITY_STANDALONE_WIN || (UNITY_EDITOR && !UNITY_WEBGL)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using System.IO;

public class SteamWorkshopDownload
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void InitializeWorkshopSubscriptions(){
        if (!SteamManager.Initialized)
            return;

        string worskhopPersistentPath = SaveLoad<CustomCharacter>.GetFilePath("workshopcharacters");
        foreach (string p in Directory.GetFiles(worskhopPersistentPath)){
            File.Delete(p);
        }

        var nItems = SteamUGC.GetNumSubscribedItems();
        if (nItems>0){
            PublishedFileId_t[] fileIds = new PublishedFileId_t[nItems];
            SteamUGC.GetSubscribedItems(fileIds,nItems);

            foreach (var id in fileIds){
                SteamUGC.GetItemInstallInfo(id,out ulong SizeOnDisk, out string Folder, 1024, out uint punTimeStamp);
                string[] path = Directory.GetFiles(Folder);
                if (path==null)
                    return;
                string filename = Path.GetFileName(path[0]);
                string fullPath=path[0];
                string destPath = Path.Join(worskhopPersistentPath,id.m_PublishedFileId.ToString()+".wch");
                File.Copy(fullPath,destPath);
                
            }
        }
    }
    
}
#endif
