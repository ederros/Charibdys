using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public abstract class PathFinder
{
    
    public float[] PathCosts(Vector2Int []path, Tilemap Tmap){
        List<float> costs = new List<float>();
        float totalCost = 0;
        costs.Add(0);
        for(int i = 1;i<path.Length;i++){
            totalCost += Tmap.GetTile<FloorTile>((Vector3Int)path[i]).turnsRequired;
            costs.Add(totalCost);
        }
        
        return costs.ToArray();
    }

    public Vector2Int[] TrimPathByCost(Vector2Int[] path, float[] costs, float maxCost){
        int i;
        List<Vector2Int> newPath = new List<Vector2Int>();
        for(i = 0;i < path.Length;i++){
            if(costs[i]>maxCost) break;
            newPath.Add(path[i]);
        }
        return newPath.ToArray();
    }
    
    public abstract Vector2Int[] GetNeighbors(Vector2Int tileCord);
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
    public Vector2Int[] PathFind(Vector2Int from, Vector2Int to, Tilemap tilemap){//A* method
        if(from == to) return null;
        if(tilemap.GetTile<FloorTile>((Vector3Int)to) == null) return null;
        if(HexManager.CheckFor<EntityBehaviour>(tilemap,to)!=null) return null;
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
