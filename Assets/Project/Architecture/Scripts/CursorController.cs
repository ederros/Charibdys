using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public class CursorController : MonoBehaviour
{


    [SerializeField]
    UnityEvent OnNewTile;
    [SerializeField]
    Transform pathPointsContainer;
    [SerializeField]
    GameObject truePathPoint;

    [SerializeField]
    GameObject falsePathPoint;

    public static Vector2Int[] path {
        get;
        private set;
    }
    public static float [] costs {
        get;
        private set;
    }
    public Tilemap tMap;
    private Vector2Int lastTile = new Vector2Int();
    void BuildPathToCursor(Vector2Int from){
        Vector2Int to = (Vector2Int)tMap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        Vector2Int[] path = HexManager.pathFinder.PathFind(from, to, tMap);
        if(path == null){
            CursorController.path = null;
            return;
        }

        costs = HexManager.pathFinder.PathCosts(path, tMap);
        Vector2Int[] truePath = HexManager.pathFinder.TrimPathByCost(path, costs, EntityBehaviour.Choosen.CurrentTurns);
        int i;
        CursorController.path = truePath;
        for(i = 1;i<truePath.Length;i++){
            Instantiate(truePathPoint, tMap.CellToWorld((Vector3Int)path[i]),Quaternion.identity,pathPointsContainer);
        }

        for(;i<path.Length;i++){
            Instantiate(falsePathPoint, tMap.CellToWorld((Vector3Int)path[i]),Quaternion.identity,pathPointsContainer);
        }
    }
    void DestroyChilds(){
        foreach(Transform t in pathPointsContainer){
            Destroy(t.gameObject);
        }
    }
    void Update()
    {
        
        if(PlayerInstanceBehaviour.myInstance == null||!PlayerInstanceBehaviour.myInstance.IsMyTurn) return;
        if(Input.GetMouseButtonDown(0)) lastTile = Vector2Int.zero;
        if(EntityBehaviour.Choosen == null||EntityWalker.IsWalking) return;
        if(lastTile == (Vector2Int)tMap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition))) return;
        DestroyChilds();
        BuildPathToCursor(EntityBehaviour.Choosen.TileCoord);
        lastTile = (Vector2Int)tMap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        OnNewTile.Invoke();

    }

    bool isFirstStep = true;

    void OnWalkerReach(){
        if(!EntityWalker.walkingEntity.CheckAffiliation()) return;
        DestroyChilds();
        isFirstStep = true;
    }

    void OnWalkerStep(){
        if(!EntityWalker.walkingEntity.CheckAffiliation()) return;
        if(isFirstStep) isFirstStep = false;
        else{ 
            EntityBehaviour tempEnt = EntityWalker.walkingEntity;
            tempEnt.TrySpendTurns(tempEnt.myTileMap.GetTile<FloorTile>((Vector3Int)tempEnt.TileCoord).turnsRequired);
            Destroy(pathPointsContainer.GetChild(0).gameObject);
        }
    }

    void Awake()
    {
        EntityWalker.OnWalkerReach.AddListener(OnWalkerReach);
        EntityWalker.OnWalkerStep.AddListener(OnWalkerStep);
    }
}
