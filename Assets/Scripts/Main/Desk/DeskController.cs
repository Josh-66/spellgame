using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskController : MonoBehaviour
{
    public static DeskController instance;
    public DeskObject[] deskObjects;
    // Start is called before the first frame update
    void Awake()
    {

        instance=this;
        deskObjects=GetComponentsInChildren<DeskObject>();
        foreach(DeskObject d in deskObjects){
            d.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ActivateAllObjects(){
        foreach(DeskObject d in deskObjects){
            Delayer.DelayAction(Random.Range(1f,2.5f),()=>{
                d.gameObject.SetActive(true);
                d.CreatePoof();
            });
        }
    }
}
