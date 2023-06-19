using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayersManager : NetworkBehaviour
{

    private List<PlayerInstanceBehaviour> players = new List<PlayerInstanceBehaviour>();
    private static PlayersManager instance;
    public static PlayersManager Instance{
        get{
            return instance;
        }
    }
    private int playerTurn = 0;
    
    [Command(requiresAuthority = false)]
    public void NextTurn(){
        EntityBehaviour.RefreshTurns();
        players[playerTurn].IsMyTurn = false;
        playerTurn ++;
        playerTurn %= players.Count;
        while(players[playerTurn] == null){
            playerTurn ++;
            playerTurn %= players.Count;
        }
        players[playerTurn].IsMyTurn = true;
    }
    public PlayerInstanceBehaviour GetPlayer(int index){
        if(index>=players.Count) return null;
        return players[index];
    }

    public void AddPlayer(PlayerInstanceBehaviour player){
        int i;
        for(i=0;i<players.Count;i++){
            if(players[i]==null){
                players[i] = player;
                break;
            }
        }
        if(i==players.Count)
            players.Add(player);
    }

    void Awake()
    {
        instance = this;
    }
}
