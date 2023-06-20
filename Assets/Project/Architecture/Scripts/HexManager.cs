using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class HexManager
{
    public static readonly PathFinder pathFinder = new HexPathFinder();
    public static readonly List<Vector2Int> secondDirs = new List<Vector2Int>{
        new Vector2Int(0,1),
        new Vector2Int(1,0),
        new Vector2Int(0,-1),
        new Vector2Int(-1,-1),
        new Vector2Int(-1,0),
        new Vector2Int(-1,1)
    };
    public static readonly List<Vector2Int> primaryDirs = new List<Vector2Int>{
        new Vector2Int(1,1),
        new Vector2Int(1,0),
        new Vector2Int(1,-1),
        new Vector2Int(0,-1),
        new Vector2Int(-1,0),
        new Vector2Int(0,1)
    };
    
    public static Vector2Int DirToVec2(int direction){
        return DirToVec2(0,direction);
    }

    public static Vector2Int DirToVec2(int yPos,int direction){
        if(direction<0||direction>=secondDirs.Count) return Vector2Int.zero;
        if(yPos%2 == 0)
            return secondDirs[direction];
        return primaryDirs[direction];
    }

    public static int Vec2ToDir(Vector2Int direction){
        return secondDirs.IndexOf(direction);
    }
    
    public static T CheckFor<T>(Tilemap tilemap, Vector2Int cell) where T:Component
    {
        Collider2D check = Physics2D.OverlapCircle(tilemap.CellToWorld((Vector3Int)cell), 0.1f);
        if(check == null) return null;
        return check.GetComponent<T>();
    }
    
}
