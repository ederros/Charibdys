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
    
    public static Vector2Int[] GetPositionsInRadius(Vector2Int center, int radius){
        return GetPositionsInRadius(center,radius,2);
    }
    public static Vector2Int[] GetPositionsInRadius(Vector2Int center, int radius, int rotationOffset){
        Vector2Int[] result = new Vector2Int[1+(6+6*radius)/2*radius];
        result[0] = center;
        int index = 0;
        for(int i = 1; i<= radius;i++){
            for(int i2 = 0;i2< 6;i2++){
                Vector2Int startPos = center;
                for(int i3 = 0; i3< i;i3++){
                    startPos += DirToVec2(startPos,i2);
                }
                for(int i3 = 0; i3< i;i3++){
                    index++;
                    result[index] = startPos;
                    startPos += DirToVec2(startPos,i2+rotationOffset);
                }
            }
        }
        return result;
    }
    public static Vector2Int DirToVec2(Vector2Int fromPos, int direction) => DirToVec2(fromPos.y, direction);
    public static Vector2Int DirToVec2(int yPos,int direction){
        direction = direction%6;
        if(direction<0||direction>=secondDirs.Count) return Vector2Int.zero;
        if(yPos%2 == 0)
            return secondDirs[direction];
        return primaryDirs[direction];
    }

    public static int Vec2ToDir(Vector2Int direction) => secondDirs.IndexOf(direction);

    public static T CheckFor<T>(Tilemap tilemap, Vector2Int cell) where T:Component
    {
        Collider2D check = Physics2D.OverlapCircle(tilemap.CellToWorld((Vector3Int)cell), 0.1f);
        if(check == null) return null;
        return check.GetComponent<T>();
    }
    
}
