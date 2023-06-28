using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;
public class CommandsInvoker
{
    
    private EntityBehaviour objectEntity;

    public CommandsInvoker(EntityBehaviour entity){
        objectEntity = entity;
    }   
    public static UnityEvent OnCommandsCompleted = new UnityEvent();

    [ClientRpc]
    public void RpcOnCommandsCompleted()=>OnCommandsCompleted.Invoke();

    public static UnityEvent<EntityCommand> OnNextCommand = new UnityEvent<EntityCommand>();

    [ClientRpc]
    public static void RpcOnNextCommand(EntityCommand cmd)=>OnNextCommand.Invoke(cmd);
    public static EntityBehaviour walkingEntity = null;

    private void CommandInvoke(EntityCommand command){
        command.TryInvoke();
        RpcOnNextCommand(command);
    }
    private IEnumerator WalkOnPath(EntityCommand[] sequence){
        walkingEntity = objectEntity;
        RpcIsWalkingSet(true);
        foreach(EntityCommand v in sequence){
            CommandInvoke(v);
            yield return new WaitForSeconds(0.2f);
        }
        RpcOnCommandsCompleted();
        RpcIsWalkingSet(false);
        walkingEntity = null;
    }
    
    static bool isWalking = false;

    [ClientRpc]
    static void RpcIsWalkingSet(bool val){
        isWalking = val;
    }
    static public bool IsWalking{
        get{
            return isWalking;
        }
    }
    
    public bool StartWalk(EntityCommand []sequence){
        if(isWalking == true||objectEntity == null) return false;
        RpcIsWalkingSet(true);
        Debug.Log(objectEntity);
        objectEntity.StartCoroutine(WalkOnPath(sequence));
        return true;
    }
}
