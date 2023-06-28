using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GameValue
{
    [SerializeField]
    private float value;
    public float Value{get; private set;}
    public float maxValue{get; private set;}
    public float minValue{get; private set;}
    public UnityEvent OnValueChanged;
    public UnityEvent OnValueMax;
    public UnityEvent OnValueMin;
    public GameValue(float value){
        this.value = value;
        maxValue = value;
        minValue = 0;
    }

    public void AddValue(float addVal){
        value+=addVal;
        if(value>maxValue){
            value = maxValue;
            OnValueMax.Invoke();
        }
            
        OnValueChanged.Invoke();
    }

    public void SubValue(float addVal){
        value-=addVal;
        if(value<=minValue){
            value = maxValue;
            OnValueMin.Invoke();
        }
            

        OnValueChanged.Invoke();
    }
    
}
