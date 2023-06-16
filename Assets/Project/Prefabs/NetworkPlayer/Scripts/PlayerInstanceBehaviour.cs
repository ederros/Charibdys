using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerInstanceBehaviour : NetworkBehaviour
{
    
    public static PlayerInstanceBehaviour myInstance;
    
    [SyncVar][SerializeField]
    private bool isMyTurn;
    public bool IsMyTurn{
        get{
            return isMyTurn;
        }
        set{
            isMyTurn = value;
        }
    }

    private void Awake() {
        if(!isLocalPlayer) return;
    } 

    void OnDestroy()
    {
        
    }

    void Start()
    {
        if(isLocalPlayer) {
            myInstance = this;
        }
        PlayersManager.Instance.AddPlayer(this);
        if(isServer){
            PlayersManager.Instance.NextTurn();
        }
    }
    
    
}
