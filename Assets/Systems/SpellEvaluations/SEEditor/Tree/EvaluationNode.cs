using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEditor;
using System.Linq;
public class EvaluationNode : SENode
{
    public string evaluationKey;
    public string starRating;
    VisualElement customDataContainer;


    public override Color GetColor()
    {
        return Color.magenta;
    }

    public override string ToString()
    {
        return $"{starRating} stars, key: {evaluationKey}";
    }

}