using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// Saves, loads and deletes all data in the game
/// </summary>
/// <typeparam name="T"></typeparam>
public static class SaveLoad<T>
{

    /// <summary>
    /// Save data to a file (overwrite completely)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="folder"></param>
    /// <param name="file"></param>
    public static void Save(T data, string folder, string file)
    {
        // get the data path of this save data
        string dataPath = GetFilePath(folder, file);

        string jsonData = JsonUtility.ToJson(data, true);
        byte[] byteData;
        
        byteData = Encoding.ASCII.GetBytes(jsonData);

        // create the file in the path if it doesn't exist
        // if the file path or name does not exist, return the default SO
        if (!Directory.Exists(Path.GetDirectoryName(dataPath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(dataPath));
        }

        // attempt to save here data
        try
        {
            // save datahere
            File.WriteAllBytes(dataPath, byteData);
            Debug.Log("Saved");
        }
        catch (Exception e)
        {
            // write out error here
            Debug.LogError("Failed to save data: " + e.Message);
        }
    }
    
    /// <summary>
    /// Load all data at a specified file and folder location
    /// </summary>
    /// <param name="folder"></param>
    /// <param name="file"></param>
    /// <returns></returns>
    public static T Load(string folder, string file)
    {
        // get the data path of this save data
        string dataPath = GetFilePath(folder, file);

        // if the file path or name does not exist, return the default SO
        if (!Directory.Exists(Path.GetDirectoryName(dataPath)))
        {
            // Debug.LogWarning("File or path does not exist! " + dataPath);
            return default(T);
        }

        // load in the save data as byte array
        byte[] jsonDataAsBytes = null;

        try
        {
            jsonDataAsBytes = File.ReadAllBytes(dataPath);
            Debug.Log("Loaded data");
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed to load data: " + e.Message);
            return default(T);
        }

        if (jsonDataAsBytes == null)
            return default(T);

        // convert the byte array to json
        string jsonData;

        // convert the byte array to json
        jsonData = Encoding.ASCII.GetString(jsonDataAsBytes);

        // convert to the specified object type
        T returnedData = JsonUtility.FromJson<T>(jsonData);

        // return the casted json object to use
        return (T)Convert.ChangeType(returnedData, typeof(T));
    }
    public static void Delete(string folder, string file)
    {
        // get the data path of this save data
        string dataPath = GetFilePath(folder, file);

        // if the file path or name does not exist, return the default SO
        if (!Directory.Exists(Path.GetDirectoryName(dataPath)))
        {
            // Debug.LogWarning("File or path does not exist! " + dataPath);
            return;
        }

        try
        {
            File.Delete(dataPath);
            Debug.Log("Deleted data");
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed to delete data: " + e.Message);
            return;
        }

        return;

    }
    
    /// <summary>
    /// Create file path for where a file is stored on the specific platform given a folder name and file name
    /// </summary>
    /// <param name="FolderName"></param>
    /// <param name="FileName"></param>
    /// <returns></returns>
    public static List<string> GetFolderContents(string FolderName){
        string path = GetFilePath(FolderName);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        string[] files = Directory.GetFiles(path);
        for (int i = 0 ; i < files.Length ; i++){
            files[i]=Path.GetFileName(files[i]);
        }
        return new List<string>(files);
    }
    private static string GetFilePath(string FolderName, string FileName = "")
    {
        string filePath="";
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        // mac
        filePath = Path.Combine(Application.streamingAssetsPath, ("data/" + FolderName));

        if (FileName != "")
            filePath = Path.Combine(filePath, (FileName));
#elif UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        // windows
        filePath = Path.Combine(Application.persistentDataPath, ("data/" + FolderName));

        if(FileName != "")
            filePath = Path.Combine(filePath, (FileName));
#elif UNITY_ANDROID
        // android
        filePath = Path.Combine(Application.persistentDataPath, ("data/" + FolderName));

        if(FileName != "")
            filePath = Path.Combine(filePath, (FileName));
#elif UNITY_IOS
        // ios
        filePath = Path.Combine(Application.persistentDataPath, ("data/" + FolderName));

        if(FileName != "")
            filePath = Path.Combine(filePath, (FileName));
#else
        filePath = Path.Combine(Application.persistentDataPath, ("data/" + FolderName));

        if(FileName != "")
            filePath = Path.Combine(filePath, (FileName));
#endif
        return filePath;
    }
}