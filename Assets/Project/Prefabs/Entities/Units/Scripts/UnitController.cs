using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class UnitController : EntityBehaviour, ITargetable
{
    

    [ClientRpc]
    void Movement(Vector2Int [] path){
        List<EntityCommand> cmdSequence = PathFinder.PathToCommands(path, this);
        commandsInvoker.StartWalk(cmdSequence.ToArray());
    }

    [Command(requiresAuthority = false)]
    void CmdMovement(Vector3Int to){
        //Debug.Log("server targ = "+target);
        Vector2Int []path = HexManager.pathFinder.PathFind(tileCoord,(Vector2Int)to,myTileMap);
        if(path == null) return;
        Movement(path);
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0)&&IsChoosen&&PlayerInstanceBehaviour.myInstance.IsMyTurn){
            if(CursorController.cmdSequence==null||CursorController.path.Length==0) return;
            CmdMovement((Vector3Int)CursorController.path[CursorController.path.Length-1]);
        }
    }

    public void Interaction(EntityBehaviour entity)
    {
        if(entity.CheckAffiliation()) return;
    }
}
