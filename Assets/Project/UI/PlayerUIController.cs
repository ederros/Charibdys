using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    public void NextTurnButton(){
        if(PlayerInstanceBehaviour.myInstance.IsMyTurn){
            PlayersManager.Instance.NextTurn();
        }
    }
}
