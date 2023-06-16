using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    private Camera myCam;
    private Vector2 lastMousePos;

    void Awake()
    {
        myCam = GetComponent<Camera>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(2)){
            lastMousePos = myCam.ScreenToWorldPoint(Input.mousePosition);
        }
        if(Input.GetMouseButton(2)){
            Vector2 deltaPos = (-(Vector2)myCam.ScreenToWorldPoint(Input.mousePosition)+lastMousePos);
            lastMousePos = (Vector2)myCam.ScreenToWorldPoint(Input.mousePosition) + deltaPos;
            transform.position += (Vector3)deltaPos;
        }
    }
}
