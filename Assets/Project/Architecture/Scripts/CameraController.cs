using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    private Camera myCam;

    [SerializeField]
    private float scaleSpeed;

    [SerializeField]
    private float minSize;

    [SerializeField]
    private float maxSize;
    private Vector2 lastMousePos;

    void Awake()
    {
        myCam = GetComponent<Camera>();
    }

    void CameraMove(){
         if(Input.GetMouseButtonDown(2)){
            lastMousePos = myCam.ScreenToWorldPoint(Input.mousePosition);
        }
        if(Input.GetMouseButton(2)){
            Vector2 deltaPos = (-(Vector2)myCam.ScreenToWorldPoint(Input.mousePosition)+lastMousePos);
            lastMousePos = (Vector2)myCam.ScreenToWorldPoint(Input.mousePosition) + deltaPos;
            transform.position += (Vector3)deltaPos;
        }
    }
    void CameraResize(){
        if(Input.mouseScrollDelta.y!=0){
            myCam.orthographicSize -= Input.mouseScrollDelta.y*myCam.orthographicSize*scaleSpeed;
            myCam.orthographicSize = Mathf.Clamp( myCam.orthographicSize,minSize,maxSize);
        }
    }

    void Update()
    {
       CameraMove();
       CameraResize();
    }
}
