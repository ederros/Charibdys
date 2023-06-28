using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorHintController : MonoBehaviour
{
    RectTransform myTransform;

    [SerializeField]
    Text hint;
    public void OnTileChanged(){
        if(CursorController.cmdSequence == null){
            hint.text = "";
            return;
        }
        hint.text = CursorController.costs[CursorController.costs.Length-1].ToString();
    }
    void FixedUpdate()
    {
        myTransform.anchoredPosition = Input.mousePosition;
    }
    void Awake(){
        myTransform = GetComponent<RectTransform>();
    }
}
