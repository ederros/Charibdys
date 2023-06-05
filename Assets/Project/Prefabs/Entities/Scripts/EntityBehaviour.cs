using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Mirror;

public class EntityBehaviour : NetworkBehaviour
{
    public enum events{
        onChangedTile
    }
    
    private Observer<events> eventHandler;
    public Observer<events> EventHandler{
        get{
            if(eventHandler == null) eventHandler = new Observer<events>();
            return eventHandler;
        }
    }
    static EntityBehaviour choosen = null;
    public static EntityBehaviour Choosen{
        get{
            return choosen;
        }
        set{
            if(choosen!=null) OnUnChoose();
            choosen = value;
            if(choosen!=null) OnChoose();
        }
    }
    public bool IsChoosen{
        get{
            return choosen == this;
        }
    }
    
    [SerializeField]
    GameValue hp = new GameValue(100);

    public Tilemap myTileMap;
    private Vector2Int tileCoord = Vector2Int.zero;
    public Vector2Int TileCoord{
        get{
            return tileCoord;
        }
        set{
            tileCoord = value;
            MyTile = myTileMap.GetTile<FloorTile>((Vector3Int)tileCoord);
        }
    }
    private FloorTile myTile;
    public FloorTile MyTile{
        get{
            return myTile;
        }
        set{
            if(myTile == value) return;
            if(myTile != null) myTile.OnEntityUnStep(this);
            myTile = value;
            myTile.OnEntityStep(this);
        }
    }
    public float armor;
    public int sightRadius;
    public int turnsPerRound;
    public int currentTurns;

    void OnMouseDown()
    {
       Choosen = this;
    }
    private static void OnChoose(){
        SpriteRenderer sr = choosen.GetComponent<SpriteRenderer>();
        sr.color = Color.blue;
        sr.sortingOrder++;
    }
    private static void OnUnChoose(){
        SpriteRenderer sr = choosen.GetComponent<SpriteRenderer>();
        sr.color = Color.white;
        sr.sortingOrder--;
    }

    public bool Move(int direction){
        return Move(HexManager.DirToVec2(tileCoord.y,direction));
    }
    public bool Move(Vector2Int direction){
        tileCoord+=direction;
        transform.position=myTileMap.CellToWorld((Vector3Int)tileCoord);
        return true;
    }

    [ClientRpc]
    void RpcSyncMovement(Vector2Int []path){
        Debug.Log("responce");
        EntityWalker.RpcStartWalk(this,path);
    }

    [Command(requiresAuthority = false)]
    void Movement(){
        Debug.Log("command");
        Vector3Int to = myTileMap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        Collider2D check = Physics2D.OverlapCircle(myTileMap.CellToWorld(to), 0.1f);
        if(check != null&&check.GetComponent<EntityBehaviour>()!=null) return;
        Vector2Int []path = HexManager.PathFind(tileCoord,(Vector2Int)to,myTileMap);
        //(HexManager.IEPathFind(tileCoord,(Vector2Int)myTileMap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition))));
        if(path != null) {
            Debug.Log("path finded");
            RpcSyncMovement(path);
        }
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0)&&IsChoosen){
            Movement();
        }
    }
    void Awake()
    {
        TileCoord = (Vector2Int)(myTileMap.WorldToCell(transform.position));
        transform.position = myTileMap.CellToWorld((Vector3Int)TileCoord);
    }
}
