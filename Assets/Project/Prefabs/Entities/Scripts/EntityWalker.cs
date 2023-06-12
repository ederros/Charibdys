using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public static class EntityWalker
{
    public enum events{
        OnWalkerReach,
        OnWalkerStep
    }
    static Observer<events> eventListener = new Observer<events>();
    static public Observer<events> EventListener{
        get{
            return eventListener;
        }
    }
    static IEnumerator WalkOnPath(EntityBehaviour myEntity, Vector2Int[] path){
        foreach(Vector2Int v in path){
            myEntity.TileCoord = v;
            myEntity.transform.position = myEntity.myTileMap.CellToWorld((Vector3Int)myEntity.TileCoord);
            EventListener.Notify(events.OnWalkerStep);
            yield return new WaitForSeconds(0.2f);
        }
        EventListener.Notify(events.OnWalkerReach);
        isWalking = false;

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
