using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class ReviewAppController : MonoBehaviour,IPointerDownHandler
{
    public static List<Evaluation> evaluations;
    public ReviewController reviewTemplate;
    public RectTransform scrollArea;
    public TextMeshProUGUI scoreText;
    public float verticalSpacing;

    public GameObject morningVersion;
    // Start is called before the first frame update
    void Awake()
    {
        if (BedroomController.morning){
            gameObject.SetActive(false);
            morningVersion.SetActive(true);
            return;
        }
        if (evaluations==null){
            evaluations=new List<Evaluation>();
            evaluations.Add(new Evaluation("",4,"Great",false){name = "Florist"});
            evaluations.Add(new Evaluation("",3.5f,"Just Okay",false){name = "Mayor"});
            evaluations.Add(new Evaluation("",5,"This changed my life",false){name = "Prankster"});
            evaluations.Add(new Evaluation("",1,"Mehh",false){name = "Error"});
        }
        if (evaluations!=null){
            UpdateReviews();
        }
    }
    public void UpdateReviews(){
        foreach(Transform t in scrollArea)
            if (t.GetComponent<ReviewController>()!=null)
                GameObject.Destroy(t.gameObject);

        float topY = reviewTemplate.rectTransform.anchoredPosition.y;
        int evalNum = 0;
        float totalScore=0;
        foreach(Evaluation eval in evaluations){
            ReviewController newReview = GameObject.Instantiate<GameObject>(reviewTemplate.gameObject).GetComponent<ReviewController>();
            newReview.transform.SetParent(scrollArea);
            newReview.rectTransform.anchoredPosition=new Vector2(0,topY-evalNum*newReview.rectTransform.sizeDelta.y);
            newReview.evaluation=eval;
            newReview.rectTransform.localScale=Vector3.one;
            Debug.Log("----");
            Debug.Log(eval.name);
            Debug.Log(eval.type);
            Character reviewer = CharacterStorage.GetCharacter(eval.name,eval.type);
            if (reviewer!=null)
                newReview.reviewer.sprite=reviewer.profileIcon;

            totalScore+=eval.starRating;

            newReview.gameObject.SetActive(true);
            evalNum++;
        }
        float averageScore = totalScore/evaluations.Count;
        averageScore= Mathf.Round(averageScore*10)/10f;
        scoreText.text=$"{averageScore}/5 Stars";

        scrollArea.sizeDelta= new Vector2(scrollArea.sizeDelta.x,Mathf.Max(750,topY+(evalNum+2)*reviewTemplate.rectTransform.sizeDelta.y));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        
    }
}
