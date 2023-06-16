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
    [SerializeField]
    public Color selectedColor;

    [HideInInspector]
    public Color unselectColor;
    private Observer<events> entityEventListener;
    public Observer<events> EntityEventListener{
        get{
            if(entityEventListener == null) entityEventListener = new Observer<events>();
            return entityEventListener;
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

    public static EntityBehaviour walkingEntity = null;

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
    public int affiliation;

    void OnMouseDown()
    {
        PlayerInstanceBehaviour player = PlayersManager.Instance.GetPlayer(affiliation);
        if(player!=null&&player.isLocalPlayer)
            Choosen = this;
    }
    private static void OnChoose(){
        SpriteRenderer sr = choosen.GetComponent<SpriteRenderer>();
        sr.color = choosen.selectedColor;
        sr.sortingOrder++;
    }
    private static void OnUnChoose(){
        SpriteRenderer sr = choosen.GetComponent<SpriteRenderer>();
        sr.color = choosen.unselectColor;
        sr.sortingOrder--;
    }

    [ClientRpc]
    void RpcSyncMovement(Vector2Int []path){
        EntityWalker.RpcStartWalk(this,path);
    }

    [Command(requiresAuthority = false)]
    void CmdMovement(Vector3Int to){
        Vector2Int []path = HexManager.pathFinder.PathFind(tileCoord,(Vector2Int)to,myTileMap);
        if(path != null) {
            RpcSyncMovement(path);
        }
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0)&&IsChoosen&&PlayerInstanceBehaviour.myInstance.IsMyTurn){
            CmdMovement(myTileMap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
        }
    }
    void Awake()
    {
        TileCoord = (Vector2Int)(myTileMap.WorldToCell(transform.position));
        transform.position = myTileMap.CellToWorld((Vector3Int)TileCoord);
        unselectColor = GetComponent<SpriteRenderer>().color;
    }
}
