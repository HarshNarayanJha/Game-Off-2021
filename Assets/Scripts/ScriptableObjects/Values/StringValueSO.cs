using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStringValue", menuName = "Values/String Value")]
public class StringValueSO : ValuesBaseSO
{
    [TextArea(10, 20)]
    [SerializeField] private string value;

    public string Value
    { 
        get => value; 
    }
}
