using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSetter : MonoBehaviour
{
    public Material stroke;
    public Material outlined,regular;
    void Awake()
    {
        DeskObject.outlined=outlined;        
        DeskObject.regular=regular;
    }

}