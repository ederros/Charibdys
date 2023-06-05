using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerInstanceBehaviour : NetworkBehaviour
{
    public static PlayerInstanceBehaviour myInstance;

   
    void Start()
    {
        if(!isLocalPlayer) return;
        myInstance = this;
    }
    
    
}
