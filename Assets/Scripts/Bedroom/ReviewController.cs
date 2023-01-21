using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class ReviewController : MonoBehaviour
{
    public Image[] stars;
    public Sprite filled,empty;
    public Image reviewer;
    public TextMeshProUGUI text;
    public RectTransform rectTransform {get{return transform as RectTransform;}}

    public Evaluation evaluation{set{
        text.text = value.review;
        
        for(int i = 0 ; i < 5 ; i++){
            stars[i].fillAmount = value.starRating-i;
        }
    }}
}