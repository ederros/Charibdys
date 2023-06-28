using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[System.Serializable]
public abstract class EntityCommand
{
    protected readonly EntityBehaviour subject;

    protected EntityCommand(EntityBehaviour subject){
        this.subject = subject;
    }
    
    [ClientRpc]
    protected abstract void Invoke();

    public abstract uint GetCost();

    public bool TryInvoke(){
        if(!subject.TrySpendTurns(GetCost())) return false;
        Invoke();
        return true;
    }
}
