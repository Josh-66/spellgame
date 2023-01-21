using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public static bool loadData = false;
    public Clickable hoveredObject{get{
        return _hoveredObject;
    }set{
        if (_hoveredObject!=null)
            _hoveredObject.OffHover();
        _hoveredObject=value;
        if (_hoveredObject!=null){
            value.OnHover();
        }
    }}
    public Clickable _hoveredObject;
    public List<CustomerSpawnRequest> customerQueue;
    public List<Evaluation> evaluations;
    public float customerSpawnDelay=5f;

    public bool clickablesActive=true;
    public bool cameraArrived {get{return CameraController.instance.arrived;}}

    bool savedAlready=false;
    public bool doCustomerSpawning=false;
    public static bool submittable{
        get{return CustomerController.instance.arrived && CustomerController.isEntry && !DialogueController.playing;}
    }

    public bool playOpening=false;

    // Start is called before the first frame update
    void Awake()
    {
        Glyph.LoadGlyphs();
        instance = this;
    }
    void Start(){
        AudioUtility.SetAllVolumes();
        


        if (loadData){
            SaveData.Load();
            playOpening=false;
            Delayer.DelayAction(3,()=>{
                doCustomerSpawning=true;
                DeskController.instance.ActivateAllObjects();
                MusicPlayer.Start();
            });    
        }
        else{
            evaluations=new List<Evaluation>();
            customerQueue=new List<CustomerSpawnRequest>();
            customerQueue.Add(new CustomerSpawnRequest("Florist"));
            customerQueue.Add(new CustomerSpawnRequest("Mayor"));
            customerQueue.Add(new CustomerSpawnRequest("Prankster"));
            playOpening=true;
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        if (cameraArrived && clickablesActive)
        {
            UpdateClicked();
            UpdateHovered();
        }
        if (doCustomerSpawning){
            ManageCustomer();
        }
        else if (CameraController.instance.arrived && playOpening){
            playOpening=false;
            Opening();
            
        }
    }
    void ManageCustomer(){
        if (!CustomerController.active && customerQueue.Count>0)
        {
            if (!savedAlready){
                SaveData.Save();
                savedAlready=true;
            }
            customerSpawnDelay-=Time.deltaTime;
            if (customerSpawnDelay<0){
                CustomerSpawnRequest csr = customerQueue[0];
                if (!csr.returning){
                    SpawnCustomer(csr.GetCharacter());
                }
                else{
                    SpawnCustomer(csr.GetCharacter(),csr.complaint);
                }
                customerSpawnDelay=Random.Range(5,7);
                savedAlready=false;
            }
        }
        if (customerQueue.Count==0){
            if (customerSpawnDelay>0){
                customerSpawnDelay-=Time.deltaTime;
                if (customerSpawnDelay<0){
                    StartExitSequence();
                }
            }
        }
    }
    void SpawnCustomer(Character c, string key="Entry"){
        CustomerController.instance.character=c;
        if (key=="Entry"){
            CustomerController.isEntry=true;
        }
        else{
            CustomerController.isEntry=false;
            CustomerController.complaint=key;
        }
        CustomerController.instance.Enter();
    }
    public void LeaveCustomer(bool fromComplaint=false){
        customerQueue.RemoveAt(0);

        if(!fromComplaint){
            DialogueController.PlayDialogue(CustomerController.instance.character,CustomerController.instance.character.dialogue.exit);
            CustomerController.instance.arrived=false;
            DialogueController.OnComplete = ()=>{CustomerController.instance.Exit();};
        }
    }
    void UpdateHovered(){
        Vector3 point = MyInput.WorldMousePos();

        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() || MyInput.clickHeld){
            hoveredObject=null;
        }
        else{
            RaycastHit2D hit = Physics2D.Raycast(point,Vector2.up,.01f,LayerMask.GetMask("Clickable"));
            if (hit){
                hoveredObject=hit.collider.GetComponent<Clickable>();
            }
            else{
                RaycastHit hit2;
                bool hit3 = Physics.Raycast(MyInput.WorldMouseRay(),out hit2, 20f,LayerMask.GetMask("Clickable"),QueryTriggerInteraction.Collide);
                if (hit3)
                {
                    hoveredObject=hit2.collider.GetComponent<Clickable>();
                }
                else{
                    hoveredObject=null;
                }
            }
        }
    }
    void UpdateClicked(){
        if (hoveredObject!=null && MyInput.click)
            hoveredObject.OnClick();
    }
    public void SubmitSpell(Spell s){
        Evaluation eval = CustomerController.instance.character.GetEvaluation(s);
        eval.name=CustomerController.instance.character.name;
        evaluations.Add(eval);
        LeaveCustomer();
        if (eval.returns){
            customerQueue.Add(new CustomerSpawnRequest(CustomerController.instance.character.name,eval.evaluationKey));
        }
    }

    public void StartExitSequence(){
        Delayer.DelayAction(1,()=>{
            OwlController.Appear(()=>{
                Delayer.DelayAction(1,()=>{
                    DialogueController.PlayDialogue(OwlController.instance.character,OwlController.EndingDialogue());
                    DialogueController.OnComplete=()=>{
                        CameraController.instance.UnArrive();
                        Delayer.DelayAction(1,()=>{
                            AsyncOperation ao = SceneManager.LoadSceneAsync("Bedroom");
                            ao.allowSceneActivation=false;
                            BlackFade.FadeInAndAcion(()=>{
                                ao.allowSceneActivation=true;
                            });
                            ReviewAppController.evaluations=evaluations;
                        });   
                    };
                    
                });
            });
        });
        
    }


    public void Opening(){
        MusicPlayer.Stop();
        Delayer.DelayAction(2,()=>{
            OwlController.Appear(()=>{
                Delayer.DelayAction(1,()=>{
                    DialogueController.PlayDialogue(OwlController.instance.character,OwlController.OpeningDialogue());
                    DialogueController.OnComplete=()=>{
                        StampPaperController.OpenPaper();
                    };
                });
            });
        });
    }
    public void DrewStamp(){
        Delayer.DelayAction(1,()=>{
            DialogueController.PlayDialogue(OwlController.instance.character,OwlController.OpeningEndDialogue());
                DialogueController.OnComplete=()=>{
                    Delayer.DelayAction(1f,()=>{
                        OwlController.Disappear(()=>{
                            DeskController.instance.ActivateAllObjects();
                            doCustomerSpawning=true;
                            MusicPlayer.Start();
                        });
                    });
                };
        });
        
        
    }
}
