using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class FloatReference
{
    [SerializeField] public bool UseConstant = false;
    [SerializeField] public float ConstantValue;
    [SerializeField] public FloatVariable Variable;

    //public bool UseConstant { get; set; }
    //public float ConstantValue { get; set; }
    //public FloatVariable Variable { get; set; }

    public float Value
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
    }
}