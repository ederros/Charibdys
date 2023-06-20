using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class UnitController : EntityBehaviour, ITargetable
{
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
            if(CursorController.path==null) return;
            CmdMovement((Vector3Int)CursorController.path[CursorController.path.Length-1]);
        }
    }

    public void Interaction(EntityBehaviour entity)
    {
        if(entity.CheckAffiliation()) return;
    }
}
