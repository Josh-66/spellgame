using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugStuff : MonoBehaviour
{
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
        if (Input.GetKeyDown(KeyCode.V)){
            Spell s = FindObjectOfType<InkController>().spell;

            SpellEvaluationTree tree = SpellEvaluationTree.Get("Testing");
            Debug.Log(tree.EvaluateSpell(s));
        }

        if (Input.GetKeyDown(KeyCode.S)){
            StampPaperController.TogglePaper();
        }
    }
}
