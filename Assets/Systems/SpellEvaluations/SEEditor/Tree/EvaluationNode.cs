using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEditor;

public class EvaluationNode : SENode
{
    [TextArea(1,1)]
    public string evaluationKey;
    public bool returns;
    public float starRating;
    [TextArea(3,5)]
    public string review;
    VisualElement customDataContainer;


    public override Color GetColor()
    {
        return Color.magenta;
    }

    public override string ToString()
    {
        return $"{starRating} stars, key: {evaluationKey}";
    }

    public Evaluation GetEvaluation(){
        return new Evaluation(evaluationKey,starRating,review,returns);
    }
}
[System.Serializable]
public class Evaluation{
    public string evaluationKey;
    public float starRating;
    public string review;
    public bool returns;
    public string name;

    public Evaluation(string e,float s, string rv,bool rt ){
        evaluationKey=e;
        starRating=s;
        review=rv;
        returns=rt;
    }
}