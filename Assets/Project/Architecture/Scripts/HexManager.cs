using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class HexManager
{
    static List<Vector2Int> secondDirs = new List<Vector2Int>{
        new Vector2Int(0,1),
        new Vector2Int(1,0),
        new Vector2Int(0,-1),
        new Vector2Int(-1,-1),
        new Vector2Int(-1,0),
        new Vector2Int(-1,1)
    };
    static List<Vector2Int> primaryDirs = new List<Vector2Int>{
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

    public static Vector2Int[] GetNeighbors(Vector2Int tileCord){
        Vector2Int[] result = new Vector2Int[secondDirs.Count];
        if(tileCord.y%2 == 0)
            secondDirs.CopyTo(result);
        else 
            primaryDirs.CopyTo(result);

        for(int i = 0; i < secondDirs.Count; i++){
            result[i] += tileCord;
        }
        return result;
    }
    struct graphPoint{
        public Vector2Int coord;
        public float cost;
        public float distanceToEnd;

        public float FullCost{
            get{
                return cost + distanceToEnd;
            }
        }
        public graphPoint(Vector2Int coord, float cost, float distance){
             this.coord = coord;
             this.cost = cost;
             this.distanceToEnd = distance;
        }
    }

    private static float HexDistance(Vector2Int a, Vector2Int b, Tilemap tilemap){
        return (tilemap.CellToWorld((Vector3Int)a) - tilemap.CellToWorld((Vector3Int)b)).magnitude;
    }
    public static Vector2Int[] PathFind(Vector2Int from, Vector2Int to, Tilemap tilemap){
        if(from == to) return null;
        List<graphPoint> openPoints = new List<graphPoint>();
        Dictionary<Vector2Int, Vector2Int> pathsToStart = new Dictionary<Vector2Int, Vector2Int>();
        openPoints.Add(new graphPoint(to, 0, HexDistance(to, from, tilemap)));
        pathsToStart.Add(to,to);
        bool isPathFinded = false;
        while(openPoints.Count>0&&!isPathFinded){
            graphPoint curPoint =  openPoints[0];
            Vector2Int[] neighbors = GetNeighbors(openPoints[0].coord);
            
            for(int i = 0;i< neighbors.Length;i++){
                if(pathsToStart.ContainsKey(neighbors[i])) continue;
                FloorTile tile = tilemap.GetTile<FloorTile>((Vector3Int)neighbors[i]);
                if(tile == null) continue;
                pathsToStart.Add(neighbors[i], curPoint.coord);
                float deltaCost = tile.turnsRequired;

                graphPoint graphNeighbor = new graphPoint(neighbors[i],curPoint.cost + deltaCost, HexDistance(neighbors[i],to,tilemap)*tilemap.layoutGrid.cellSize.x);
                int insertedIndex;
                for(insertedIndex = 0; insertedIndex<openPoints.Count; insertedIndex++){
                    if(openPoints[insertedIndex].FullCost>graphNeighbor.FullCost) break;
                }

                openPoints.Insert(insertedIndex, graphNeighbor);
                if(neighbors[i] == from) {
                    isPathFinded = true;
                    break;
                }
            }
            openPoints.Remove(curPoint);
        }
        if(!isPathFinded) return null;

        List<Vector2Int> result = new List<Vector2Int>();
        result.Add(from);
        while(result[result.Count-1]!= to){
            result.Add(pathsToStart[result[result.Count-1]]);
        }
        return result.ToArray();
    }
}
