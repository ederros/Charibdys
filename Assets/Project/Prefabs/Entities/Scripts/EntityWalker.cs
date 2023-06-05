using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public static class EntityWalker
{
    static IEnumerator WalkOnPath(EntityBehaviour myEntity, Vector2Int[] path){
        foreach(Vector2Int v in path){
            myEntity.TileCoord = v;
            myEntity.transform.position = myEntity.myTileMap.CellToWorld((Vector3Int)myEntity.TileCoord);
            yield return new WaitForSeconds(0.2f);
        }
        isWalking = false;

    }
    static bool isWalking = false;
    
    static public bool RpcStartWalk(EntityBehaviour myEntity, Vector2Int []path){
        if(isWalking == true) return false;
        isWalking = true;
        myEntity.StartCoroutine(WalkOnPath(myEntity, path));
        return true;
    }
}
