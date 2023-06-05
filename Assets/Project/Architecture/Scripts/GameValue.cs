using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameValue : MonoBehaviour
{
    enum events{
        OnValueChanged
    }

    [SerializeField]
    public float value{get; private set;}
    public float maxValue{get; private set;}
    public float minValue{get; private set;}

    public GameValue(float value){
        this.value = value;
        maxValue = value;
        minValue = 0;
    }

    
}
