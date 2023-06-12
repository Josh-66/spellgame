using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public Sprite testStamp;
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
    public static float totalNumberOfCustomers=3;
    public static float numberOfCustomers=0;
    public static List<CustomerSpawnRequest> customerQueue;
    public static List<Evaluation> evaluations;
    public GameObject endTestButton;
    public float customerSpawnDelay=5f;

    public bool clickablesActive=true;
    public bool cameraArrived {get{return CameraController.instance.arrived;}}

    bool savedAlready=false;
    public bool doCustomerSpawning=false;
    public static bool submittable{
        get{
            return CustomerController.instance.arrived && CustomerController.isEntry && !DialogueController.playing;
        }
    }

    public static bool playOpening=false;

    public DaylightController daylightController;    

    // Start is called before the first frame update
    void Awake()
    {
        Glyph.LoadGlyphs();
        instance = this;
    }
    void Start(){
        AudioUtility.SetAllVolumes();
        


        if (gameType==GameType.load){
            daylightController.targetTime=numberOfCustomers/totalNumberOfCustomers;
            daylightController.time=numberOfCustomers/totalNumberOfCustomers;
            SaveData.Load();//Sets Evaluations and CustomerQueue
            GameController.numberOfCustomers=GameController.evaluations.Count;
            Delayer.DelayAction(3,()=>{
                doCustomerSpawning=true;
                DeskController.instance.ActivateAllObjects();
                MusicPlayer.Start();
            });    
        }
        else if (gameType==GameType.test){
            
            StampPaperController.stampTexture=testStamp.texture;
            StampPaperController.stampSprite=testStamp;
            StampPaperController.instance.UpdatePreview();
            
            doCustomerSpawning=true;    
            DeskController.instance.ActivateAllObjects();
            MusicPlayer.Start();

        }
        
        endTestButton.SetActive(gameType==GameType.test);
        
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
            if (!savedAlready && gameType!=GameType.test){
                SaveData.Save();
                savedAlready=true;
            }
            if (gameType==GameType.test)
                customerSpawnDelay=0;
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
                    #if UNITY_STANDALONE_WIN || UNITY_EDITOR
                    if (gameType==GameType.test){
                        ReviewAppController.evaluations=evaluations;
                        PhoneController.instance.Open();
                        PhoneController.instance.UpdateReviewApp();
                        customerQueue.Add(new CustomerSpawnRequest(evaluations[0].name));
                        evaluations= new List<Evaluation>();
                        return;
                    }
                    #endif
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
        if (!CustomerController.instance.arrived)
            return;
        customerQueue.RemoveAt(0);
        CustomerController.instance.arrived=false;
        if(!fromComplaint){
            DialogueController.PlayDialogue(CustomerController.instance.character,CustomerController.instance.character.dialogue.exit);
            DialogueController.OnComplete = ()=>{
                CustomerController.instance.ExitDialogueDone();
                numberOfCustomers++;
                daylightController.targetTime=numberOfCustomers/totalNumberOfCustomers;
            };

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
        if (!CustomerController.instance.arrived)
            return;
        Evaluation eval = CustomerController.instance.character.GetEvaluation(s);
        eval.name=CustomerController.instance.character.name;
        evaluations.Add(eval);
        LeaveCustomer();
        if (eval.returns){
            customerQueue.Add(new CustomerSpawnRequest(CustomerController.instance.character,eval.evaluationKey));
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
                            Utility.FadeToScene("Bedroom");
                            BedroomController.morning=false;
                            ReviewAppController.evaluations=evaluations;
                        });   
                    };
                    
                });
            });
        });
        
    }
    public void EndTestMode(){
        #if UNITY_STANDALONE_WIN || UNITY_EDITOR
        Utility.FadeToScene("CharacterEditor");
        #endif
    }

    public void Opening(){
        MusicPlayer.Stop();
        Delayer.DelayAction(3.5f,()=>{
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
    
    public enum GameType{
        start,
        load,
        test
    }
    public static GameType gameType;
    public static void PrepareGame(GameType gameType, params CustomerSpawnRequest[] customerSpawnRequests){
        GameController.gameType=gameType;

        if (gameType==GameType.load){
            //Load is called in GC Start
            GameController.playOpening=false;
        }
        else if (gameType==GameType.start){
            GameController.evaluations=new List<Evaluation>();
            GameController.customerQueue=new List<CustomerSpawnRequest>();
            GameController.customerQueue.AddRange(customerSpawnRequests);
            GameController.playOpening=true;

            GameController.numberOfCustomers=0;
            GameController.totalNumberOfCustomers=customerSpawnRequests.Length;
            StampPaperController.stampTexture=null;
        }
        else if (gameType==GameType.test){
            
            GameController.evaluations=new List<Evaluation>();
            GameController.customerQueue=new List<CustomerSpawnRequest>();
            GameController.customerQueue.AddRange(customerSpawnRequests);
            GameController.playOpening=false;

            GameController.numberOfCustomers=0;
            GameController.totalNumberOfCustomers=customerSpawnRequests.Length;
        }
    }
    public static void PrepareBaseGame(){
        List<CustomerSpawnRequest> csrs = new List<CustomerSpawnRequest>();
        foreach (string s in CharacterStorage.baseCharacterNames){
            csrs.Add (new CustomerSpawnRequest(s));
        }
        PrepareGame(GameType.start,csrs.ToArray());
    }
    
}
