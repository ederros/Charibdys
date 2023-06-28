using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public class CursorController : MonoBehaviour
{
    private static CursorController instance;
    public static CursorController Instance{
        get{
            return instance;
        }
    }

    [SerializeField]
    UnityEvent OnNewTile;
    public Transform pathPointsContainer;
    [SerializeField]
    GameObject truePathPoint;

    [SerializeField]
    GameObject falsePathPoint;

    public static EntityCommand[] cmdSequence {
        get;
        private set;
    }
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
        if(path == null||path.Length==0){
            CursorController.cmdSequence = null;
            CursorController.path = null;
            return;
        }
        EntityCommand[] sequence = PathFinder.PathToCommands(path,EntityBehaviour.Choosen).ToArray();
        costs = HexManager.pathFinder.PathCosts(sequence, tMap);
        EntityCommand[] trueCmdSequence = HexManager.pathFinder.TrimSequenceByCost(sequence, costs, EntityBehaviour.Choosen.CurrentTurns);
        CursorController.path = HexManager.pathFinder.TrimSequenceByCost(path, costs, EntityBehaviour.Choosen.CurrentTurns);
        int i;
        CursorController.cmdSequence = trueCmdSequence;
        for(i = 0;i<trueCmdSequence.Length;i++){
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
        if(EntityBehaviour.Choosen == null||CommandsInvoker.IsWalking) return;
        if(lastTile == (Vector2Int)tMap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition))) return;
        DestroyChilds();
        BuildPathToCursor(EntityBehaviour.Choosen.TileCoord);
        lastTile = (Vector2Int)tMap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        OnNewTile.Invoke();

    }

    bool isFirstStep = true;

    void OnWalkerReach(){
        if(!CommandsInvoker.walkingEntity.CheckAffiliation()) return;
        DestroyChilds();
        isFirstStep = true;
    }


    void Awake()
    {
        instance = this;
        CommandsInvoker.OnCommandsCompleted.AddListener(OnWalkerReach);
        //CommandsInvoker.OnNextCommand.AddListener(OnWalkerStep);
    }
}
