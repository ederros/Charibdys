using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Mirror;

public class EntityBehaviour : NetworkBehaviour
{


    [SerializeField]
    public Color selectedColor;

    [HideInInspector]
    public Color unselectColor;
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
    
    [SerializeReference]
    public GameValue hp = new GameValue(100);

    public Tilemap myTileMap;
    protected Vector2Int tileCoord = Vector2Int.zero;
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

        public int affiliation;

    private static List<EntityBehaviour> allEntities = new List<EntityBehaviour>();
    [SyncVar]
    public uint turnsPerRound;

    [SyncVar][SerializeField]
    uint currentTurns;

    public uint CurrentTurns{
        get{
            return currentTurns;
        }
    }
    public bool TrySpendTurns(uint turnsToSpend){
        if(turnsToSpend>currentTurns) return false;
        currentTurns-=turnsToSpend;
        
        return true;
    }
    public static void RefreshTurns(){
        foreach(EntityBehaviour e in allEntities){
            if(e.CheckAffiliation())
                e.currentTurns = e.turnsPerRound;
        }
    }

    public bool CheckAffiliation() => PlayersManager.Instance.GetPlayer(affiliation)==PlayerInstanceBehaviour.myInstance; // returns true if this unit affiliates to local player 

    void TryChoose(){
        PlayerInstanceBehaviour player = PlayersManager.Instance.GetPlayer(affiliation);
        if(player!=null&&player.isLocalPlayer)
            Choosen = this;
    }

    void OnMouseDown()
    {
        TryChoose();
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

    
    void OnDestroy()
    {
        allEntities.Remove(this);
    }
    void Awake()
    {
        TileCoord = (Vector2Int)(myTileMap.WorldToCell(transform.position));
        transform.position = myTileMap.CellToWorld((Vector3Int)TileCoord);
        unselectColor = GetComponent<SpriteRenderer>().color;
        allEntities.Add(this);
    }
}
