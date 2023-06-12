using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneController : WindowController
{
    public static PhoneController instance=>FindObjectOfType<PhoneController>(true);
    public static bool isOpen {get{return instance.gameObject.activeSelf;}}
    public override void Activate()
    {
    }
    public void UpdateReviewApp(){
        ReviewAppController rac = GetComponentInChildren<ReviewAppController>(true);
        rac.UpdateReviews();
    }
}
