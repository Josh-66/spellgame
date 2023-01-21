using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugStuff : MonoBehaviour
{
    public Character character;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space)){
            Time.timeScale=.1f;
        }
        else{
            Time.timeScale=1f;
        }


        // if (Input.GetKeyDown(KeyCode.S)){
        //     StampPaperController.TogglePaper();
        // }
        if (Input.GetKeyDown(KeyCode.D)){
            ToolController.activeTool=Tool.Debug;
        }


        if (Input.GetKeyDown(KeyCode.K)){
            SaveData.Save();
        }
        if (Input.GetKeyDown(KeyCode.L)){
            SaveData.Load();
        }
        
    }
}
