using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;
public static class EntityWalker
{
    public static UnityEvent OnWalkerReach = new UnityEvent();
    public static UnityEvent OnWalkerStep = new UnityEvent();
    static IEnumerator WalkOnPath(EntityBehaviour myEntity, Vector2Int[] path){
        EntityBehaviour.walkingEntity = myEntity;
        foreach(Vector2Int v in path){
            myEntity.TileCoord = v;
            myEntity.transform.position = myEntity.myTileMap.CellToWorld((Vector3Int)myEntity.TileCoord);
            
            OnWalkerStep.Invoke();
            yield return new WaitForSeconds(0.2f);
        }
        OnWalkerReach.Invoke();
        isWalking = false;
        EntityBehaviour.walkingEntity = null;
    }
    static bool isWalking = false;
    static public bool IsWalking{
        get{
            return isWalking;
        }
    }
    static public bool RpcStartWalk(EntityBehaviour myEntity, Vector2Int []path){
        if(isWalking == true) return false;
        isWalking = true;
        myEntity.StartCoroutine(WalkOnPath(myEntity, path));
        return true;
        
    }
}
