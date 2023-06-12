using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerInstanceBehaviour : NetworkBehaviour
{
    
    public static PlayerInstanceBehaviour myInstance;
    public static List<PlayerInstanceBehaviour> players;
    
    
    private void Awake() {
        if(isServer){
            int i;
            for(i = 0; i<players.Count;i++){
                
            }
            //players
        }
    } 
    
    [Command]
    void AddToServer(){
        
    }

    void Start()
    {
        if(!isLocalPlayer) return;
        myInstance = this;
        
    }
    
    
}
