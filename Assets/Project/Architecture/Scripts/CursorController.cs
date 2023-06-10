using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CursorController : MonoBehaviour
{
    public Tilemap tMap;
    void BuildPathToCursor(Vector2Int from){
        tMap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    void Update()
    {
        if(EntityBehaviour.Choosen == null) return;
    }
}
