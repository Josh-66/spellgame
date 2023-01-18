using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public static bool submittable{
        get{return CustomerController.instance.arrived && CustomerController.isEntry && !DialogueController.playing;}
    }

    // Start is called before the first frame update
    void Awake()
    {
        Glyph.LoadGlyphs();
        instance = this;
    }
    void Start(){
        AudioUtility.SetAllVolumes();
        


        if (loadData)
            SaveData.Load();
        else{
            evaluations=new List<Evaluation>();
            customerQueue=new List<CustomerSpawnRequest>();
            customerQueue.Add(new CustomerSpawnRequest("Florist"));
            customerQueue.Add(new CustomerSpawnRequest("Mayor"));
            customerQueue.Add(new CustomerSpawnRequest("Prankster"));
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
        if (StampPaperController.stampTexture!=null){
            ManageCustomer();
        }
        else if (CameraController.instance.arrived){
            if (!StampPaperController.isOpen)
                StampPaperController.OpenPaper();
        }
    }
    void ManageCustomer(){
        if (!CustomerController.active && customerQueue.Count>0)
        {
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
        SaveData.Save();
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
                hoveredObject=null;
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
}
